using System;
using ClientApplicationInterfaces;
using Common.Protocol.NTOs;
using ConsoleMenusInterfaces;
using Domain.BusinessObjects;

namespace ConsoleMenus.Client
{
    public class ConsolePublishReviewMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            ReviewNetworkTransferObject reviewNTO = new ReviewNetworkTransferObject();
            
            Review review = GetReviewInput();
            reviewNTO.Load(review);
            string response = IClientHandler.Instance.PublishReview(reviewNTO);

            _nextMenu = new ConsoleGameMenu(false);
            Console.WriteLine(response);

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Write a review of the game. You must provide a score (1 to 5).\n");
        }

        private Review GetReviewInput()
        {
            Console.WriteLine("Write a review about the game. Note: You can't use enter.");
            string description = Console.ReadLine();

            Console.WriteLine("");
            Console.WriteLine("Score:");
            string score = Console.ReadLine();

            while(!VerifyScore(ref score))
            {
                Console.WriteLine("\nSCORE input incorrect, reenter:");
                score = Console.ReadLine();
            }

            if(score == "0")
            {
                Console.WriteLine("\nNo score given. A score is required. Returning to game menu.");
                return null;
            }

            Review ret = new Review();
            ret.Description = description;
            ret.Score = int.Parse(score);

            return ret;
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
                for(int i = 1; i <= Review.MAX_SCORE; i++)
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
