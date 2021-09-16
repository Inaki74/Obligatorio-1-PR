using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus.Client
{
    public class ConsoleExitMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => false;

        public IConsoleMenu NextMenu => throw new NotImplementedException();

        public void Action(string answer)
        {
            //Enviar el mensaje
        }

        public void PrintMenu()
        {
            Console.WriteLine("Exiting application...");
        }
    }
}
