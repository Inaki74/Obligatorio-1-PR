using System;

namespace Exceptions.BusinessExceptions
{
    public class FindException : BusinessException
    {
        public FindException() {}

        public override string Message => "Tried to get an element that doesnt exist.";
    }
}
