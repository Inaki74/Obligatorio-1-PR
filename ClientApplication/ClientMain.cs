using ConsoleMenusInterfaces;
using ConsoleMenusFactory;
using Common.Configuration.Interfaces;
using Common.Configuration;
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

            Console.WriteLine((new ConfigurationHandler()).GetField("prueba"));

            return;

            while(!consoleManager.Exit)
            {
                consoleManager.ExecuteMenu();
            }
        }
        
    }
}
