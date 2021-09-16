namespace ConsoleMenusInterfaces
{
    public interface IConsoleMenuManager
    {
        bool Exit { get; }
        void ExecuteMenu();
    }
}