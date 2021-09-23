using System;
using Common.Protocol.Interfaces;
using Domain.BusinessObjects;

namespace Common.Protocol.NTOs
{
    public class GameNetworkTransferObject : INetworkTransferObject<Game>
    {
        public string OwnerName { get; set; } = "";
        public string Title {get; set;} = "";
        public string Genre {get; set;} = "";
        public string ESRB {get; set;} = "";
        public string Synopsis {get; set;} = "";
        public string CoverPath {get; set;} = "";

        public void Load(Game game)
        {
            OwnerName = game.Owner.Username;
            Title = game.Title;
            Genre = game.Genre;
            ESRB = game.ESRB;
            Synopsis = game.Synopsis;
            CoverPath = game.CoverPath;
        }

        public string Encode()
        {
            string input = "";

            input += VaporProtocolHelper.FillNumber(OwnerName.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + OwnerName;

            input += VaporProtocolHelper.FillNumber(Title.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Title;

            input += VaporProtocolHelper.FillNumber(Genre.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Genre;

            input += VaporProtocolHelper.FillNumber(ESRB.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + ESRB;

            input += VaporProtocolHelper.FillNumber(Synopsis.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Synopsis;

            return input;
        }

        public Game Decode(string toDecode)
        {
            //Desarmar payload en un juego. Agregarlo a la lista.
            // Payload = 
            //  Cada field:
            //        XXXX XXXX... - Largo Info
            // Orden: Titulo -> Genero -> Esrb -> Synopsis -> Caratula
            Game game = new Game();

            int index = 0;
            string username = ExtractGameField(toDecode, ref index);
            string title = ExtractGameField(toDecode, ref index);
            string genre = ExtractGameField(toDecode, ref index);
            string esrb = ExtractGameField(toDecode, ref index);
            string synopsis = ExtractGameField(toDecode, ref index);
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
