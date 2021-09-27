using System;
using ClientApplicationInterfaces;

namespace ClientApplication
{
    public class ClientSession : IClientSession
    {
        private readonly string _username;
        public string Username => _username;
        //public string gameSelected { get; set; }
        public int GameSelectedId { get; set; }

        public ClientSession(string username)
        {
            _username = username;
            GameSelectedId = -1;
        }
    }
}
