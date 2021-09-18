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
    public class PublishGameCommand : Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_LOGIN_CODE;

        public string ActionReq(byte[] payload)
        {
            // Armado de juego
            Game game = DisassembleGamePayload(payload);

            IGameLogic gameLogic = new GameLogic();
            gameLogic.AddGame(game);

            // Response
            int statusCode = 0;
            string response = "";

            

            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] payload)
        {
            // Mensaje de si se publico el juego
            string payloadString = Encoding.UTF8.GetString(payload);
            int statusCode = int.Parse(payloadString.Substring(0, VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE));
            string message = payloadString.Substring(VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE, payloadString.Length-VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE);
            string response = "";

            switch(statusCode)
            {
                case StatusCodeConstants.OK:
                    response = "Logged in!";
                    break;
                case StatusCodeConstants.INFO:
                    response = "User didn't exist, created new user.";
                    break;
                case StatusCodeConstants.ERROR_CLIENT:
                    response = message;
                    break;
            }

            return new VaporStatusResponse(statusCode, response);
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
            string title = ExtractGameField(payloadAsString, ref index);
            string genre = ExtractGameField(payloadAsString, ref index);
            string esrb = ExtractGameField(payloadAsString, ref index);
            string synopsis = ExtractGameField(payloadAsString, ref index);
            //string caratula = ExtractField(payloadAsString, ref index);

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