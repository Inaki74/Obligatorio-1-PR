using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleExitMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => throw new NotImplementedException();

        public bool Action(string answer)
        {
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Exiting application...");
        }
    }
}
