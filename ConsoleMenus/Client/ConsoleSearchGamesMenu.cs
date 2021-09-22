using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using Common.Protocol.NTOs;
using ConsoleMenusInterfaces;
using Domain;

namespace ConsoleMenus.Client
{
    public class ConsoleSearchGamesMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => throw new NotImplementedException();

        public bool RequiresAnswer => false;

        public bool Action(string answer)
        {
            string title = Console.ReadLine();
            string genre = Console.ReadLine();

            string score = Console.ReadLine();
            while(VerifyScore(score))
            {
                score = Console.ReadLine();
            }

            GameSearchQuery query = new GameSearchQuery(title, genre, int.Parse(score));
            GameSearchQueryNetworkTransferObject queryNTO = new GameSearchQueryNetworkTransferObject();
            queryNTO.Load(query);

            VaporStatusResponse response = IClientHandler.Instance.SearchGames(queryNTO);

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("\nIntroduce the following information in order: TITLE, GENRE and SCORE (1 to 5).");
        }

        private bool VerifyScore(string score)
        {
            bool isCorrect = false;

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
