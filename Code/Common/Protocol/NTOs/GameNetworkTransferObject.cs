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

            input += VaporProtocolHelper.FillNumber(IdAsString.Length,VaporProtocolSpecification.GAME_ID_MAXSIZE) + IdAsString;

            input += VaporProtocolHelper.FillNumber(OwnerName.Length,VaporProtocolSpecification.USER_NAME_MAXSIZE) + OwnerName;

            input += VaporProtocolHelper.FillNumber(Title.Length,VaporProtocolSpecification.GAME_TITLE_MAXSIZE) + Title;

            input += VaporProtocolHelper.FillNumber(Genre.Length,VaporProtocolSpecification.GAME_GENRE_MAXSIZE) + Genre;

            input += VaporProtocolHelper.FillNumber(ESRB.Length,VaporProtocolSpecification.GAME_ESRB_MAXSIZE) + ESRB;

            input += VaporProtocolHelper.FillNumber(Synopsis.Length,VaporProtocolSpecification.GAME_SYNOPSIS_MAXSIZE) + Synopsis;

            return input;
        }

        public Game Decode(string toDecode)
        {
            Game game = new Game();

            int index = 0;
            int id = int.Parse(NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_ID_MAXSIZE));
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.USER_NAME_MAXSIZE);
            string title = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_TITLE_MAXSIZE);
            string genre = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_GENRE_MAXSIZE);
            string esrb = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_ESRB_MAXSIZE);
            string synopsis = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_SYNOPSIS_MAXSIZE);

            game.Id = id;
            game.Owner = new User(username, -1);
            game.Title = title;
            game.Genre = genre;
            game.ESRB = esrb;
            game.Synopsis = synopsis;

            return game;
        }
    }
}
