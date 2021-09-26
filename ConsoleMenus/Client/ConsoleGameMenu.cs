using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        private const string VIEW_GAME_SCORE_OPTION = "1";
        private const string VIEW_REVIEW_OPTION = "2";
        private const string VIEW_DETAILS_OPTION = "3";
        private const string ACQUIRE_GAME_OPTION = "4";
        private const string PUBLIC_REVIEW_OPTION = "5";
        private const string MODIFY_OPTION = "6";
        private const string DELETE_OPTION = "7";
        
        private const string GO_BACK_OPTION = "0";

        private bool _isGameOwner = false;

        public IConsoleMenu NextMenu => _nextMenu;
        public bool RequiresAnswer => true;

        public ConsoleGameMenu(bool isOwner)
        {
            _isGameOwner = isOwner;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Select an option: ");
            
            Console.WriteLine($"{VIEW_GAME_SCORE_OPTION}. View game score.");
            Console.WriteLine($"{VIEW_REVIEW_OPTION}. View game reviews.");
            Console.WriteLine($"{VIEW_DETAILS_OPTION}. View Game details.");
            Console.WriteLine($"{ACQUIRE_GAME_OPTION}. Acquire game.");

            // No tiene sentido que el creador del juego pueda poner un review.
            if(!_isGameOwner)
            {
                Console.WriteLine($"{PUBLIC_REVIEW_OPTION}. Public review");
            }

            if(_isGameOwner)
            {
                Console.WriteLine($"{MODIFY_OPTION}. Modify game");
                Console.WriteLine($"{DELETE_OPTION}. Delete game.");
            }

            Console.WriteLine($"{GO_BACK_OPTION}. Go back.");
        }

        public bool Action(string answer)
        {
            //switch con cada menu dependiendo de las constantes
            //igual al switch en Main Menu

            if(!_isGameOwner && (answer == DELETE_OPTION || answer == MODIFY_OPTION))
            {
                answer = "";
            }

            if(_isGameOwner && answer == PUBLIC_REVIEW_OPTION)
            {
                answer = "";
            }

            switch (answer)
            {
                case VIEW_GAME_SCORE_OPTION:
                    _nextMenu = new ConsoleGetGameScoreMenu(_isGameOwner);
                    break;
                case MODIFY_OPTION:
                    _nextMenu = new ConsoleModifyGameMenu();
                case ACQUIRE_GAME_OPTION:
                    _nextMenu = new ConsoleAcquireGameMenu(_isGameOwner);
                    break;
                case PUBLIC_REVIEW_OPTION:
                    _nextMenu = new ConsolePublishReviewMenu();
                    break;
                case MODIFY_OPTION:
                    break;
                case DELETE_OPTION:
                    _nextMenu = new ConsoleDeleteGameMenu();
                    break;
                case GO_BACK_OPTION:
                    _nextMenu = new ConsoleMainMenu();
                    break;
                default:
                    Console.WriteLine("That's not a valid option");
                    _nextMenu = this;
                    break;
            }

            return false;
        }
    }
}







