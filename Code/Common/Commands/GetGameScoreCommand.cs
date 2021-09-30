using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.Interfaces;
using Common.Protocol.NTOs;
using Domain.HelperObjects;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class GetGameScoreCommand : CommandBase, ICommand
    {
        public string Command => CommandConstants.COMMAND_GET_GAME_SCORE_CODE;
        
        public string ActionReq(byte[] payload)
        {
            int statusCode = 0;
            string response = "";

            IReviewLogic reviewLogic = new ReviewLogic();
            GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
            ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
            ListNetworkTransferObject<Review> reviewListNTO = new ListNetworkTransferObject<Review>(reviewNTO);

            string gameString = Encoding.UTF8.GetString(payload);
            Game game = gameNTO.Decode(gameString);
            List<Review> gameReviewList = reviewLogic.GetReviews(game);
            reviewListNTO.Load(gameReviewList);
            response = reviewListNTO.Encode();

            statusCode = StatusCodeConstants.OK;

            return statusCode.ToString() + response;
        }
        
        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
            ListNetworkTransferObject<Review> reviewListNTO = new ListNetworkTransferObject<Review>(reviewNTO);
            
            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                statusMessage.ReviewsList = reviewListNTO.Decode(statusMessage.Message);

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
        
    }
}
