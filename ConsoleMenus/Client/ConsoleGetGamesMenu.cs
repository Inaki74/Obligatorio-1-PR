using System;
using System.Collections.Generic;
using ClientApplicationInterfaces;
using ConsoleMenusInterfaces;
using Common.Protocol;
using Common;
using Domain.BusinessObjects;

namespace ConsoleMenus.Client
{
    public class ConsoleGetGamesMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;

        public bool RequiresAnswer => false;

        public bool Action(string answer)
        {
            VaporStatusResponse response = IClientHandler.Instance.GetGames();
            
            if(response.Code == StatusCodeConstants.OK)
            {
                bool listIsEmpty = response.GamesList.Count == 0;
                if(!listIsEmpty)
                {
                    Console.WriteLine("GAMES FOUND:\n");
                }
                else
                { 
                    Console.WriteLine("No games were found.");
                }

                ShowGameList(response.GamesList);
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
            Console.WriteLine("Fetching all games ...");
        }

        private void ShowGameList(List<Game> gameList)
        {
            foreach(Game game in gameList)
            {
                Console.WriteLine($"  {game.Title}");
            }
        }
    }
}
