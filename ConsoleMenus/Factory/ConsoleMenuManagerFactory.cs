using ConsoleMenus.Interfaces;
using ConsoleMenus;

namespace ConsoleMenus.Factory
{
    public class ConsoleMenuManagerFactory
    {
        public ConsoleMenuManagerFactory()
        {

        }

        public IConsoleMenuManager Create()
        {
            return new ConsoleMenuManager();
        }
    }
}