using System;

namespace Exceptions.BusinessExceptions
{
    public class FindUserException : BusinessException
    {
        public FindUserException() {}

        public FindUserException(string message) 
        {
            _innerMessage = message;
        }

        private string _innerMessage = "";

        public override string Message => $"That user doesn't exist!";
    }
}
