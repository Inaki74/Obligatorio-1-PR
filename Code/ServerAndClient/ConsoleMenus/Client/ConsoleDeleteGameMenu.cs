using System;
using ClientApplicationInterfaces;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleDeleteGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        private const string YES_RESPONSE = "y";

        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            string response = "";
            if(answer == YES_RESPONSE)
            {
                response = IClientHandler.Instance.DeleteGame();
                Console.WriteLine(response);
                _nextMenu = new ConsoleMainMenu();
            }
            else
            {
                Console.WriteLine("Game wasn't deleted.");
                _nextMenu = new ConsoleGameMenu(true);
            }

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine($"Are you sure you want to delete this game? \nPress {YES_RESPONSE} to continue. Any other key to cancel)");
        }
    }
}
