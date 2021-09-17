using System;
using ConsoleMenusInterfaces;

namespace ConsoleMenus
{
    public class ConsoleMessageMenu : ConsoleMenusBase, IConsoleMenu
    {
        public bool RequiresAnswer => true;

        public IConsoleMenu NextMenu => throw new NotImplementedException();

        public void Action(string answer)
        {
            //Enviar el mensaje
        }

        public void PrintMenu()
        {
            Console.WriteLine("Write a message.");
        }
    }
}