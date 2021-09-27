using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using ConsoleMenusInterfaces;
using Domain.BusinessObjects;

namespace ConsoleMenus.Client
{
    public class ConsoleViewReviewMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;
        public bool RequiresAnswer => false;
        public void PrintMenu()
        {
            Console.WriteLine("Please enter the review author: ");
        }

        public bool Action(string answer)
        {
            
                string username = Console.ReadLine();
                VaporStatusResponse response = IClientHandler.Instance.GetGameReview(username);
                Review review = response.Review;
                Console.WriteLine($"Game: {review.Game.Title}");
                Console.WriteLine($"Score: {review.Score}");
                Console.WriteLine($"Description: {review.Description}");
                Console.WriteLine($"Publisher: {review.ReviewPublisher.Username}");
                _nextMenu = new ConsoleMainMenu();
            
            
            return false;
        }

        
    }
}