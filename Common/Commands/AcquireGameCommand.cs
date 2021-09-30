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
            int statusCode = 0;
            string response = "";
            
            GameUserRelationQuery query = queryDummy.Decode(Encoding.UTF8.GetString(payload));
            IGameLogic gameLogic = new GameLogic();
            bool gameSuccesfullyAcquired = gameLogic.AcquireGame(query);
            if (gameSuccesfullyAcquired)
            {
                statusCode = StatusCodeConstants.OK;
                response = "Game acquired succesfully";
            }
            else
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = "Something went wrong!";
            }
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
    }
}