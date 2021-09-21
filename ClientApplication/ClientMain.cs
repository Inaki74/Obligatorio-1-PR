using ConsoleMenusInterfaces;
using ConsoleMenusFactory;
using System;

namespace ClientApplication
{
    class ClientMain
    {
        static void Main(string[] args)
        {
            new ClientHandler();
            ConsoleMenuManagerFactory consoleFactory = new ConsoleMenuManagerFactory();
            IConsoleMenuManager consoleManager = consoleFactory.Create(true);

            while(!consoleManager.Exit)
            {
                consoleManager.ExecuteMenu();
            }
        }
        
    }
}
