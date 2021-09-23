using System;
using System.Text;
using Business;
using BusinessInterfaces;
using Common.Interfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using Domain;

namespace Common.Commands
{
    public class AcquireGameCommand : CommandBase, ICommand
    {
        public string Command { get; }
        public string ActionReq(byte[] payload)
        {
            // GameNetworkTransferObject gameDummy = new GameNetworkTransferObject();
            // int statusCode = 0;
            // string response = "";
            // try
            // {
            //     Game game = gameDummy.Decode(Encoding.UTF8.GetString(payload));
            //     IGameLogic gameLogic = new GameLogic();
            //     bool gameSuccesfullyAcquired = gameLogic.AcquireGame(game.Title, );
            // }
            // catch (Exception e)
            // {
                
            // }
            return "";
        }

        public VaporStatusResponse ActionRes(byte[] reqPayload)
        {
            throw new System.NotImplementedException();
        }
    }
}