
using ConsoleMenus;
using ConsoleMenusInterfaces;
using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using Common;
using Domain;
using Common.Protocol.NTOs;

namespace ConsoleMenus.Client
{
    public class ConsolePublishGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            // Titulo juego, genero juego, ESRB, sinopsis, caratula, con separadores.
            // DOOM 1994 Mature Shooter Un juego en donde matas bichos.
            // 12007JamesBond
            // Escribir juego
            GameNetworkTransferObject input = GetGame();

            VaporStatusResponse response = IClientHandler.Instance.PublishGame(input);

            _nextMenu = new ConsoleMainMenu();
            Console.WriteLine(response.Message);
            
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Please enter the game's details: ");
        }

        private GameNetworkTransferObject GetGame()
        {
            GameNetworkTransferObject input = new GameNetworkTransferObject();

            Console.WriteLine("Game Title: ");
            string title = Console.ReadLine();

            Console.WriteLine("Genre: ");
            string genre = Console.ReadLine();

            Console.WriteLine("Public ESRB rating: ");
            string esrb = Console.ReadLine();

            Console.WriteLine("Enter the game's synopsis: ");
            string synopsis = Console.ReadLine();

            Console.WriteLine("Enter the Cover image Path: ");
            string coverPath = Console.ReadLine();

            input.Title = title;
            input.Genre = genre;
            input.ESRB = esrb;
            input.Synopsis = synopsis;
            input.CoverPath = coverPath;

            return input;
        }
    }
}