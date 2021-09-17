using System;
using System.Text;

namespace Common.Protocol
{
    public class VaporHeader
    {
        public byte[] RequestType => _requestType;

        public byte[] Command => _command;

        private byte[] _requestType;

        private byte[] _command;

        public VaporHeader()
        {
        }

        public VaporHeader(string requestType, int command)
        {
            _requestType = Encoding.UTF8.GetBytes(requestType);
            _command = BitConverter.GetBytes(command);
        }

    }
}
