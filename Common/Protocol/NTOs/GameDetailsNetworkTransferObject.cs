using System.Collections.Generic;
using Common.Protocol.Interfaces;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace Common.Protocol.NTOs
{
    public class GameDetailsNetworkTransferObject : INetworkTransferObject<DetailsQuery>
    {
        public Game Game { get; set; }
        public List<Review> Reviews { get; set; }
        
        public float Score { get; set; }
        private GameNetworkTransferObject _gameNTO { get; set; } = new GameNetworkTransferObject();
        private ListNetworkTransferObject<Review> _reviewListNTO { get; set; } = new ListNetworkTransferObject<Review>(new ReviewNetworkTransferObject());
        public string Encode()
        {
           _gameNTO.Load(Game);
           _reviewListNTO.Load(Reviews);
            string input = _gameNTO.Encode();
            input += _reviewListNTO.Encode();
            input += input += VaporProtocolHelper.FillNumber(Score.ToString().Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Score;
            return input;
        }

        public DetailsQuery Decode(string toDecode)
        {
            
            DetailsQuery decodedQuery = new DetailsQuery();
            
            decodedQuery.Game = _gameNTO.Decode(toDecode);
            
            _gameNTO.Load(decodedQuery.Game);
            int gameLength = _gameNTO.Encode().Length;
            
            string restOfData = toDecode.Substring(gameLength, toDecode.Length - gameLength);
            
            decodedQuery.Reviews = _reviewListNTO.Decode(restOfData);
            
            _reviewListNTO.Load(decodedQuery.Reviews);
            int reviewLength = _reviewListNTO.Encode().Length;
            
            restOfData = restOfData.Substring(reviewLength, restOfData.Length - reviewLength);
            
            int index = 0;
            decodedQuery.Score = float.Parse(NetworkTransferHelperMethods.ExtractGameField(restOfData,ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE));
            
            return decodedQuery;
        }

        public void Load(DetailsQuery details)
        {
            Reviews = details.Reviews;
            Game = details.Game;
            Score = details.Score;
        }
    }
}