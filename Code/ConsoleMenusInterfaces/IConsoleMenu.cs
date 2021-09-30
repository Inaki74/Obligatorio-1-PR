
namespace ConsoleMenusInterfaces
{
    public interface IConsoleMenu
    {
        IConsoleMenu NextMenu { get; }
        bool RequiresAnswer { get; }
        void PrintMenu();

        bool Action(string answer);
    }
}