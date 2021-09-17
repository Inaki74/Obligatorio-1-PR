using System;

namespace Common.Protocol
{
    public class VaporProcessedPacket
    {
        // req
        // cmd
        // length
        // payload

        public ReqResHeader ReqResHeader {get; set;}
        public int Command {get; set;}
        public int Length{get;set;}
        public byte[] Payload {get;set;}
    }
}
