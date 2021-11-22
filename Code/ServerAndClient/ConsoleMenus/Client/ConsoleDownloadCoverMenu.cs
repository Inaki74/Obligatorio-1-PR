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
        public bool RequiresAnswer => true;

        private bool _isOwner;

        private const string YES_RESPONSE = "y";

        public ConsoleDownloadCoverMenu(bool isOwner)
        {
            _isOwner = isOwner;
        }
        public void PrintMenu()
        {
            Console.WriteLine("Want to download Cover? ");
            Console.WriteLine($"(answer '{YES_RESPONSE}' if yes, anything else if not)");
            
        }

        public bool Action(string answer)
        {
            if (answer.ToLower() == YES_RESPONSE)
            {
                Console.WriteLine("Where do you want to download the image? (path)");
                string path = Console.ReadLine();
                
                VaporStatusResponse response = IClientHandler.Instance.DownloadGameCover(path).Result;
                
                if(response.Code == StatusCodeConstants.OK)
                {
                    Console.WriteLine("Cover downloaded succesfully");
                }
                else
                {
                    Console.WriteLine(response.Message);
                }
            }
            
            _nextMenu = new ConsoleGameMenu(_isOwner);
            return false;
        }
    }
}