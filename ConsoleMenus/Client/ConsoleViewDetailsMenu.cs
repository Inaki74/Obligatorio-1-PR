using System;
using System.Collections.Generic;
using ClientApplicationInterfaces;
using Common;
using Common.Protocol;
using ConsoleMenusInterfaces;
using Domain.BusinessObjects;

namespace ConsoleMenus.Client
{
    public class ConsoleViewDetailsMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;
        public bool RequiresAnswer => false;
        public void PrintMenu()
        {
            Console.WriteLine("Fetching game details... ");
        }

        public bool Action(string answer)
        {
            VaporStatusResponse response = IClientHandler.Instance.GetGameDetails();
            
            if (response.Code == StatusCodeConstants.OK)
            {
                List<Review> reviewList = response.ReviewsList;
                Game game = response.Game;
                Console.WriteLine($"Game: {game.Title}");
                Console.WriteLine($"Score: {response.GameScore}");
                Console.WriteLine($"Synopsis: {game.Synopsis}");
                Console.WriteLine($"ESRB: {game.ESRB}");
                Console.WriteLine($"Owner: {game.Owner.Username}");
                Console.WriteLine($"--- Game reviews ---");
                showReviews(reviewList);
                _nextMenu = new ConsoleDownloadCoverMenu();
            }
            else
            {
                Console.WriteLine(response.Message);
                _nextMenu = new ConsoleMainMenu();
            }

            return false;
        }

        private void showReviews(List<Review> reviewList)
        {
            foreach (Review review in reviewList)
            {
                Console.WriteLine($"Score: {review.Score}");
                Console.WriteLine($"Description: {review.Description}");
                Console.WriteLine($"Publisher: {review.ReviewPublisher.Username}");
                Console.WriteLine($"--");
            }
        }
    }
}