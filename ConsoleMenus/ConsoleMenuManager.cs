using ConsoleMenusInterfaces;
using System;
using ConsoleMenus.Client;

namespace ConsoleMenus
{
    public class ConsoleMenuManager : IConsoleMenuManager
    {
        private IConsoleMenu _currentMenu;

        public bool Exit => _exit;

        private bool _exit;

        public ConsoleMenuManager()
        {
            _currentMenu = new ConsoleWelcomeMenu();
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

            _currentMenu.Action(answer);
            _currentMenu = _currentMenu.NextMenu;
        }
    }
}