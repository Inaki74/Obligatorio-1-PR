using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class PublishGameCommand : CommandBase, Interfaces.ICommand
    {
        public PublishGameCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_PUBLISH_GAME_CODE;

        public string ActionReq(byte[] payload)
        {
            IGameLogic gameLogic = new GameLogic();
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            
            string gameString = Encoding.UTF8.GetString(payload);
            Game game = gameNTO.Decode(gameString);
            
            int statusCode = 0;
            string response = "";
            
            int id = gameLogic.AddGame(game);
            game.Id = id;
            gameNTO.Load(game);
            response = gameNTO.Encode();
        
            statusCode = StatusCodeConstants.OK;
            
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            
            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                Game game = gameNTO.Decode(statusMessage.Message);
                statusMessage.SelectedGameId = game.Id;
            }
            
            return statusMessage;
        }

    }
}