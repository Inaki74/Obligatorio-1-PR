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

        public VaporHeader(ReqResHeader requestType, int command, int length)
        {
            string req = "";
            if(requestType == ReqResHeader.REQ)
            {
                req = HeaderConstants.Request;
            }
            else
            {
                req = HeaderConstants.Response;
            }

            _requestType = Encoding.UTF8.GetBytes(req);
            _command = BitConverter.GetBytes(command);
            _length = BitConverter.GetBytes(length);
        }

    }
}
