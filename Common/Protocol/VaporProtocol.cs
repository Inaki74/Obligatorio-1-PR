using System;
using System.Text;
using Common.NetworkUtilities;
using Common.NetworkUtilities.Interfaces;

namespace Common.Protocol
{
    public class VaporProtocol
    {
        private INetworkStreamHandler _streamHandler;
        public VaporProtocol(INetworkStreamHandler streamHandler)
        {
            _streamHandler = streamHandler;
        }

        // Devolver lo que recibio procesado.
        public VaporProcessedPacket Receive()
        {
            // Cuando server recibe mensaje:
                // Server sabe que:
                //  primero viene REQ/RES
                //  luego viene CMD
                //  luego viene LARGO
                //  finalmente PAYLOAD

            byte[] req = _streamHandler.Read(VaporProtocolSpecification.REQ_FIXED_SIZE);
            byte[] cmd = _streamHandler.Read(VaporProtocolSpecification.CMD_FIXED_SIZE);
            byte[] length = _streamHandler.Read(VaporProtocolSpecification.LENGTH_FIXED_SIZE);
            byte[] payload = _streamHandler.Read(BitConverter.ToInt32(length));

            VaporProcessedPacket processedPacket = new VaporProcessedPacket(req, cmd, length, payload);

            return processedPacket;
        }

        public void Send(ReqResHeader request, int command, int length, string data)
        {
            VaporHeader header = CreateHeader(request, command, length);
            VaporPacket packet = new VaporPacket(header, Encoding.UTF8.GetBytes(data));

            _streamHandler.Write(packet.Create());
        }

         private VaporHeader CreateHeader(ReqResHeader requestType, int command, int length)
        {
            VaporHeader header = new VaporHeader(requestType, command, length);

            return header;
        }
    }
}