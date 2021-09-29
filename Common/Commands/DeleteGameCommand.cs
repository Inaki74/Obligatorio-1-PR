using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class DeleteGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_DELETE_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameNetworkTransferObject gameDummy = new GameNetworkTransferObject();
            int statusCode = 0;
            string response = "";
            try
            {
                Game game = gameDummy.Decode(Encoding.UTF8.GetString(payload));
                IGameLogic gameLogic = new GameLogic(); 
                gameLogic.DeleteGame(game);
                
                statusCode = StatusCodeConstants.OK;
                response = "Game deleted successfully.";

                return statusCode.ToString() + response;
                
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = "Something went wrong! exception: " + e.Message;
                return statusCode.ToString() + response;
            }

        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
    }
}