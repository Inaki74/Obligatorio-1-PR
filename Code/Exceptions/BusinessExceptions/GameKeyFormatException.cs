using System;

namespace Exceptions.BusinessExceptions
{
    public class GameKeyFormatException : BusinessException
    {
        public GameKeyFormatException()
        {

        }

        public override string Message => "Game title shouldn't be empty.";
    }
}
