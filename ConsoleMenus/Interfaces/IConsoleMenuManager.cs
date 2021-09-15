namespace ConsoleMenus.Interfaces
{
    public interface IConsoleMenuManager
    {
        bool Exit { get; }
        void ExecuteMenu();
    }
}