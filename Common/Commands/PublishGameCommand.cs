using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class PublishGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_PUBLISH_GAME_CODE;

        public string ActionReq(byte[] payload)
        {
            // Armado de juego
            Game game = DisassembleGamePayload(payload);

            IGameLogic gameLogic = new GameLogic();
            
            // Response
            int statusCode = 0;
            string response = "";
            
            int id = gameLogic.AddGame(game);
            statusCode = StatusCodeConstants.OK;

            response = EncodeGameResponse(id);
        
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                statusMessage.SelectedGameId = DecodeGameIdResponse(statusMessage.Message);
            }
            
            return statusMessage;
        }

        private Game DisassembleGamePayload(byte[] payload)
        {
            string payloadAsString = Encoding.UTF8.GetString(payload);

            GameNetworkTransferObject game = new GameNetworkTransferObject();

            return game.Decode(payloadAsString);
        }

        private string EncodeGameResponse(int id)
        {
            Game dummyGame = new Game();
            dummyGame.Id = id;

            GameNetworkTransferObject gameDummyResponse = new GameNetworkTransferObject();
            gameDummyResponse.Load(dummyGame);

            return gameDummyResponse.Encode();
        }

        private int DecodeGameIdResponse(string payload)
        {
            GameNetworkTransferObject gameDummyResponse = new GameNetworkTransferObject();

            return gameDummyResponse.Decode(payload).Id;
        }
    }
}