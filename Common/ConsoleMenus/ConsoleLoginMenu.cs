
using Common.ConsoleMenus.Interfaces;

namespace Common.ConsoleMenus
{
    public class ConsoleLoginMenu : IConsoleMenu
    {
        public bool RequiresAnswer => true;

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