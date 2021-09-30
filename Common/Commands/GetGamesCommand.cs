using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class GetGamesCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_GET_GAMES_CODE;
        
        public string ActionReq(byte[] payload)
        {
            int statusCode = 0;
            string response = "";

            IGameLogic gameLogic = new GameLogic();
            ListNetworkTransferObject<Game> listNTO = new ListNetworkTransferObject<Game>(new GameNetworkTransferObject());
            
            try
            {
                List<Game> allGames = gameLogic.GetAllGames();
                statusCode = StatusCodeConstants.OK;
                listNTO.Load(allGames);
                response = listNTO.Encode();

                return statusCode.ToString() + response;
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Something went wrong server-side: {e.Message}";
                return statusCode.ToString() + response;
            }
        }
        
        
        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);
            ListNetworkTransferObject<Game> listNTO = new ListNetworkTransferObject<Game>(new GameNetworkTransferObject());
            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                statusMessage.GamesList = listNTO.Decode(statusMessage.Message);
            }
            
            return statusMessage;
        }
    }
}
