using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class PublishReviewCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_PUBLISH_REVIEW_CODE;

        public string ActionReq(byte[] payload)
        {
            // Armado de juego
            Review review = DisassembleReviewPayload(payload);

            //IReviewLogic reviewLogic = new ReviewLogic();
            
            // Response
            int statusCode = 0;
            string response = "";
            
            try
            {
                //reviewLogic.AddReview(review);
                statusCode = StatusCodeConstants.OK;
                response = "Review published!";
            }
            catch (Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_CLIENT;
                response = $"Something went wrong when publishing your game: {e.Message}";
            }
        
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);

            return statusMessage;
        }

        private Review DisassembleReviewPayload(byte[] payload)
        {
            string payloadAsString = Encoding.UTF8.GetString(payload);

            ReviewNetworkTransferObject review = new ReviewNetworkTransferObject();

            return review.Decode(payloadAsString);
        }
    }
}