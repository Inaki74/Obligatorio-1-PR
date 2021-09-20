namespace Common.Protocol
{
    public static class VaporProtocolHelper
    {
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