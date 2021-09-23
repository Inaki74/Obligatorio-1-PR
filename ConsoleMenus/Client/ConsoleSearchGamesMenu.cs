using System;
using ClientApplicationInterfaces;
using Common;
using Common.Protocol;
using Common.Protocol.NTOs;
using ConsoleMenusInterfaces;
using Domain.BusinessObjects;
using Domain.HelperObjects;

namespace ConsoleMenus.Client
{
    public class ConsoleSearchGamesMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;

        public bool RequiresAnswer => false;

        public bool Action(string answer)
        {
            GameSearchQuery query = GetQueryInput();

            if(query == null)
            {
                _nextMenu = new ConsoleMainMenu();
                return false;
            }

            GameSearchQueryNetworkTransferObject queryNTO = new GameSearchQueryNetworkTransferObject();
            queryNTO.Load(query);

            VaporStatusResponse response = IClientHandler.Instance.SearchGames(queryNTO);

            if(response.Code == StatusCodeConstants.OK)
            {
                Console.WriteLine("GAMES FOUND:\n");
                foreach(Game game in response.GamesList)
                {
                    Console.WriteLine($"   {game.Title}");
                }
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine(response.Message);
            }

            _nextMenu = new ConsoleMainMenu();
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("\nIntroduce the following information.\n Leave blank if you don't want that parameter to influence on the search.\n");
        }

        private GameSearchQuery GetQueryInput()
        {
            Console.WriteLine("TITLE:");
            string title = Console.ReadLine();

            Console.WriteLine("");
            Console.WriteLine("GENRE:");
            string genre = Console.ReadLine();

            Console.WriteLine("");
            Console.WriteLine("SCORE (1 to 5):");
            string score = Console.ReadLine();

            while(!VerifyScore(ref score))
            {
                Console.WriteLine("\nSCORE input incorrect, reenter:");
                score = Console.ReadLine();
            }

            if(string.IsNullOrEmpty(title) && string.IsNullOrEmpty(genre) && score == "0")
            {
                Console.WriteLine("\nNo parameters introduced, cancelling search.");
                return null;
            }

            return new GameSearchQuery(title, genre, int.Parse(score));
        }

        private bool VerifyScore(ref string score)
        {
            bool isCorrect = false;

            if(string.IsNullOrEmpty(score))
            {
                score = "0";
                return true;
            }

            try
            {
                int parsedScore = int.Parse(score);
                int MAXSCORE = 5; // TODO: Make it a constant somewhere.
                for(int i = 1; i <= MAXSCORE; i++)
                {
                    if(parsedScore == i)
                    {
                        isCorrect = true;
                    }
                }
            }
            catch(Exception e)
            {
                isCorrect = false;
            }

            return isCorrect;
        }
    }
}
