using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.HelperObjects;

namespace Common.Commands
{
    public class DeleteGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_DELETE_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameDeleteQueryNetworkTransferObject queryDummy = new GameDeleteQueryNetworkTransferObject();
            int statusCode = 0;
            string response = "";
            try
            {
                GameDeleteQuery deleteQuery = queryDummy.Decode(Encoding.UTF8.GetString(payload));
                IGameLogic gameLogic = new GameLogic(); 
                //gameLogic.DeleteGame(deleteQuery);

                statusCode = StatusCodeConstants.OK;

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