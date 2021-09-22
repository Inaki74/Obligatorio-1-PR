using System;
using System.Collections.Generic;
using ClientApplicationInterfaces;
using ConsoleMenusInterfaces;
using Common.Protocol;
using Common;
using Domain;

namespace ConsoleMenus.Client
{
    public class ConsoleGetGamesMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;

        public bool RequiresAnswer => false;

        public bool Action(string answer)
        {
            // Conseguir la lista
            VaporStatusResponse response = IClientHandler.Instance.GetGames();

            // Imprimirla
            if(response.Code == StatusCodeConstants.OK)
            {
                Console.WriteLine("TODOS LOS JUEGOS \n");
                foreach(Game game in response.GamesList)
                {
                    Console.WriteLine($"   {game.Title}");
                }
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
    }
}
