using System;

namespace Exceptions.BusinessExceptions
{
    public class FindGameException : BusinessException
    {
        public FindGameException() {}
        public FindGameException(string message) 
        {
            _innerMessage = message;
        }

        private string _innerMessage = "";

        public override string Message => $"That game doesn't exist! Inner exception: {_innerMessage}";
    }
}
