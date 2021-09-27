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
                VaporStatusResponse isOwnerResponse = IClientHandler.Instance.CheckIsOwner();

                if(isOwnerResponse.Code != StatusCodeConstants.OK && isOwnerResponse.Code != StatusCodeConstants.ERROR_CLIENT_NOTAUTHORIZED)
                {
                    Console.WriteLine(isOwnerResponse.Message);
                    _nextMenu = new ConsoleMainMenu();
                    return false;
                }
                else
                {
                    Console.WriteLine("Game selected!");
                    _nextMenu = new ConsoleGameMenu(isOwnerResponse.Code == StatusCodeConstants.OK);
                }
            }
            else
            {
                Console.WriteLine(response.Message);
                _nextMenu = new ConsoleMainMenu();
            }

            return false;
        }
    }
}