using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class GetGamesCommand : CommandBase, ICommand
    {
        public GetGamesCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_GET_GAMES_CODE;
        
        public string ActionReq(byte[] payload)
        {
            int statusCode = 0;
            string response = "";

            IGameLogic gameLogic = new GameLogic();
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            ListNetworkTransferObject<Game> listNTO = new ListNetworkTransferObject<Game>(gameNTO);
            
            List<Game> allGames = gameLogic.GetAllGames();
            listNTO.Load(allGames);
            response = listNTO.Encode();
             
            statusCode = StatusCodeConstants.OK;
             
            return statusCode.ToString() + response;
        }
        
        
        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);
         
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            ListNetworkTransferObject<Game> listNTO = new ListNetworkTransferObject<Game>(gameNTO);
            
            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                statusMessage.GamesList = listNTO.Decode(statusMessage.Message);
            }
            
            return statusMessage;
        }
    }
}
