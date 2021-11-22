using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using BusinessInterfaces;
using Common.Protocol;
using Domain.BusinessObjects;
using Domain.HelperObjects;
using Exceptions.BusinessExceptions;
using Grpc.Core;
using LogCommunicator;
using LogCommunicatorInterfaces;
using Microsoft.Extensions.Logging;

namespace ServerApplicationWithGrpc
{
    public class UserMessagerService : UserMessager.UserMessagerBase
    {
        private readonly ILogger<UserMessagerService> _logger;
        private readonly IUserLogic _userLogic;
        private readonly ILogSender _logSender;
        private readonly ILogGenerator _logGenerator;

        public UserMessagerService(ILogger<UserMessagerService> logger, ILogSender logSender)
        {
            _logger = logger;
            _logSender = logSender;
            _userLogic = new UserLogic();
            _logGenerator = new LogGenerator();
        }

        public override Task<UserReply> AddUser(AddUserRequest request, ServerCallContext context)
        {
            
            string logMessage = "";
            int statusCode = StatusCodeConstants.OK;

            try
            {
                int id = _userLogic.AddUser(request.Username);
                logMessage = $"The user {request.Username} has been added.";
                _logSender.SendLog(_logGenerator.CreateLog("ADMIN", -1, false, logMessage));
            }
            catch(BusinessException e)
            {
                statusCode = StatusCodeConstants.ERROR_CLIENT;
                logMessage = e.Message;
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                logMessage = e.Message;
            }
            
            return Task.FromResult(new UserReply()
            {
                StatusCode = statusCode,
                Message = logMessage
            });
        }
        
        public override Task<UserReply> ModifyUser(ModifyUserRequest request, ServerCallContext context)
        {
            
            string logMessage = "";
            int statusCode = StatusCodeConstants.OK;

            try
            {
                _userLogic.ModifyUser(request.Username, request.UserId);
                logMessage = $"The user with id {request.UserId} has been modified to {request.Username}.";
                _logSender.SendLog(_logGenerator.CreateLog("ADMIN", -1, false, logMessage));
            }
            catch(BusinessException e)
            {
                statusCode = StatusCodeConstants.ERROR_CLIENT;
                logMessage = e.Message;
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                logMessage = e.Message;
            }
            
            return Task.FromResult(new UserReply()
            {
                StatusCode = statusCode,
                Message = logMessage
            });
        }
        
        public override Task<UserReply> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            string logMessage = "";
            int statusCode = StatusCodeConstants.OK;

            try
            {
                string username = _userLogic.GetUser(request.UserId).Username;
                _userLogic.DeleteUser(request.UserId);
                logMessage = $"The user {username} has been deleted by ADMIN.";
                _logSender.SendLog(_logGenerator.CreateLog("ADMIN", -1, false, logMessage));
            }
            catch(BusinessException e)
            {
                statusCode = StatusCodeConstants.ERROR_CLIENT;
                logMessage = e.Message;
            }
            catch(Exception e)
            {
                statusCode = StatusCodeConstants.ERROR_SERVER;
                logMessage = e.Message;
            }
            
            return Task.FromResult(new UserReply()
            {
                StatusCode = statusCode,
                Message = logMessage
            });
        }
    }
}