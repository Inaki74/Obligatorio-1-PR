using System;
using ClientApplicationInterfaces;
using Common;
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

            if(response.Code == StatusCodeConstants.OK)
            {
                Review review = response.Review;
                ShowReview(review);
            }
            else
            {
                Console.WriteLine(response.Message);
            }
            _nextMenu = new ConsoleMainMenu();
            
            return false;
        }

        private void ShowReview(Review review)
        {
            Console.WriteLine($"Game: {review.Game.Title}");
            Console.WriteLine($"Score: {review.Score}");
            Console.WriteLine($"Description: {review.Description}");
            Console.WriteLine($"Publisher: {review.ReviewPublisher.Username}");
        }

        
    }
}