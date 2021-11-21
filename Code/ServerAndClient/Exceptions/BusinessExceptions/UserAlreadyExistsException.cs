namespace Exceptions.BusinessExceptions
{
    public class UserAlreadyExistsException : BusinessException
    {
        public override string Message => $"User already exists!";
    }
}