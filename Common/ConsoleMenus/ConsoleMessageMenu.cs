using System;
using Common.ConsoleMenus.Interfaces;

namespace Common.ConsoleMenus
{
    public class ConsoleMessageMenu : IConsoleMenu
    {
        public bool RequiresAnswer => true;

        public void Action()
        {
            throw new NotImplementedException();
        }

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