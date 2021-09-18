namespace BusinessInterfaces
{
    public interface IUserLogic
    {
        bool Login(string username);
        void Logout(string username);
    }
}