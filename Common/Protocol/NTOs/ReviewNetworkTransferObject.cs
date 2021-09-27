using System;
using Common.Protocol.Interfaces;
using Domain.BusinessObjects;

namespace Common.Protocol.NTOs
{
    public class ReviewNetworkTransferObject : INetworkTransferObject<Review>
    {
        public string Username { get; set; } = "";
        public int Gameid {get; set;} = -1;
        public string Gametitle { get; set; } = "";
        public string Description {get; set;} = "";
        public string Score {get; set;} = "";

        public void Load(Review review)
        {
            Username = review.ReviewPublisher != null ? review.ReviewPublisher.Username : String.Empty;
            Gameid = review.Game != null ? review.Game.Id : -1;
            Gametitle = review.Game != null ? review.Game.Title : String.Empty;
            Description = review.Description;
            Score = review.Score.ToString();
        }

        public string Encode()
        {
            // Order
            // User->Game->description->score
            string input = "";
            string gameIdAsString = Gameid.ToString();

            //TODO: Is VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE adequate for everything? If so, is it a good name?
            input += VaporProtocolHelper.FillNumber(Username.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Username;

            input += VaporProtocolHelper.FillNumber(gameIdAsString.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + gameIdAsString;

            input += VaporProtocolHelper.FillNumber(Gametitle.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Gametitle;

            input += VaporProtocolHelper.FillNumber(Description.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Description;

            input += Score.Length.ToString() + Score;

            return input;
        }

        public Review Decode(string toDecode)
        {
            Review review = new Review();

            int index = 0;
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            int gameid = int.Parse(NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE));
            string gamename = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string description = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE);
            string score = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, 1);

            User dummyUser = new User(username, -1);
            Game dummyGame = new Game();
            
            dummyGame.Id = gameid;
            dummyGame.Title = gamename;
            review.ReviewPublisher = dummyUser;
            review.Game = dummyGame;
            review.Description = description;
            review.Score = int.Parse(score);

            return review;
        }
    }
}