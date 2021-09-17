
using ConsoleMenus;
using ConsoleMenusInterfaces;
using System;
using ClientApplicationInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleLoginMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => throw new System.NotImplementedException();


        public void Action(string answer)
        {
            // Intentar login
            Console.WriteLine($"You are trying to login right now duh: {answer}");
            IClientHandler.Instance.Login(answer);
            // REQ 1
        }

        public void PrintMenu()
        {
            Console.WriteLine("Please enter your username:");
        }
    }
}