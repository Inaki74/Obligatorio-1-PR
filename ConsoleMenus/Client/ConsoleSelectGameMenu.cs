using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleSelectGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu { get; }
        public bool RequiresAnswer { get; }
        public void PrintMenu()
        {
            throw new System.NotImplementedException();
        }

        public bool Action(string answer)
        {
            throw new System.NotImplementedException();
        }
    }
}