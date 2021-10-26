
using ConsoleMenus;
using ConsoleMenusInterfaces;
using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using Common;
using Common.Protocol.NTOs;

namespace ConsoleMenus.Client
{
    public class ConsoleLoginMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => _nextMenu;


        public bool Action(string answer)
        {
            Console.WriteLine($"Attempting to login with {answer}");

            UserNetworkTransferObject user = new UserNetworkTransferObject();
            
            user.Username = answer;
            VaporStatusResponse response = IClientHandler.Instance.Login(user);

            switch(response.Code)
            {
                case StatusCodeConstants.OK:
                    _nextMenu = new ConsoleMainMenu();
                    Console.WriteLine($"Welcome {answer}!");
                    break;
                case StatusCodeConstants.INFO:
                    _nextMenu = new ConsoleMainMenu();
                    Console.WriteLine($"INFO: User didn't exist, created new user.");
                    Console.WriteLine($"Welcome {answer}!");
                    break;
                case StatusCodeConstants.ERROR_CLIENT:
                    _nextMenu = new ConsoleLoginMenu();
                    Console.WriteLine("Couldn't login...");
                    Console.WriteLine($"{response.Message}");
                    break;
            }

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Please enter your username:");
        }
    }
}