using System;

namespace Exceptions.BusinessExceptions
{
    public class FindGameException : BusinessException
    {
        public FindGameException() {}

        public override string Message => "That game doesn't exist!";
    }
}
