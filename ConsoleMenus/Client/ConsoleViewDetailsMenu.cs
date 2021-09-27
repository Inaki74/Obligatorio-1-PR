using System;
using System.Collections.Generic;
using ClientApplicationInterfaces;
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
            List<Review> reviewList = response.ReviewsList;
            Game game = response.Game;
            Console.WriteLine($"Game: {game.Title}");
            Console.WriteLine($"Score: {response.GameScore}");
            Console.WriteLine($"Description: {game.Synopsis}");
            Console.WriteLine($"Owner: {game.Owner}");
            Console.WriteLine($"Reviews:");
            showReviews(reviewList);
            _nextMenu = new ConsoleMainMenu();

            return false;
        }

        private void showReviews(List<Review> ReviewList)
        {
            foreach (Review review in ReviewList)
            {
                Console.WriteLine($"Score: {review.Score}");
                Console.WriteLine($"Description: {review.Description}");
                Console.WriteLine($"Publisher: {review.ReviewPublisher.Username}");
                Console.WriteLine($"----------------");
            }
        }
    }
}