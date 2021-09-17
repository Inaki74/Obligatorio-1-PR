using System;
using Common.Interfaces;

namespace Common.Protocol
{
    // Es el de byte[] que mandamos por la red.
    public class VaporPacket
    {
        private VaporHeader _header;
        private byte[] _payload;

        public VaporPacket(VaporHeader header, byte[] payload)
        {
            this._header = header;
            this._payload = payload;
        }
        
        public byte[] Create()
        {
            byte[] packet = new byte[PacketLength()];

            byte[] requestType = _header.RequestType;
            byte[] command = _header.Command;
            byte[] payloadLength = BitConverter.GetBytes(_payload.Length);
            byte[] payload = _payload;

            int indexAcum = 0;

            Array.Copy(requestType, 0, packet, indexAcum, requestType.Length);
            indexAcum += requestType.Length;
            Array.Copy(command, 0, packet, indexAcum, command.Length);
            indexAcum += command.Length;
            Array.Copy(payloadLength, 0, packet, indexAcum, payloadLength.Length);
            indexAcum += payloadLength.Length;
            Array.Copy(payload, 0, packet, indexAcum, payload.Length);

            return packet;
        }

        private int PacketLength()
        {
            return _header.RequestType.Length + _header.Command.Length + BitConverter.GetBytes(_payload.Length).Length + _payload.Length;
        }
    }
}