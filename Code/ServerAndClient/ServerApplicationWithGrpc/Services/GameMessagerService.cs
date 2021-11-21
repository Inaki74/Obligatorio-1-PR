using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace ServerApplicationWithGrpc
{
    public class GameMessagerService : GameMessager.GameMessagerBase
    {
        private readonly ILogger<GameMessagerService> _logger;

        public GameMessagerService(ILogger<GameMessagerService> logger)
        {
            _logger = logger;
        }

        public override Task<GameReply> AddGame(AddGameRequest request, ServerCallContext context)
        {
            IGameLogic gameLogic = new GameLogic();

            int id = gameLogic.AddGame(game);

            string logMessage = $"The game {game.Title} has been published by {game.Owner.Username}.";
            SendLog(game.Owner.Username, id, logMessage);

             statusCode = StatusCodeConstants.OK;

            return Task.FromResult(new GameReply
            {
                StatusCode = 20,
                Message = request.Esrb + request.Gamename + request.Genre + request.Synopsis
            });
        }
    }
}
