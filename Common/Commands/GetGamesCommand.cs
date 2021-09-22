using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain;

namespace Common.Commands
{
    public class GetGamesCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_GET_GAMES_CODE;

        // Lo que hace el server.
        public string ActionReq(byte[] payload)
        {
            //conseguir todos los juegos
            int statusCode = 0;
            string response = "";

            IGameLogic gameLogic = new GameLogic();
            try
            {
                List<Game> allGames = gameLogic.GetAllGames();
                statusCode = StatusCodeConstants.OK;
                response = "All games obtained.";

                return statusCode.ToString() + response + EncodeGameList(allGames);
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Something went wrong server-side: {e.Message}";
                return statusCode.ToString() + response;
            }
        }

        // Lo que hace el cliente.
        public VaporStatusResponse<T> ActionRes<T>(byte[] payload)
        {
            VaporStatusResponse<T> statusMessage = ParseStatusResponse<T>(payload);

            statusMessage.Payload = default(T);

            return statusMessage;
        }

        private string EncodeGameList(List<Game> games)
        {
            // CANT-JUEGOS JUEGO(1) JUEGO(2) ... JUEGO(CANT-JUEGOS)
            string encoded = "";
            int cantJuegos = games.Count;
            encoded += VaporProtocolHelper.FillNumber(cantJuegos, VaporProtocolSpecification.GAMES_MAX_AMOUNT_FIXED_SIZE);

            foreach(Game game in games)
            {
                GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
                gameNTO.Load(game);
                encoded += gameNTO.Encode();
            }

            return encoded;
        }

        private void DecodeGameList(string data)
        {
            
        }
    }
}
