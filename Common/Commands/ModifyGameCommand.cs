using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class ModifyGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_MODIFY_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            Game game = DisassembleGamePayload(payload);
            IGameLogic gameLogic = new GameLogic();
            int statusCode = 0;
            string response = "";
            try
            {
                gameLogic.ModifyGame(game);
                statusCode = StatusCodeConstants.OK;
                response = "Game modified!";
            }
            catch (Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_CLIENT;
                response = $"Something went wrong when modifying your game: {e.Message}";
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