using Common.ConsoleMenus.Interfaces;
using Common.ConsoleMenus;

namespace Common.ConsoleMenus.Factory
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