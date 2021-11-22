using System;
using System.Text;
using Common.Protocol;
using LogCommunicatorInterfaces;
using LogCommunicator;
using Models;
using Configuration;

namespace Common.Commands
{
    public abstract class CommandBase
    {
        protected readonly ILogGenerator _logsGenerator;
        protected readonly ILogSender _logsSender;
        public CommandBase(ILogSender logSender)
        {
            _logsGenerator = new LogGenerator();
            _logsSender = logSender;
        }
        protected virtual VaporStatusResponse ParseStatusResponse(byte[] payload)
        {
            string payloadString = Encoding.UTF8.GetString(payload);
            int statusCodeFixedSize = VaporProtocolSpecification.STATUS_CODE_FIXED_SIZE;
            int statusCode = int.Parse(payloadString.Substring(0, statusCodeFixedSize));
            string message = payloadString.Substring(statusCodeFixedSize, payloadString.Length-statusCodeFixedSize);
            string response = message;

            return new VaporStatusResponse(statusCode, response);
        }

        protected void SendLog(string username, int gameid, string message)
        {
            _logsSender.SendLog(_logsGenerator.CreateLog(username, gameid, false, message));
        }
    }
}
