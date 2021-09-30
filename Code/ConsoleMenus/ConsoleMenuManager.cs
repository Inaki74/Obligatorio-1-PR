using ConsoleMenusInterfaces;
using System;
using ConsoleMenus.Client;
using ConsoleMenus.Server;
using Exceptions.ConnectionExceptions;

namespace ConsoleMenus
{
    public class ConsoleMenuManager : IConsoleMenuManager
    {
        private IConsoleMenu _currentMenu;

        public bool Exit => _exit;

        private bool _exit;

        public ConsoleMenuManager(bool client)
        {
            if(client)
            {
                _currentMenu = new ConsoleWelcomeMenu();
            }
            else
            {
                _currentMenu = new ConsoleStartServerMenu();
            }
            
        }

        public void ExecuteMenu()
        {
            Console.WriteLine("");
            _currentMenu.PrintMenu();
            Console.WriteLine("");

            var answer = "";
            if(_currentMenu.RequiresAnswer)
            {
                answer = Console.ReadLine();
                Console.WriteLine("");
            }

            try
            {
                bool exit = _currentMenu.Action(answer);
                _currentMenu = _currentMenu.NextMenu;
                _exit = exit;
            }
            catch(ExitException e)
            {
                Console.WriteLine(e.Message);
                _currentMenu = new ConsoleExitMenu();
                _currentMenu.PrintMenu();
                _exit = true;
            }
        }
    }
}