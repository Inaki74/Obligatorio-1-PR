using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.HelperObjects;

namespace Common.Commands
{
    public class CheckGameOwnerCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_CHECKOWNERSHIP_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameUserRelationQueryNetworkTransferObject queryDummy = new GameUserRelationQueryNetworkTransferObject();
            int statusCode = 0;
            string response = "";
            try
            {
                GameUserRelationQuery query = queryDummy.Decode(Encoding.UTF8.GetString(payload));
                IGameLogic gameLogic = new GameLogic(); 
                bool isOwner = gameLogic.CheckIsOwner(query);

                statusCode = isOwner ? StatusCodeConstants.OK : StatusCodeConstants.ERROR_CLIENT_NOTAUTHORIZED;

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