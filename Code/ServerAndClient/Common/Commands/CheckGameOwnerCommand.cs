using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.HelperObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class CheckGameOwnerCommand : CommandBase, Interfaces.ICommand
    {
        public CheckGameOwnerCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_CHECKOWNERSHIP_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameUserRelationQueryNetworkTransferObject queryDummy = new GameUserRelationQueryNetworkTransferObject();
            IGameLogic gameLogic = new GameLogic(); 
            
            int statusCode = 0;
            string response = "";

            string queryString = Encoding.UTF8.GetString(payload);
            GameUserRelationQuery query = queryDummy.Decode(queryString);
            bool isOwner = gameLogic.CheckIsOwner(query);

            statusCode = isOwner ? StatusCodeConstants.OK : StatusCodeConstants.ERROR_CLIENT_NOTAUTHORIZED;

            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
    }
}