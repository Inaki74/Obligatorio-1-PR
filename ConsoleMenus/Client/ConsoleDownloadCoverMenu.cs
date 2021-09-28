using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleDownloadCoverMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;
        public bool RequiresAnswer => false;
        public void PrintMenu()
        {
            Console.WriteLine("Want to download Cover? ");
            Console.WriteLine("(Y if yes, anything else if not)");
            
        }

        public bool Action(string answer)
        {
            string reply = Console.ReadLine();
            if (reply == "Y")
            {
                
            }



            return false;
        }
    }
}