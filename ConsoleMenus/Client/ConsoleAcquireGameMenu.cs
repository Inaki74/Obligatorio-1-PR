using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleAcquireGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;
        public bool RequiresAnswer => true;
        
        private const string YES_RESPONSE = "y";

        private bool _isGameOwner;
        public ConsoleAcquireGameMenu(bool isGameOwner)
        {
            _isGameOwner = isGameOwner;
        }
        public void PrintMenu()
        {
            Console.WriteLine($"Are you sure you want to acquire this game? \nPress {YES_RESPONSE} to continue. Any other key to cancel)");
        }

        public bool Action(string answer)
        {
            
            if(answer == YES_RESPONSE)
            {
                VaporStatusResponse response = IClientHandler.Instance.AcquireGame();
                Console.WriteLine(response.Message);
            }
            else
            {
                Console.WriteLine("Game wasn't acquired.");
            }
            _nextMenu = new ConsoleGameMenu(_isGameOwner);
            return false;
        }
    }
}