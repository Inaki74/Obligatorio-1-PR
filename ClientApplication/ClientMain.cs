using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ConsoleMenusInterfaces;
using ConsoleMenusFactory;

namespace ClientApplication
{
    class ClientMain
    {
        static void Main(string[] args)
        {
            new ClientHandler();
            ConsoleMenuManagerFactory consoleFactory = new ConsoleMenuManagerFactory();
            IConsoleMenuManager consoleManager = consoleFactory.Create();

            while(!consoleManager.Exit)
            {
                consoleManager.ExecuteMenu();
            }
        }
        
    }
}
