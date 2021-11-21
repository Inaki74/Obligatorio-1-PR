using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogicInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq;

namespace Server.Controllers
{
    [ApiController]
    [Route("v1/api/logs")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly ILogLogic _logLogic;

        public LogController(ILogger<LogController> logger, ILogLogic logLogic)
        {
            _logger = logger;
            _logLogic = logLogic;
        }

        [HttpGet]
        public IActionResult Get(string username="", string gamename="", DateTime? date=null)
        {
            List<LogModel> logs = _logLogic.Get(username, gamename, date);
            logs = logs.OrderBy(l => l.Id).ToList();
            return Ok(logs);
        }
    }
}
