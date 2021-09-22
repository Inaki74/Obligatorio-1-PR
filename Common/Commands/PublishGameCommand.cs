using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Domain;

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
            
            try
            {
                gameLogic.AddGame(game);
                statusCode = StatusCodeConstants.OK;
                response = "Game published!";
            }
            catch (Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_CLIENT;
                response = $"Something went wrong when publishing your game: {e.Message}";
            }
        
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse<T> ActionRes<T>(byte[] payload)
        {
            VaporStatusResponse<T> statusMessage = ParseStatusResponse<T>(payload);

            statusMessage.Payload = default(T);

            return statusMessage;
        }

        private Game DisassembleGamePayload(byte[] payload)
        {
            //Desarmar payload en un juego. Agregarlo a la lista.
            // Payload = 
            //  Cada field:
            //        XXXX XXXX... - Largo Info
            // Orden: Titulo -> Genero -> Esrb -> Synopsis -> Caratula
            Game game = new Game();
            string payloadAsString = Encoding.UTF8.GetString(payload);

            int index = 0;
            string username = ExtractGameField(payloadAsString, ref index);
            string title = ExtractGameField(payloadAsString, ref index);
            string genre = ExtractGameField(payloadAsString, ref index);
            string esrb = ExtractGameField(payloadAsString, ref index);
            string synopsis = ExtractGameField(payloadAsString, ref index);
            //string caratula = ExtractField(payloadAsString, ref index);

            game.Owner = new User(username, -1);
            game.Title = title;
            game.Genre = genre;
            game.ESRB = esrb;
            game.Synopsis = synopsis;
            // caratula

            return game;
        }

        private string ExtractGameField(string payload, ref int index)
        {
            int length = int.Parse(payload.Substring(index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE));
            index += VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE;
            string field = payload.Substring(index, length);
            index += length;

            return field;
        }
    }
}