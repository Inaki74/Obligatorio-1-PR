namespace Exceptions.BusinessExceptions
{
    public class UserDoesntExistException : BusinessException
    {
        public override string Message => $"User doesnt exist!";
    }
}