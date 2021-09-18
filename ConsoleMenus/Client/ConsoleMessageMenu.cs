using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus
{
    public class ConsoleMessageMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => throw new NotImplementedException();

        public bool Action(string answer)
        {
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("Write a message.");
        }
    }
}