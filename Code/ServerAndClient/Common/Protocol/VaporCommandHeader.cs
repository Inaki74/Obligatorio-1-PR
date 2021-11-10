using System;
using System.Text;
using Common.Protocol.Interfaces;

namespace Common.Protocol
{
    public class VaporCommandHeader : IVaporHeader
    {
        public byte[] RequestType => _requestType;

        public byte[] Command => _command;

        public byte[] Length => _length;

        public byte[] Payload => _payload;

        private byte[] _requestType;

        private byte[] _command;

        private byte[] _length;

        private byte[] _payload;

        public VaporCommandHeader()
        {
        }

        public VaporCommandHeader(ReqResHeader requestType, string command, string payload)
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
            _command = Encoding.UTF8.GetBytes(command);
            _length = BitConverter.GetBytes(payload.Length);
            _payload = Encoding.UTF8.GetBytes(payload);
        }

        public byte[] Create()
        {
            byte[] packet = new byte[GetLength()];

            int indexAcum = 0;

            Array.Copy(_requestType, 0, packet, indexAcum, VaporProtocolSpecification.REQ_FIXED_SIZE);
            indexAcum += VaporProtocolSpecification.REQ_FIXED_SIZE;
            Array.Copy(_command, 0, packet, indexAcum, VaporProtocolSpecification.CMD_FIXED_SIZE);
            indexAcum += VaporProtocolSpecification.CMD_FIXED_SIZE;
            Array.Copy(_length, 0, packet, indexAcum, VaporProtocolSpecification.LENGTH_FIXED_SIZE);
            indexAcum += VaporProtocolSpecification.LENGTH_FIXED_SIZE;
            Array.Copy(_payload, 0, packet, indexAcum, _payload.Length);

            return packet;
        }

        public int GetLength()
        {
            return RequestType.Length + Command.Length + BitConverter.GetBytes(_payload.Length).Length + _payload.Length;
        }
    }
}
