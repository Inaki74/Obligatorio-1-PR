
using ConsoleMenus;
using ConsoleMenusInterfaces;
using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using Common;
using Domain;

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
            string input = GetGame();

            IClientHandler.Instance.PublishGame(input);

            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Please enter the game's details: ");
        }

        private string GetGame()
        {
            string input = "";

            Console.WriteLine("Game Title: ");
            string title = Console.ReadLine();
            input += VaporProtocolHelper.FillNumber(title.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + title;

            Console.WriteLine("Genre: ");
            string genre = Console.ReadLine();
            input += VaporProtocolHelper.FillNumber(genre.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + genre;

            Console.WriteLine("Public ESRB rating: ");
            string esrb = Console.ReadLine();
            input += VaporProtocolHelper.FillNumber(esrb.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + esrb;

            Console.WriteLine("Enter the game's synopsis");
            string sinopsis = Console.ReadLine();
            input += VaporProtocolHelper.FillNumber(sinopsis.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + sinopsis;

            Console.WriteLine("Caratula: (MAS TARDE)");
            //string title = Console.ReadLine();
            //input += title.Length.ToString() + title;

            return input;
        }

        
    }
}