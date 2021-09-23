using System;
using ClientApplicationInterfaces;
using Common;
using Common.Protocol;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleSelectGameMenu : ConsoleMenusBase, IConsoleMenu
    {
        public IConsoleMenu NextMenu => _nextMenu;
        public bool RequiresAnswer => true;
        public void PrintMenu()
        {
            Console.WriteLine("Title:");
        }

        public bool Action(string answer)
        {
            Console.WriteLine("Looking for game...");
            VaporStatusResponse response = IClientHandler.Instance.SelectGame(answer);
            if (response.Code == StatusCodeConstants.OK)
            {
                bool isOwnerResponse = IClientHandler.Instance.CheckIsOwner();

                _nextMenu = new ConsoleGameMenu(isOwnerResponse);
            }
            else
            {
                _nextMenu = new ConsoleMainMenu();
            }
            
            Console.WriteLine(response.Message);
            return false;
        }
    }
}