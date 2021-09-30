using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class DownloadCoverCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_DOWNLOAD_COVER_CODE;
        public string ActionReq(byte[] payload)
        {
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            IGameLogic gameLogic = new GameLogic();
            
            int statusCode = 0;
            string response = "";
            
            string payloadAsString = Encoding.UTF8.GetString(payload);
            Game gameDummy = gameNTO.Decode(payloadAsString);
            Game realGame = gameLogic.GetGame(gameDummy.Id);
            
            gameNTO.Load(realGame);
            response = gameNTO.Encode();
            
            statusCode = StatusCodeConstants.OK;

            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
        
    }
}