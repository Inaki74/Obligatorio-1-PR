using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Grpc.Net.Client;
using Configuration;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/api/weather")]
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new GameMessager.GameMessagerClient(channel);
            var request = new AddGameRequest 
                { 
                    Gamename = "GreeterClient",
                    Genre = "Shooter",
                    Esrb = "M",
                    Synopsis = "Ayayay",
                    PathAFoto = "c/2/c/s"
                };
            var reply = await client.AddGameAsync(request);

            return Ok("Greeting: " + reply.Message);
        }
    }
}
