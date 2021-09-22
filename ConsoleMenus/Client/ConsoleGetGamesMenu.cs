using System;
using System.Collections.Generic;
using ClientApplicationInterfaces;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleGetGamesMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;

        public bool RequiresAnswer => false;

        public bool Action(string answer)
        {
            // Conseguir la lista
            List<string> listaDeJuegos = IClientHandler.Instance.GetGames();

            // Imprimirla
            Console.WriteLine("TODOS LOS JUEGOS \n");
            foreach(string game in listaDeJuegos)
            {
                Console.WriteLine($"   {game}");
            }
            Console.WriteLine("");

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Fetching all games ...");
        }
    }
}
