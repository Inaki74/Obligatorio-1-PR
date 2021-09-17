using ConsoleMenusInterfaces;
using System;
using ConsoleMenus.Client;
using ConsoleMenus.Server;

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
            _currentMenu.PrintMenu();

            var answer = "";
            if(_currentMenu.RequiresAnswer)
            {
                Console.WriteLine("Answer: ");
                answer = Console.ReadLine();
            }

            try
            {
                _currentMenu.Action(answer);
                _currentMenu = _currentMenu.NextMenu;
            }
            catch(Exception e)
            {
                Console.WriteLine($"An error ocurred: {e}");
                _currentMenu = new ConsoleExitMenu();
                _currentMenu.PrintMenu();
                _exit = true;
            }
        }
    }
}