using System;

namespace Exceptions.BusinessExceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() {}

        public override string Message => base.Message;
    }
}
