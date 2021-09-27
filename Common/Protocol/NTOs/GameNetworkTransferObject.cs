using System;
using Common.Protocol.Interfaces;
using Domain.BusinessObjects;

namespace Common.Protocol.NTOs
{
    public class GameNetworkTransferObject : INetworkTransferObject<Game>
    {
        public int ID { get; set; } = -1;
        public string OwnerName { get; set; } = "";
        public string Title {get; set;} = "";
        public string Genre {get; set;} = "";
        public string ESRB {get; set;} = "";
        public string Synopsis {get; set;} = "";
        public string CoverPath {get; set;} = "";

        public void Load(Game game)
        {
            ID = game.Id;
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
            string IdAsString = ID.ToString();

            input += VaporProtocolHelper.FillNumber(IdAsString.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + IdAsString;

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
            int id = int.Parse(NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE));
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string title = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string genre = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string esrb = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string synopsis = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            //string caratula = ExtractField(payloadAsString, ref index);

            game.Id = id;
            game.Owner = new User(username, -1);
            game.Title = title;
            game.Genre = genre;
            game.ESRB = esrb;
            game.Synopsis = synopsis;
            // caratula

            return game;
        }
    }
}
