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
            IGameLogic gameLogic = new GameLogic();
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            
            int statusCode = 0;
            string response = "";
            
            string gameString = Encoding.UTF8.GetString(payload);
            Game game = gameNTO.Decode(gameString);
            gameLogic.ModifyGame(game);
            
            statusCode = StatusCodeConstants.OK;
            response = "Game modified!";

            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
        
    }
}