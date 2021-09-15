using ConsoleMenus.Interfaces;
using ConsoleMenus.Factory;

namespace ConsoleMenus
{
    public abstract class ConsoleMenusBase
    {
        protected ConsoleMenusFactory _factory;
        protected IConsoleMenu _nextMenu;

        public ConsoleMenusBase()
        {
            _factory = new ConsoleMenusFactory();
        }
    }
}