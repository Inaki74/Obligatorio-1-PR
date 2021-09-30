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
    public class ViewDetailsCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_VIEW_DETAILS_CODE;
        public string ActionReq(byte[] payload)
        {
            int statusCode = 0;
            string response = "";

            IGameLogic gameLogic = new GameLogic();
            IReviewLogic reviewLogic = new ReviewLogic();
            GameNetworkTransferObject queryDummy = new GameNetworkTransferObject();
            GameDetailsNetworkTransferObject detailsDummy = new GameDetailsNetworkTransferObject();
            try
            {
                DetailsQuery query = new DetailsQuery();
                Game gameDummy = queryDummy.Decode(Encoding.UTF8.GetString(payload));
                
                query.Game = gameLogic.GetGame(gameDummy.Id);
                query.Reviews = reviewLogic.GetReviews(query.Game);
                query.Score = reviewLogic.GetGameScore(query.Game);
                
                detailsDummy.Load(query);
                response = detailsDummy.Encode();
                
                statusCode = StatusCodeConstants.OK;
                
                return statusCode.ToString() + response;
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = $"Something went wrong server-side: {e.Message}";
                return statusCode.ToString() + response;
            }
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                GameDetailsNetworkTransferObject detailsNTO = new GameDetailsNetworkTransferObject();
                DetailsQuery query = detailsNTO.Decode(statusMessage.Message);
                
                statusMessage.Game = query.Game;
                statusMessage.ReviewsList = query.Reviews;
                statusMessage.GameScore = query.Score;
            }
            
            return statusMessage;
        }
    }
}