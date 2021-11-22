using System;

namespace Exceptions
{
    public class FileWritingException : Exception
    {
        public FileWritingException(string message)
        {
            _innerMessage = message;
        }

        private string _innerMessage = "";

        public override string Message => $"Something went wrong when trying to write your file. Inner exception: {_innerMessage}";
    }
}
