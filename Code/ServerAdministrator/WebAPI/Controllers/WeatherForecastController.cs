using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Grpc.Net.Client;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5002");
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
