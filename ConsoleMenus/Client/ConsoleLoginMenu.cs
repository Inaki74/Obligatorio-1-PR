
using ConsoleMenus;
using ConsoleMenusInterfaces;
using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using Common;

namespace ConsoleMenus.Client
{
    public class ConsoleLoginMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => _nextMenu;


        public void Action(string answer)
        {
            // Intentar login
            Console.WriteLine($"Attempting to login with {answer}");
            VaporStatusMessage message = IClientHandler.Instance.Login(answer);

            switch(message.Code)
            {
                case StatusCodeConstants.OK:
                    _nextMenu = new ConsoleMainMenu();
                    Console.WriteLine($"Welcome {answer}!");
                    break;
                case StatusCodeConstants.INFO:
                    _nextMenu = new ConsoleMainMenu();
                    Console.WriteLine($"INFO: {message.Message}");
                    Console.WriteLine($"Welcome {answer}!");
                    break;
                case StatusCodeConstants.ERROR_CLIENT:
                    _nextMenu = new ConsoleLoginMenu();
                    Console.WriteLine("Couldn't login...");
                    Console.WriteLine($"{message.Message}");
                    break;
            }
        }

        public void PrintMenu()
        {
            Console.WriteLine("Please enter your username:");
        }
    }
}