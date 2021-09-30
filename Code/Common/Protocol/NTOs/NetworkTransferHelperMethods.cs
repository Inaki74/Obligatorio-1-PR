using System;

namespace Common.Protocol.NTOs
{
    public static class NetworkTransferHelperMethods
    {
        public static string ExtractGameField(string payload, ref int index, int maxLength)
        {
            //Extracts field of shape:
            //  XXXX XXXXX... -> FIELDLENGTH FIELD
            int length = int.Parse(payload.Substring(index, maxLength));
            index += maxLength;
            string field = payload.Substring(index, length);
            index += length;

            return field;
        }
    }
}
