using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.HelperObjects;
using Domain.BusinessObjects;
using Exceptions.BusinessExceptions;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class SearchGamesCommand : CommandBase, ICommand
    {
        public SearchGamesCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_SEARCH_GAMES_CODE;
        
        public string ActionReq(byte[] payload)
        {
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            ListNetworkTransferObject<Game> listNTO = new ListNetworkTransferObject<Game>(gameNTO);
            GameSearchQueryNetworkTransferObject queryNTO = new GameSearchQueryNetworkTransferObject();
            IGameLogic gameLogic = new GameLogic();

            int statusCode = 0;
            string response = "";

            string queryString = Encoding.UTF8.GetString(payload);
            GameSearchQuery query = queryNTO.Decode(queryString);

            List<Game> coincidences = gameLogic.SearchGames(query);
            listNTO.Load(coincidences);
            response = listNTO.Encode();
                
            statusCode = StatusCodeConstants.OK;
                
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            ListNetworkTransferObject<Game> listNTO = new ListNetworkTransferObject<Game>(gameNTO);
            
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                statusMessage.GamesList = listNTO.Decode(statusMessage.Message);
            }
            
            return statusMessage;
        }
        
    }
}
