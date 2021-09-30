using System;
using System.Collections.Generic;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace Common.Commands
{
    public class ViewReviewCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_VIEW_REVIEW_CODE;
        public string ActionReq(byte[] payload)
        {
            GameUserRelationQueryNetworkTransferObject queryDummy = new GameUserRelationQueryNetworkTransferObject();
            ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
            IReviewLogic reviewLogic = new ReviewLogic();
            
            int statusCode = 0;
            string response = "";

            string queryString = Encoding.UTF8.GetString(payload);
            GameUserRelationQuery query = queryDummy.Decode(queryString);
            Review review = reviewLogic.GetReview(query);
                
            reviewNTO.Load(review);
            response = reviewNTO.Encode();
                
            statusCode = StatusCodeConstants.OK;
                
            return statusCode.ToString() + response;

        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
                statusMessage.Review = reviewNTO.Decode(statusMessage.Message);
            }
            
            return statusMessage;
        }
        
    }
}