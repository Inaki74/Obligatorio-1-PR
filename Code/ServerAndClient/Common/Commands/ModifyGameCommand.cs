using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class ModifyGameCommand : CommandBase, Interfaces.ICommand
    {
        public ModifyGameCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_MODIFY_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            IGameLogic gameLogic = new GameLogic();
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            
            int statusCode = 0;
            string response = "";
            
            string gameString = Encoding.UTF8.GetString(payload);
            Game game = gameNTO.Decode(gameString);
            Game oldGame = gameLogic.GetGame(game.Id);

            string logMessage = "";
            if(oldGame.Title != game.Title)
            {
                logMessage = "The game " + game.Title + " has been modified by its owner: " + game.Owner.Username + ". The game's name changed from " + oldGame.Title + " to " + game.Title;
            }
            else
            {
                logMessage = "The game " + game.Title + " has been modified by its owner: " + game.Owner.Username;
            }

            gameLogic.ModifyGame(game);

            
            SendLog(game.Owner.Username, game.Id, logMessage);
            
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