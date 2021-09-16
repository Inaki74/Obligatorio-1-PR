using ConsoleMenusInterfaces;


namespace ConsoleMenusFactory
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