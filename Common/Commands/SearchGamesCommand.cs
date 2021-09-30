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

namespace Common.Commands
{
    public class SearchGamesCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_SEARCH_GAMES_CODE;
        
        public string ActionReq(byte[] payload)
        {
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            ListNetworkTransferObject<Game> listNTO = new ListNetworkTransferObject<Game>(gameNTO);
            IGameLogic gameLogic = new GameLogic();

            int statusCode = 0;
            string response = "";
            
            try
            {
                GameSearchQueryNetworkTransferObject queryNTO = new GameSearchQueryNetworkTransferObject();
                GameSearchQuery query = queryNTO.Decode(Encoding.UTF8.GetString(payload));

                List<Game> coincidences = gameLogic.SearchGames(query);
                listNTO.Load(coincidences);
                response = listNTO.Encode();
                
                statusCode = StatusCodeConstants.OK;
                
                return statusCode.ToString() + response;
            }
            catch(Exception e) //TODO: Ver posibles errores del parte del cliente.
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Something went wrong server-side: {e.Message} + {e.StackTrace}";
                return statusCode.ToString() + response;
            }
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
