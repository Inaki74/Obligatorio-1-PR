using System;
using System.Threading.Tasks;
using APIModel;
using Configuration;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI
{
    public class ResultHelper
    {
        private const int INFO_CODE = 10;
        private const int OK_CODE = 20;
        private const int ERROR_CODE = 40;
        private const int ERROR_UNAUTH_CODE = 41;
        private const int ERROR_CONNECTION_CODE = 42;
        private const int ERROR_SERVER_CODE = 50;

        public static IActionResult ClassifyResult(object ret, int statuscode, ControllerBase controller)
        {
            IActionResult result = controller.Ok(ret);

            switch(statuscode)
            {
                case ERROR_CODE:
                    result = controller.BadRequest(ret);
                    break;
                case ERROR_CONNECTION_CODE:
                    result = controller.BadRequest(ret);
                    break;
                case ERROR_UNAUTH_CODE:
                    result = controller.Unauthorized();
                    break;
                case ERROR_SERVER_CODE:
                    result = controller.StatusCode(500, ret);
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
