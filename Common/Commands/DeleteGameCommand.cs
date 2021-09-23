using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class DeleteGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_SELECT_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            UserNetworkTransferObject userDummy = new UserNetworkTransferObject();
            int statusCode = 0;
            string response = "";
            try
            {
                User game = userDummy.Decode(Encoding.UTF8.GetString(payload));
                IGameLogic gameLogic = new GameLogic(); 
                bool gameSelected = true;//gameLogic.SelectGame(game.Title);
                if (gameSelected)
                {
                    statusCode = StatusCodeConstants.OK;
                    response = "Game selected succesfully.";
                }
                else
                {
                    statusCode = StatusCodeConstants.ERROR_CLIENT;
                    response = "Couldn't select game. Game doesn't exist.";
                }
                return statusCode.ToString() + response;
                
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                response = "Something went wrong! exception: " + e.Message;
                return statusCode.ToString() + response;
            }

        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            return statusMessage;
        }
    }
}