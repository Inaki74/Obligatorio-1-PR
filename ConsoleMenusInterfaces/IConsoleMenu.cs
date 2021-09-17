
namespace ConsoleMenusInterfaces
{
    public interface IConsoleMenu
    {
        IConsoleMenu NextMenu { get; }
        bool RequiresAnswer { get; }
        void PrintMenu();

        void Action(string answer);
    }
}