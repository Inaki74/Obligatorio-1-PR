using ConsoleMenusFactory;
using ConsoleMenusInterfaces;

namespace ServerApplication
{
    class ServerMain
    {
        private static void Main(string[] args)
        {
            new ServerHandler();
            ConsoleMenuManagerFactory consoleFactory = new ConsoleMenuManagerFactory();
            IConsoleMenuManager consoleManager = consoleFactory.Create(false);
            
            while(!consoleManager.Exit)
            {
                consoleManager.ExecuteMenu();
            }
        }
    }
}
