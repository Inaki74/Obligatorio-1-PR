using System;

namespace Exceptions.BusinessExceptions
{
    public class FindUserException : BusinessException
    {
        public FindUserException() {}

        public override string Message => "That user doesn't exist!";
    }
}
