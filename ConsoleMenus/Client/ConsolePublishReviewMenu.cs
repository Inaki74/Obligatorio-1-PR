using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsolePublishReviewMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => _nextMenu;

        public bool Action(string answer)
        {
            return false;
        }

        public void PrintMenu()
        {
            Console.WriteLine("");
        }
    }
}
