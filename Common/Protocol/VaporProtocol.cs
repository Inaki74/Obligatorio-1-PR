using System;
using System.Text;
using Common.NetworkUtilities;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol.Interfaces;

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
        public VaporProcessedPacket ReceiveCommand()
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

        public void SendCommand(ReqResHeader request, string command, string data)
        {
            IVaporHeader header = new VaporCommandHeader(request, command, data);

            _streamHandler.Write(header.Create());
        }

        public void SendCover(string gameTitle, string localPath)
        {
            // largoNombreFile NombreFile tamañoFile
            // Ej: 4 doom 32678
            // Envia header
            IVaporHeader header = new VaporCoverHeader(localPath, 2); //Path llega, fs.GetSize()

            // Envia imagen
        }

        public void ReceiveCover(string targetDirectoryPath)
        {
            // Recibe header
            // 
        }
    }
}