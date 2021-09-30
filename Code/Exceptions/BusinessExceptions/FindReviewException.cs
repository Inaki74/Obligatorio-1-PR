using System;

namespace Exceptions.BusinessExceptions
{
    public class FindReviewException : BusinessException
    {
        public FindReviewException() {}
        public FindReviewException(string message) 
        {
            _innerMessage = message;
        }

        private string _innerMessage = "";

        public override string Message => $"That review doesn't exist! Inner exception: {_innerMessage}";
    }
}
