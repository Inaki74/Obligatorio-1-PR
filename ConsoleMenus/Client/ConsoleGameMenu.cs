using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        private const string MODIFY_OPTION = "1";
        private const string DELETE_OPTION = "2";
        private const string PUBLIC_REVIEW_OPTION = "3";
        private const string VIEW_RATING_OPTION = "4";
        private const string VIEW_REVIEW_OPTION = "5";
        private const string VIEW_DETAILS_OPTION = "6";
        private const string GO_BACK_OPTION = "0";
        
        public IConsoleMenu NextMenu => _nextMenu;
        public bool RequiresAnswer => true;
        public void PrintMenu()
        {
            Console.WriteLine("Select an option: ");
            Console.WriteLine($"{MODIFY_OPTION}. Modify game");
            Console.WriteLine($"{DELETE_OPTION}. Delete game.");
            Console.WriteLine($"{PUBLIC_REVIEW_OPTION}. Public review");
            Console.WriteLine($"{VIEW_RATING_OPTION}. View rating.");
            Console.WriteLine($"{VIEW_REVIEW_OPTION}. View game reviews.");
            Console.WriteLine($"{VIEW_DETAILS_OPTION}. View Game details.");
            Console.WriteLine($"{GO_BACK_OPTION}. Go back.");
        }

        public bool Action(string answer)
        {
            //switch con cada menu dependiendo de las constantes
            //igual al switch en Main Menu
            switch (answer)
            {
                case MODIFY_OPTION:
                    break;
                case GO_BACK_OPTION:
                    _nextMenu = new ConsoleMainMenu();
                    break;
                default:
                    break;
            }

            return false;
        }
    }
}