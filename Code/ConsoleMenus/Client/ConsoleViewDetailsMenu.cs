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

        private bool _isOwner;

        public ConsoleViewDetailsMenu(bool isOwner)
        {
            this._isOwner = isOwner;
        }
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
                game.OverallScore = response.GameScore;
                
                ShowGameDetails(game);
                ShowReviews(reviewList);
                _nextMenu = new ConsoleDownloadCoverMenu(_isOwner);
            }
            else
            {
                Console.WriteLine(response.Message);
                _nextMenu = new ConsoleGameMenu(_isOwner);
            }

            return false;
        }

        private void ShowGameDetails(Game game)
        {
            Console.WriteLine($"Game: {game.Title}");
            Console.WriteLine($"Genre: {game.Genre}");
            Console.WriteLine($"ESRB: {game.ESRB}");
            Console.WriteLine($"Synopsis: {game.Synopsis}");
            Console.WriteLine($"Score: {game.OverallScore}");
            Console.WriteLine($"Owner: {game.Owner.Username}");
        }

        private void ShowReviews(List<Review> reviewList)
        {
            Console.WriteLine($"--- Game reviews ---");
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