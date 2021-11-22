namespace Exceptions.BusinessExceptions
{
    public class UserInvalidRequestException : BusinessException
    {
        public override string Message => $"The format or content of the request were invalid!";
    }
}