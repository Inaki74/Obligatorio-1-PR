using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class PublishReviewCommand : CommandBase, Interfaces.ICommand
    {
        public PublishReviewCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_PUBLISH_REVIEW_CODE;

        public string ActionReq(byte[] payload)
        {
            ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
            IReviewLogic reviewLogic = new ReviewLogic();

            int statusCode = 0;
            string response = "";
            
            string payloadAsString = Encoding.UTF8.GetString(payload);
            Review review = reviewNTO.Decode(payloadAsString);
            
            bool existed = reviewLogic.Exists(review);
            reviewLogic.AddReview(review);

            if(!existed)
            {
                statusCode = StatusCodeConstants.OK;
                response = "Review published!";
            }
            else
            {
                statusCode = StatusCodeConstants.INFO;
                response = "You already had a review for this game, it was replaced with the new one.";
            }
        
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] payload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(payload);

            return statusMessage;
        }
        
    }
}