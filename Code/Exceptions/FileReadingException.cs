using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Exceptions
{
    public class FileReadingException : Exception
    {
        public FileReadingException(){}
        public FileReadingException(string message)
        {
            _innerMessage = message;
        }

        private string _innerMessage = "";

        public override string Message 
        {
            get
            {
                if(string.IsNullOrEmpty(_innerMessage))
                {
                    return "Couldn't read file, file stream broke.";
                }

                return $"Something went wrong when trying to read your file. Inner exception: {_innerMessage}.";
            }
        }
    }
}
