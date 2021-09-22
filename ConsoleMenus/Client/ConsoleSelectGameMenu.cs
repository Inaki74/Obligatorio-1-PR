using System;
using ClientApplicationInterfaces;
using Common.Protocol;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleSelectGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu { get; }
        public bool RequiresAnswer => true;
        public void PrintMenu()
        {
            Console.WriteLine("Title:");
        }

        public bool Action(string answer)
        {
            Console.WriteLine("Looking for game...");
            VaporStatusResponse response = IClientHandler.Instance.SelectGame(answer);
            _nextMenu = new ConsoleGameMenu();
        }
    }
}