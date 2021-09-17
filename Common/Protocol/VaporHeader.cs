using System;
using System.Text;

namespace Common.Protocol
{
    public class VaporHeader
    {
        public byte[] RequestType => _requestType;

        public byte[] Command => _command;

        public byte[] Length => _length;

        private byte[] _requestType;

        private byte[] _command;

        private byte[] _length;

        public VaporHeader()
        {
        }

        public VaporHeader(string requestType, int command, int length)
        {
            _requestType = Encoding.UTF8.GetBytes(requestType);
            _command = BitConverter.GetBytes(command);
            _length = length;
        }

    }
}
