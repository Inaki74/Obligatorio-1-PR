using System;
using Common.Protocol.Interfaces;
using Domain.HelperObjects;

namespace Common.Protocol.NTOs
{
    public class GameDeleteQueryNetworkTransferObject : INetworkTransferObject<GameDeleteQuery>
    {
        public string Username { get; set; } = "";
        public string Gamename {get; set;} = "";

        public void Load(GameDeleteQuery game)
        {
            Username = game.Username;
            Gamename = game.Gamename;
        }

        public string Encode()
        {
            string input = "";

            input += VaporProtocolHelper.FillNumber(Username.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Username;

            input += VaporProtocolHelper.FillNumber(Gamename.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Gamename;

            return input;
        }

        public GameDeleteQuery Decode(string toDecode)
        {
            GameDeleteQuery deleteQuery = new GameDeleteQuery();

            int index = 0;
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index);
            string gamename = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index);

            deleteQuery.Username = username;
            deleteQuery.Gamename = gamename;

            return deleteQuery;
        }
    }
}

