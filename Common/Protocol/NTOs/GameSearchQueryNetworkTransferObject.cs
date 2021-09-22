using System;
using Common.Protocol.Interfaces;
using Domain;

namespace Common.Protocol.NTOs
{
    public class GameSearchQueryNetworkTransferObject : INetworkTransferObject<GameSearchQuery>
    {
        private GameSearchQuery _gameQuery;
        public GameSearchQuery Decode(string toDecode)
        {
            GameSearchQuery query = new GameSearchQuery();

            int index = 0;
            string title = ExtractGameField(toDecode, ref index);
            string genre = ExtractGameField(toDecode, ref index);
            int score = int.Parse(toDecode.Substring(index, VaporProtocolSpecification.GAME_SCOREINPUT_FIXED_SIZE));
            
            query.Title = title;
            query.Genre = genre;
            query.Score = score;

            return query;
        }

        public string Encode()
        {
            // 
            string title = _gameQuery.Title;
            string genre = _gameQuery.Genre;
            int score = _gameQuery.Score;

            string input = "";

            input += VaporProtocolHelper.FillNumber(title.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + title;

            input += VaporProtocolHelper.FillNumber(genre.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + genre;

            input += VaporProtocolHelper.FillNumber(score,VaporProtocolSpecification.GAME_SCOREINPUT_FIXED_SIZE);

            return input;
        }

        public void Load(GameSearchQuery obj)
        {
            _gameQuery = obj;
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
