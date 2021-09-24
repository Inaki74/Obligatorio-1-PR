using System;
using Common.Protocol.Interfaces;
using Domain.BusinessObjects;

namespace Common.Protocol.NTOs
{
    public class ReviewNetworkTransferObject : INetworkTransferObject<Review>
    {
        public string Username { get; set; } = "";
        public string Gamename {get; set;} = "";
        public string Description {get; set;} = "";
        public string Score {get; set;} = "";

        public void Load(Review review)
        {
            Username = review.ReviewPublisher.Username;
            Gamename = review.Game.Title;
            Description = review.Description;
            Score = review.Score.ToString();
        }

        public string Encode()
        {
            // Order
            // User->Game->description->score
            string input = "";

            //TODO: Is VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE adequate for everything? If so, is it a good name?
            input += VaporProtocolHelper.FillNumber(Username.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Username;

            input += VaporProtocolHelper.FillNumber(Gamename.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Gamename;

            input += VaporProtocolHelper.FillNumber(Description.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Description;

            input += Score.Length.ToString() + Score;

            return input;
        }

        public Review Decode(string toDecode)
        {
            Review review = new Review();

            int index = 0;
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index);
            string gamename = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index);
            string description = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index);
            string score = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index);

            User dummyUser = new User(username, -1);
            Game dummyGame = new Game();
            dummyGame.Title = gamename;

            review.ReviewPublisher = dummyUser;
            review.Game = dummyGame;
            review.Description = description;
            review.Score = int.Parse(score);

            return review;
        }
    }
}