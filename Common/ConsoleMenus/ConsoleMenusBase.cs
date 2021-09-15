using Common.ConsoleMenus.Interfaces;
using Common.ConsoleMenus.Factory;

namespace Common.ConsoleMenus
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