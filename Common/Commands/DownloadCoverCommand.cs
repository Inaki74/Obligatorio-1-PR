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
            IGameLogic gameLogic = new GameLogic();
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();

            int statusCode = 0;
            string response = "";

            try
            {
                Game gameDummy = DisassembleGamePayload(payload);
                Game realGame = gameLogic.GetGame(gameDummy.Id);
                gameNTO.Load(realGame);
                statusCode = StatusCodeConstants.OK;
                response = gameNTO.Encode();
            }
            catch (Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = e.Message;
            }
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
        
        private Game DisassembleGamePayload(byte[] payload)
        {
            string payloadAsString = Encoding.UTF8.GetString(payload);

            GameNetworkTransferObject game = new GameNetworkTransferObject();

            return game.Decode(payloadAsString);
        }
    }
}