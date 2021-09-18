﻿
namespace Common.Protocol
{
    public class VaporProtocolSpecification
    {
        public const int REQ_FIXED_SIZE = 3;
        public const int CMD_FIXED_SIZE = 2;
        public const int LENGTH_FIXED_SIZE = 4;

        public const int STATUS_CODE_FIXED_SIZE = 2;
        public const int MAX_PACKET_SIZE = 16382;
    }
}