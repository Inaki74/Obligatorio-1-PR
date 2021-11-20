using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Business;
using BusinessInterfaces;
using Configuration;
using Configuration.Interfaces;
using Common.FileSystemUtilities;
using Common.FileSystemUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class DeleteGameCommand : CommandBase, Interfaces.ICommand
    {
        public DeleteGameCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_DELETE_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameNetworkTransferObject gameDummy = new GameNetworkTransferObject();
            IFileStreamHandler fileStreamHandler = new FileStreamHandler();
            IGameLogic gameLogic = new GameLogic(); 
            
            int statusCode = 0;
            string response = "";
            
            string gameString = Encoding.UTF8.GetString(payload);
            Game game = gameDummy.Decode(gameString);

            Game gameInDB = gameLogic.GetGame(game.Id);
            string gameownerUsername = gameInDB.Owner.Username;
            int gameId = gameInDB.Id;
            string logMessage = "The game " + gameInDB.Title + " has been deleted by its owner: " + gameownerUsername;
            SendLog(gameownerUsername, gameId, logMessage);

            gameLogic.DeleteGame(game);

            IConfigurationHandler configurationHandler = new ConfigurationHandler();
            IPathHandler pathHandler = new PathHandler();
            
            string path = pathHandler.AppendPath(configurationHandler.GetPathFromAppSettings(),$"{game.Id}.png");
            fileStreamHandler.Delete(path);
            
            statusCode = StatusCodeConstants.OK;
            response = "Game deleted successfully.";
            
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
        
    }
}