using System;
using System.Text;

namespace Common.Protocol
{
    public class VaporProcessedPacket
    {
        public ReqResHeader ReqResHeader {get; set;}
        public string Command {get; set;}
        public int Length{get;set;}
        public byte[] Payload {get;set;}

        public VaporProcessedPacket(byte[] req, byte[] cmd, byte[] length, byte[] payload)
        {
            string reqString = Encoding.UTF8.GetString(req);

            if(reqString == HeaderConstants.Request)
            {
                ReqResHeader = ReqResHeader.REQ;
            }
            else
            {
                ReqResHeader = ReqResHeader.RES;
            }

            Command = Encoding.UTF8.GetString(cmd);
            Length = BitConverter.ToInt32(length);
            Payload = payload;
        }
    }
}
