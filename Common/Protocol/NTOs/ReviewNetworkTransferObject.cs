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
            input += VaporProtocolHelper.FillNumber(Username.Length,VaporProtocolSpecification.USER_NAME_MAXSIZE) + Username;

            input += VaporProtocolHelper.FillNumber(gameIdAsString.Length,VaporProtocolSpecification.GAME_ID_MAXSIZE) + gameIdAsString;

            input += VaporProtocolHelper.FillNumber(Gametitle.Length,VaporProtocolSpecification.GAME_TITLE_MAXSIZE) + Gametitle;

            input += VaporProtocolHelper.FillNumber(Description.Length,VaporProtocolSpecification.REVIEW_DESCRIPTION_MAXSIZE) + Description;

            input += VaporProtocolHelper.FillNumber(Score.Length,VaporProtocolSpecification.GAME_SINGULAR_SCORE_MAXSIZE) + Score;

            return input;
        }

        public Review Decode(string toDecode)
        {
            Review review = new Review();

            int index = 0;
            string username = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.USER_NAME_MAXSIZE);
            int gameid = int.Parse(NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_ID_MAXSIZE));
            string gamename = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_TITLE_MAXSIZE);
            string description = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.REVIEW_DESCRIPTION_MAXSIZE);
            string score = NetworkTransferHelperMethods.ExtractGameField(toDecode, ref index, VaporProtocolSpecification.GAME_SINGULAR_SCORE_MAXSIZE);

            User dummyUser = new User(username, User.DEFAULT_USER_ID);
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