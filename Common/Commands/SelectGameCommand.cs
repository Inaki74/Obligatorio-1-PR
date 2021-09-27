﻿using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;

namespace Common.Commands
{
    public class SelectGameCommand : CommandBase, Interfaces.ICommand
    {
        public string Command => CommandConstants.COMMAND_SELECT_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            GameNetworkTransferObject gameDummy = new GameNetworkTransferObject();
            int statusCode = 0;
            string response = "";
            try
            {
                Game game = gameDummy.Decode(Encoding.UTF8.GetString(payload));
                IGameLogic gameLogic = new GameLogic(); 

                game.Id = gameLogic.GetGameId(game.Title);
                Game gameSelected = gameLogic.SelectGame(game.Id);
                if (gameSelected != null)
                {
                    statusCode = StatusCodeConstants.OK;
                    string idAsString = gameSelected.Id.ToString();
                    response = VaporProtocolHelper.FillNumber(idAsString.Length, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + idAsString;
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
                response = "Something went wrong! exception: " + e.Message + e.StackTrace;
                return statusCode.ToString() + response;
            }

        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                int dummy = 0;
                int id = int.Parse(NetworkTransferHelperMethods.ExtractGameField(statusMessage.Message, ref dummy, VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE));

                statusMessage.SelectedGameId = id;
            }
            
            return statusMessage;
        }
    }
}