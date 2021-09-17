using System;

namespace Common.Commands
{
    public class CommandHelper
    {
        public static string RESPONSE_SEPARATOR = "#";
        public static string MakeResponse(int statusCode, string extraData)
        {
            return statusCode.ToString() + RESPONSE_SEPARATOR + extraData;
        }
    }
}
