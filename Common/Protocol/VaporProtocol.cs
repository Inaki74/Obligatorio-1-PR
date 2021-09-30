﻿using System;
using System.Text;
using Common.FileSystemUtilities;
using Common.FileSystemUtilities.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol.Interfaces;
using Exceptions.ConnectionExceptions;

namespace Common.Protocol
{
    public class VaporProtocol
    {
        private readonly INetworkStreamHandler _networkStreamHandler;
        private readonly IPathHandler _pathHandler;
        private readonly IFileStreamHandler _fileStreamHandler;
        private readonly IFileInformationHandler _fileInformation;
        public VaporProtocol(INetworkStreamHandler streamHandler)
        {
            _networkStreamHandler = streamHandler;
            _pathHandler = new PathHandler();
            _fileStreamHandler = new FileStreamHandler();
            _fileInformation = new FileInformationHandler();
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

            byte[] req = _networkStreamHandler.Read(VaporProtocolSpecification.REQ_FIXED_SIZE);
            byte[] cmd = _networkStreamHandler.Read(VaporProtocolSpecification.CMD_FIXED_SIZE);
            byte[] length = _networkStreamHandler.Read(VaporProtocolSpecification.LENGTH_FIXED_SIZE);
            byte[] payload = _networkStreamHandler.Read(BitConverter.ToInt32(length));

            VaporProcessedPacket processedPacket = new VaporProcessedPacket(req, cmd, length, payload);

            return processedPacket;
        }

        public void SendCommand(ReqResHeader request, string command, string data)
        {
            IVaporHeader header = new VaporCommandHeader(request, command, data);

            _networkStreamHandler.Write(header.Create());
        }

        public void SendCoverFailed()
        {
            _networkStreamHandler.Write(Encoding.UTF8.GetBytes(VaporCoverHeader.FAILED_COVER));
        }

        public void SendCover(string gameTitle, string localPath)
        {
            // largoNombreFile tamañoFile NombreFile
            // Ej: 4 doom 32678
            // Envia header
            long fileSize = _fileInformation.GetFileSize(localPath);

            IVaporHeader header = new VaporCoverHeader(gameTitle, fileSize);
            _networkStreamHandler.Write(header.Create());
            // Envia imagen
            SendImage(fileSize, localPath);
        }

        public void ReceiveCover(string targetDirectoryPath)
        {
            byte[] isGoodCover = _networkStreamHandler.Read(VaporProtocolSpecification.COVER_CONFIRM_FIXED_SIZE);
            string isGoodCoverString = Encoding.UTF8.GetString(isGoodCover);
            if(isGoodCoverString == VaporCoverHeader.FAILED_COVER)
            {
                throw new CoverNotReceivedException();
            }

            // Recibe header
            byte[] fileNameLength = _networkStreamHandler.Read(VaporProtocolSpecification.COVER_FILENAMELENGTH_FIXED_SIZE);
            byte[] fileSize = _networkStreamHandler.Read(VaporProtocolSpecification.COVER_FILESIZE_FIXED_SIZE);
            byte[] fileName = _networkStreamHandler.Read(BitConverter.ToInt32(fileNameLength));

            long fileSizeDecoded = BitConverter.ToInt64(fileSize);
            string fileNameDecoded = Encoding.UTF8.GetString(fileName);
            string path = _pathHandler.AppendPath(targetDirectoryPath, fileNameDecoded + ".png");

            // Recibir imagen
            ReceiveImage(fileSizeDecoded, path);
        }

        private void ReceiveImage(long fileSize, string path)
        {
            long parts = VaporProtocolHelper.GetFileParts(fileSize);

            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = _networkStreamHandler.Read(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = _networkStreamHandler.Read(VaporProtocolSpecification.MAX_PACKET_SIZE);
                    offset += VaporProtocolSpecification.MAX_PACKET_SIZE;
                }

                bool isFirstPart = currentPart == 1;
                _fileStreamHandler.Write(data, path, isFirstPart);
                currentPart++;
            }
        }

        private void SendImage(long fileSize, string path)
        {
            long parts = VaporProtocolHelper.GetFileParts(fileSize);

            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = _fileStreamHandler.Read(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = _fileStreamHandler.Read(path, offset, VaporProtocolSpecification.MAX_PACKET_SIZE);
                    offset += VaporProtocolSpecification.MAX_PACKET_SIZE;
                }

                _networkStreamHandler.Write(data);
                currentPart++;
            }
        }
    }
}