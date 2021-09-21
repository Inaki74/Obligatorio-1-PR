using System;

namespace Common.Protocol
{
    public static class VaporProtocolHelper
    {
        public static long GetFileParts(long fileSize)
        {
            double division = fileSize / VaporProtocolSpecification.MAX_PACKET_SIZE;
            return (long)Math.Ceiling(division);
        }

        public static string FillNumber(int number, int maxLength)
        {
            string numberString = number.ToString();

            while(numberString.Length < maxLength)
            {
                numberString = "0" + numberString;
            }

            return numberString;
        }
    }
}