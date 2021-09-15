
using ConsoleMenus;
using ConsoleMenus.Interfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleLoginMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => throw new System.NotImplementedException();

        public void Action()
        {
            throw new System.NotImplementedException();
        }

        public void Action(string answer)
        {
            // Intentar login
        }

        public void PrintMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}