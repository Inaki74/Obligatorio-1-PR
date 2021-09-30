using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Configuration;
using Common.Configuration.Interfaces;
using Common.FileSystemUtilities;
using Common.FileSystemUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class DeleteGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_DELETE_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameNetworkTransferObject gameDummy = new GameNetworkTransferObject();
            int statusCode = 0;
            string response = "";
            
            IFileStreamHandler fileStreamHandler = new FileStreamHandler();
                
            Game game = gameDummy.Decode(Encoding.UTF8.GetString(payload));
            IGameLogic gameLogic = new GameLogic(); 
            gameLogic.DeleteGame(game);
            
            //Deleting image from server
            IPathHandler pathHandler = new PathHandler();
            IConfigurationHandler configurationHandler = new ConfigurationHandler();
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