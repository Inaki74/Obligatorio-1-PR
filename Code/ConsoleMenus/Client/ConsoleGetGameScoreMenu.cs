using System;
using System.Collections.Generic;
using ClientApplicationInterfaces;
using ConsoleMenusInterfaces;
using Common.Protocol;
using Common;
using Domain.BusinessObjects;

namespace ConsoleMenus.Client
{
    public class ConsoleGetGameScoreMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;

        public bool RequiresAnswer => false;
        private bool _isGameOwner;

        public ConsoleGetGameScoreMenu(bool isGameOwner)
        {
            _isGameOwner = isGameOwner;
        }

        public bool Action(string answer)
        {
            VaporStatusResponse response = IClientHandler.Instance.GetGameScore();
            
            if(response.Code == StatusCodeConstants.OK)
            {
                bool listIsEmpty = response.ReviewsList.Count == 0;

                if(listIsEmpty)
                {
                    Console.WriteLine("NO REVIEWS FOUND.");
                }
                else
                {
                    Console.WriteLine("GAME OVERALL SCORE\n");
                    Console.WriteLine($"   {response.GameScore}");
                    Console.WriteLine("CALCULATED FROM REVIEWS FROM:\n");
                }

                ShowReviews(response.ReviewsList);
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine(response.Message);
            }

            _nextMenu = new ConsoleGameMenu(_isGameOwner);

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Fetching game reviews ...");
        }

        private void ShowReviews(List<Review> reviewsList)
        {
            foreach(Review review in reviewsList)
            {
                Console.WriteLine($"  {review.ReviewPublisher.Username}");
            }
        }
    }
}
