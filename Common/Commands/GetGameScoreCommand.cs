using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.HelperObjects;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class GetGameScoreCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_GET_GAME_SCORE_CODE;

        // Get reviews list
        public string ActionReq(byte[] payload)
        {
            int statusCode = 0;
            string response = "";

            IReviewLogic reviewLogic = new ReviewLogic();
            try
            {
                //Decode query
                GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
                Game game = gameNTO.Decode(Encoding.UTF8.GetString(payload));

                List<Review> gameReviewList = reviewLogic.GetReviews(game);
                statusCode = StatusCodeConstants.OK;
                response = EncodeReviewList(gameReviewList);

                return statusCode.ToString() + response;
            }
            catch(Exception e) //TODO: Ver posibles errores del parte del cliente.
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Something went wrong server-side: {e.Message} + {e.StackTrace}";
                return statusCode.ToString() + response;
            }
        }

        // Decode games list
        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                statusMessage.ReviewsList = DecodeReviewList(statusMessage.Message);

                if(statusMessage.ReviewsList.Count != 0)
                {
                    IReviewLogic reviewLogic = new ReviewLogic();
                    statusMessage.GameScore = reviewLogic.GetGameScore(statusMessage.ReviewsList);
                }
                else
                {
                    statusMessage.GameScore = 0f;
                }
            }
            
            return statusMessage;
        }

        

        private string EncodeReviewList(List<Review> reviews)
        {
            // CANT-REVIEWS REVIEW(1) REVIEW(2) ... REVIEW(CANT-REVIEWS)
            string encoded = "";
            int cantReviews = reviews.Count;
            encoded += VaporProtocolHelper.FillNumber(cantReviews, VaporProtocolSpecification.REVIEWS_MAX_AMOUNT_FIXED_SIZE);

            foreach(Review review in reviews)
            {
                ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
                reviewNTO.Load(review);
                encoded += reviewNTO.Encode();
            }

            return encoded;
        }

        private List<Review> DecodeReviewList(string data)
        {
            List<Review> ret = new List<Review>();
            int cantReviews = int.Parse(data.Substring(0, VaporProtocolSpecification.REVIEWS_MAX_AMOUNT_FIXED_SIZE));
            string restOfData = data.Substring(VaporProtocolSpecification.REVIEWS_MAX_AMOUNT_FIXED_SIZE, data.Length - VaporProtocolSpecification.REVIEWS_MAX_AMOUNT_FIXED_SIZE);

            int index = 0;
            for(int i = 0; i < cantReviews; i++)
            {
                ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
                string reviewData = restOfData.Substring(index);

                Review review = reviewNTO.Decode(reviewData);
                ret.Add(review);

                reviewNTO.Load(review);
                index += reviewNTO.Encode().Length;
            }
            return ret;
        }
    }
}
