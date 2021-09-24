using System;
using Common.Protocol.Interfaces;
using Domain.HelperObjects;

namespace Common.Protocol.NTOs
{
    public class GameOwnershipQueryNetworkTransferObject : INetworkTransferObject<GameUserRelationQuery>
    {
        public string Username { get; set; } = "";
        public string Gamename {get; set;} = "";

        public void Load(GameUserRelationQuery game)
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

        public GameUserRelationQuery Decode(string toDecode)
        {
            GameUserRelationQuery deleteQuery = new GameUserRelationQuery();

            int index = 0;
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string gamename = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);

            deleteQuery.Username = username;
            deleteQuery.Gamename = gamename;

            return deleteQuery;
        }
    }
}