using ConsoleMenus.Interfaces;
using ConsoleMenus;

namespace ConsoleMenus.Factory
{
    public class ConsoleMenusFactory
    {
        public ConsoleMenusFactory()
        {

        }

        public IConsoleMenu Create<T>() where T : IConsoleMenu, new()
        {
            IConsoleMenu menu = new T();

            return menu;
        }
    }
}