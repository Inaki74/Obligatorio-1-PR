using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Common;
using ConsoleMenusFactory;
using ConsoleMenusInterfaces;
using System.Configuration;
using Common.Configuration;

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

            Console.WriteLine("Main closing.");
        }

        
    }
}
