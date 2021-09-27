using System;
using Common.Protocol.Interfaces;
using Domain.HelperObjects;

namespace Common.Protocol.NTOs
{
    public class GameUserRelationQueryNetworkTransferObject : INetworkTransferObject<GameUserRelationQuery>
    {
        public string Username { get; set; } = "";
        public int Gameid {get; set;} = -1;

        public void Load(GameUserRelationQuery game)
        {
            Username = game.Username;
            Gameid = game.Gameid;
        }

        public string Encode()
        {
            string input = "";
            string gameIdAsString = Gameid.ToString();

            input += VaporProtocolHelper.FillNumber(Username.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Username;

            input += VaporProtocolHelper.FillNumber(gameIdAsString.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + gameIdAsString;

            return input;
        }

        public GameUserRelationQuery Decode(string toDecode)
        {
            GameUserRelationQuery deleteQuery = new GameUserRelationQuery();

            int index = 0;
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            int gameid = int.Parse(NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE));

            deleteQuery.Username = username;
            deleteQuery.Gameid = gameid;

            return deleteQuery;
        }
    }
}