using System;

namespace Common.Protocol
{
    public class VaporHeader
    {
        private byte[] _requestType;

        private byte[] _command;

        public VaporHeader()
        {
        }

        public VaporHeader(string requestType, int command, int payloadLength)
        {

        }

    }
}
