using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain.BusinessObjects;
using LogCommunicatorInterfaces;

namespace Common.Commands
{
    public class SelectGameCommand : CommandBase, Interfaces.ICommand
    {
        public SelectGameCommand(ILogSender logSender) : base(logSender)
        {
        }

        public string Command => CommandConstants.COMMAND_SELECT_GAME_CODE;
        public string ActionReq(byte[] payload)
        {
            Console.WriteLine("Init, select game");
            GameNetworkTransferObject gameDummy = new GameNetworkTransferObject();
            IGameLogic gameLogic = new GameLogic(); 
            
            int statusCode = 0;
            string response = "";

            string gameString = Encoding.UTF8.GetString(payload);
            Game game = gameDummy.Decode(gameString);
            game.Id = gameLogic.GetGameId(game.Title);
            Game gameSelected = gameLogic.SelectGame(game.Id);
            
            string idAsString = gameSelected.Id.ToString();
            int gameInputFixedSize = VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE;
            
            response = VaporProtocolHelper.FillNumber(idAsString.Length, gameInputFixedSize) + idAsString;
            statusCode = StatusCodeConstants.OK;
            
            return statusCode.ToString() + response;
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            VaporStatusResponse statusMessage = ParseStatusResponse(reqPayload);

            if(statusMessage.Code == StatusCodeConstants.OK)
            {
                int dummy = 0;
                int gameInputFixedSize = VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE;
                string GameField = NetworkTransferHelperMethods.ExtractGameField(statusMessage.Message, ref dummy, gameInputFixedSize);
                int id = int.Parse(GameField);

                statusMessage.SelectedGameId = id;
            }
            
            return statusMessage;
        }
    }
}