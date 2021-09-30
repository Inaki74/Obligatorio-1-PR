using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace Common.Commands
{
    public class AcquireGameCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_ACQUIRE_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameUserRelationQueryNetworkTransferObject queryDummy = new GameUserRelationQueryNetworkTransferObject();
            IGameLogic gameLogic = new GameLogic();
            
            int statusCode = 0;
            string response = "";

            string queryString = Encoding.UTF8.GetString(payload);
            GameUserRelationQuery query = queryDummy.Decode(queryString);
            gameLogic.AcquireGame(query);

            statusCode = StatusCodeConstants.OK;
            response = "Game acquired succesfully";

            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
    }
}