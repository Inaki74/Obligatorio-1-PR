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
            
            try
            {
                GameNetworkTransferObject gameNTO = new GameNetworkTransferObject();
                ListNetworkTransferObject<Review> reviewListNTO = new ListNetworkTransferObject<Review>(new ReviewNetworkTransferObject());
                
                Game game = gameNTO.Decode(Encoding.UTF8.GetString(payload));
                List<Review> gameReviewList = reviewLogic.GetReviews(game);
                statusCode = StatusCodeConstants.OK;
                
                reviewListNTO.Load(gameReviewList);
                response = reviewListNTO.Encode();

                return statusCode.ToString() + response;
            }
            catch(Exception e) //TODO: Ver posibles errores del parte del cliente.
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Something went wrong server-side: {e.Message} + {e.StackTrace}";
                return statusCode.ToString() + response;
            }
        }
        
        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                ListNetworkTransferObject<Review> reviewListNTO = new ListNetworkTransferObject<Review>(new ReviewNetworkTransferObject());
                
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
