﻿using System;
using System.Threading.Tasks;
using APIModel;
using Configuration;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/api/users")]
    public class UsersController : ControllerBase
    {
        private readonly string _serverAddress;
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            _serverAddress = new ConfigurationHandler().GetField(ConfigurationConstants.MAINSERVER_ADDRESS_KEY);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostUser(UserModel user)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new UserMessager.UserMessagerClient(channel);
            var request = new AddUserRequest 
            { 
                Username = user.Username,
            };
            var reply = await client.AddUserAsync(request);

            return Ok(reply);
        }
        
        [HttpPut("{userid}")]
        public async Task<IActionResult> ModifyUser(UserModel user, int userId)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new UserMessager.UserMessagerClient(channel);
            var request = new ModifyUserRequest() 
            { 
               UserId = userId, 
               Username = user.Username,
                
            };
            var reply = await client.ModifyUserAsync(request);

            return Ok(reply);
        }
    }
}