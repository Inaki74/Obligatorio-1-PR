using System;
using Common.Protocol.Interfaces;
using Domain.HelperObjects;

namespace Common.Protocol.NTOs
{
    public class GameSearchQueryNetworkTransferObject : INetworkTransferObject<GameSearchQuery>
    {
        private GameSearchQuery _gameQuery;
        public GameSearchQuery Decode(string toDecode)
        {
            GameSearchQuery query = new GameSearchQuery();

            int index = 0;
            string title = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string genre = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string score = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_SCOREINPUT_FIXED_SIZE);
            
            query.Title = title;
            query.Genre = genre;
            query.Score = int.Parse(score);

            return query;
        }

        public string Encode()
        {
            string title = _gameQuery.Title;
            string genre = _gameQuery.Genre;
            string score = _gameQuery.Score.ToString();

            string input = "";

            input += VaporProtocolHelper.FillNumber(title.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + title;

            input += VaporProtocolHelper.FillNumber(genre.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + genre;

            input += VaporProtocolHelper.FillNumber(score.Length,VaporProtocolSpecification.GAME_SCOREINPUT_FIXED_SIZE) + score;

            return input;
        }

        public void Load(GameSearchQuery obj)
        {
            _gameQuery = obj;
        }
    }
}
