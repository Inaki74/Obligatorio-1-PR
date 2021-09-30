using ConsoleMenusInterfaces;
using ConsoleMenus;

namespace ConsoleMenusFactory
{
    public class ConsoleMenuManagerFactory
    {
        public ConsoleMenuManagerFactory()
        {

        }

        public IConsoleMenuManager Create(bool client)
        {
            return new ConsoleMenuManager(client);
        }
    }
}