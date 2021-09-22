using System;
using System.Collections.Generic;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Domain;

namespace Common.Commands
{
    public class SearchGamesCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_SEARCH_GAMES_CODE;

        // Get games list
        public string ActionReq(byte[] payload)
        {
            //conseguir todos los juegos
            int statusCode = 0;
            string response = "";

            IGameLogic gameLogic = new GameLogic();
            try
            {
                List<Game> coincidences = gameLogic.SearchGames();
                statusCode = StatusCodeConstants.OK;
                response = EncodeGameList(coincidences);

                return statusCode.ToString() + response;
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Something went wrong server-side: {e.Message}";
                return statusCode.ToString() + response;
            }
        }

        // Decode games list
        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                statusMessage.GamesList = DecodeGameList(statusMessage.Message);
            }
            
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

        private List<Game> DecodeGameList(string data)
        {
            List<Game> ret = new List<Game>();
            int cantJuegos = int.Parse(data.Substring(0, VaporProtocolSpecification.GAMES_MAX_AMOUNT_FIXED_SIZE));
            string restOfData = data.Substring(VaporProtocolSpecification.GAMES_MAX_AMOUNT_FIXED_SIZE, data.Length - VaporProtocolSpecification.GAMES_MAX_AMOUNT_FIXED_SIZE);

            for(int i = 0; i < cantJuegos; i++)
            {
                GameNetworkTransferObject game = new GameNetworkTransferObject();

                ret.Add(game.Decode(restOfData));
            }
            return ret;
        }
    }
}
