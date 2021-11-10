using System;
using System.Text;
using System.Threading.Tasks;
using Common.FileSystemUtilities;
using Common.FileSystemUtilities.Interfaces;
using Common.NetworkUtilities.Interfaces;
using Common.Protocol.Interfaces;
using Exceptions.ConnectionExceptions;

namespace Common.Protocol
{
    public class VaporProtocol
    {
        private readonly IStreamHandler _streamHandler;
        private readonly IPathHandler _pathHandler;
        private readonly IFileStreamHandler _fileStreamHandler;
        private readonly IFileInformationHandler _fileInformation;
        public VaporProtocol(IStreamHandler streamHandler)
        {
            _streamHandler = streamHandler;
            _pathHandler = new PathHandler();
            _fileStreamHandler = new FileStreamHandler();
            _fileInformation = new FileInformationHandler();
        }

        public async Task<VaporProcessedPacket> ReceiveCommandAsync()
        {
            byte[] req = await _streamHandler.ReadAsync(VaporProtocolSpecification.REQ_FIXED_SIZE);
            byte[] cmd = await _streamHandler.ReadAsync(VaporProtocolSpecification.CMD_FIXED_SIZE);
            byte[] length = await _streamHandler.ReadAsync(VaporProtocolSpecification.LENGTH_FIXED_SIZE);
            byte[] payload = await _streamHandler.ReadAsync(BitConverter.ToInt32(length));

            VaporProcessedPacket processedPacket = new VaporProcessedPacket(req, cmd, length, payload);

            return processedPacket;
        }

        public async Task SendCommandAsync(ReqResHeader request, string command, string data)
        {
            IVaporHeader header = new VaporCommandHeader(request, command, data);

            await _streamHandler.WriteAsync(header.Create());
        }

        public async Task SendCoverFailedAsync()
        {
            await _streamHandler.WriteAsync(Encoding.UTF8.GetBytes(VaporCoverHeader.FAILED_COVER));
        }

        public async Task SendCoverAsync(string gameTitle, string localPath)
        {
            long fileSize = _fileInformation.GetFileSize(localPath);

            IVaporHeader header = new VaporCoverHeader(gameTitle, fileSize);
            await _streamHandler.WriteAsync(header.Create());
            await SendImageAsync(fileSize, localPath);
        }

        public async Task ReceiveCoverAsync(string targetDirectoryPath)
        {
            byte[] isGoodCover = await _streamHandler.ReadAsync(VaporProtocolSpecification.COVER_CONFIRM_FIXED_SIZE);
            string isGoodCoverString = Encoding.UTF8.GetString(isGoodCover);
            if(isGoodCoverString == VaporCoverHeader.FAILED_COVER)
            {
                throw new CoverNotReceivedException();
            }
            
            byte[] fileNameLength = await _streamHandler.ReadAsync(VaporProtocolSpecification.COVER_FILENAMELENGTH_FIXED_SIZE);
            byte[] fileSize = await _streamHandler.ReadAsync(VaporProtocolSpecification.COVER_FILESIZE_FIXED_SIZE);
            byte[] fileName = await _streamHandler.ReadAsync(BitConverter.ToInt32(fileNameLength));

            long fileSizeDecoded = BitConverter.ToInt64(fileSize);
            string fileNameDecoded = Encoding.UTF8.GetString(fileName);
            string path = _pathHandler.AppendPath(targetDirectoryPath, fileNameDecoded + ".png");
            
            await ReceiveImageAsync(fileSizeDecoded, path);
        }

        private async Task ReceiveImageAsync(long fileSize, string path)
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
                    data = await _streamHandler.ReadAsync(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = await _streamHandler.ReadAsync(VaporProtocolSpecification.MAX_PACKET_SIZE);
                    offset += VaporProtocolSpecification.MAX_PACKET_SIZE;
                }

                bool isFirstPart = currentPart == 1;
                await _fileStreamHandler.WriteAsync(data, path, isFirstPart);
                currentPart++;
            }
        }

        private async Task SendImageAsync(long fileSize, string path)
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
                    data = await _fileStreamHandler.ReadAsync(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = await _fileStreamHandler.ReadAsync(path, offset, VaporProtocolSpecification.MAX_PACKET_SIZE);
                    offset += VaporProtocolSpecification.MAX_PACKET_SIZE;
                }

                await _streamHandler.WriteAsync(data);
                currentPart++;
            }
        }
    }
}