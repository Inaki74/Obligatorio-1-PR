using System;

namespace Exceptions.BusinessExceptions
{
    public class FindReviewException : BusinessException
    {
        public FindReviewException() {}

        public override string Message => "That review doesn't exist!";
    }
}
