using Domain;

namespace BusinessInterfaces
{
    public interface IUserLogic
    {
        bool Login(User username);
        void Logout(string username);
    }
}