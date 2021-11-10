using System;

namespace Common.Protocol.NTOs
{
    public static class NetworkTransferHelperMethods
    {
        public static string ExtractGameField(string payload, ref int index, int maxLength)
        {
            int length = int.Parse(payload.Substring(index, maxLength));
            index += maxLength;
            string field = payload.Substring(index, length);
            index += length;

            return field;
        }
    }
}
