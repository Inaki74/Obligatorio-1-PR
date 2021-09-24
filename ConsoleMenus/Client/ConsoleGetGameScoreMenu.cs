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
            // Conseguir la lista
            VaporStatusResponse response = IClientHandler.Instance.GetGameScore();

            // Imprimirla
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

                foreach(Review review in response.ReviewsList)
                {
                    Console.WriteLine($"   {review.ReviewPublisher.Username}");
                }
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
    }
}
