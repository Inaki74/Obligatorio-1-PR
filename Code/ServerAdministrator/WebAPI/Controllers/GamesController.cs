using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Grpc.Net.Client;
using Configuration;
using APIModel;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/api/games")]
    public class GamesController : ControllerBase
    {
        private readonly string _serverAddress;
        private readonly ILogger<GamesController> _logger;

        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            _serverAddress = new ConfigurationHandler().GetField(ConfigurationConstants.MAINSERVER_ADDRESS_KEY);
        }

        [HttpPost]
        public async Task<IActionResult> PostGame(GameModel game)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new GameMessager.GameMessagerClient(channel);
            var request = new AddGameRequest 
                { 
                    Gamename = game.Gamename,
                    Genre = game.Genre,
                    Esrb = game.ESRB,
                    Synopsis = game.Synopsis,
                    PathAFoto = game.PathToImage
                };
            var reply = await client.AddGameAsync(request);

            return Ok(reply);
        }
        
        [HttpPut("{gameid}")]
        public async Task<IActionResult> PutGame(GameModel game, int gameid)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new GameMessager.GameMessagerClient(channel);
            var request = new ModifyGameRequest 
            {
                Gamename = game.Gamename,
                Genre = game.Genre,
                Esrb = game.ESRB,
                Synopsis = game.Synopsis,
                PathAFoto = game.PathToImage,
                GameId = gameid
            };
            var reply = await client.ModifyGameAsync(request);

            return Ok(reply);
        }
        
        [HttpDelete("{gameid}")]
        public async Task<IActionResult> DeleteGame( int gameid)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new GameMessager.GameMessagerClient(channel);
            var request = new DeleteGameRequest 
            {
                GameId = gameid,
            };
            var reply = await client.DeleteGameAsync(request);

            return Ok(reply);
        }

        [HttpPost("user-acquire/{gameid}")]
        public async Task<IActionResult> UserAcquireGame(UserGameLinkModel obj, int gameid)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new GameMessager.GameMessagerClient(channel);
            var request = new LinkUserGameRequest 
                { 
                    Username = obj.Username,
                    Gameid = gameid
                };
            var reply = await client.LinkUserGameAsync(request);

            return Ok(reply);
        }

        [HttpDelete("user-acquire/{gameid}")]
        public async Task<IActionResult> UserUnacquireGame(UserGameLinkModel obj, int gameid)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new GameMessager.GameMessagerClient(channel);
            var request = new UnlinkUserGameRequest 
                { 
                    Username = obj.Username,
                    Gameid = gameid
                };
            var reply = await client.UnlinkUserGameAsync(request);

            return Ok(reply);
        }
    }
}
