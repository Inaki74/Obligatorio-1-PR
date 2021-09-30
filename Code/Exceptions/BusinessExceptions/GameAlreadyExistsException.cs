using System;

namespace Exceptions.BusinessExceptions
{
    public class GameAlreadyExistsException : BusinessException
    {
        public GameAlreadyExistsException() {}

        public override string Message => "A game with that title already exists.";
    }
}
