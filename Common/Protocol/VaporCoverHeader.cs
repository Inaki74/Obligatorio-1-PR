using System;
using System.Text;
using Common.Protocol.Interfaces;

namespace Common.Protocol
{
    public class VaporCoverHeader : IVaporHeader
    {
        public byte[] FileNameLength => _fileNameLength;
        public byte[] FileName => _fileName;
        public byte[] FileSize => _fileSize;

        private byte[] _fileNameLength;
        private byte[] _fileName;
        private byte[] _fileSize;

        public VaporCoverHeader(string fileName, long fileSize)
        {
            _fileNameLength = BitConverter.GetBytes(fileName.Length);
            _fileName =  Encoding.UTF8.GetBytes(fileName);
            _fileSize = BitConverter.GetBytes(fileSize);
        }

        public byte[] Create()
        {
            byte[] packet = new byte[GetLength()];

            int indexAcum = 0;

            Array.Copy(_fileNameLength, 0, packet, indexAcum, VaporProtocolSpecification.COVER_FILENAMELENGTH_FIXED_SIZE);
            indexAcum += VaporProtocolSpecification.COVER_FILENAMELENGTH_FIXED_SIZE;
            Array.Copy(_fileSize, 0, packet, indexAcum, VaporProtocolSpecification.COVER_FILESIZE_FIXED_SIZE);
            indexAcum += VaporProtocolSpecification.COVER_FILESIZE_FIXED_SIZE;
            Array.Copy(_fileName, 0, packet, indexAcum, _fileName.Length);

            return packet;
        }

        public int GetLength()
        {
            throw new NotImplementedException();
        }
    }
}
