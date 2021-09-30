using System;

namespace Exceptions.BusinessExceptions
{
    public class UserLoggedException : BusinessException
    {
        public UserLoggedException() {}

        public override string Message => "User already logged in!";
    }
}
