using System;
using System.Text;
using Common.Protocol;
using LogCommunicator.Interfaces;
using LogCommunicator;
using Models;

namespace Common.Commands
{
    public abstract class CommandBase
    {
        protected readonly ILogGenerator _logsGenerator;
        protected readonly ILogSender _logsSender;
        public CommandBase()
        {
            _logsGenerator = new LogGenerator();
            _logsSender = new LogSender();
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
            // ESTO DEBERIA DE SER ASYNC?!?!?!
            _logsSender.SendLog(_logsGenerator.CreateLog(username, gameid, false, message));
        }
    }
}
