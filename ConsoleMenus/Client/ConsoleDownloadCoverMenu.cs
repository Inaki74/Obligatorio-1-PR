using System;
using ClientApplicationInterfaces;
using Common;
using Common.Protocol;
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
                Console.WriteLine("Where do you want to download the image? (path)");
                string path = Console.ReadLine();
                VaporStatusResponse response = IClientHandler.Instance.DownloadGameCover(path);
                Console.WriteLine("Cover downloaded succesfully");
            }
            
            _nextMenu = new ConsoleMainMenu();
            return false;
        }
    }
}