using System;
using ClientApplicationInterfaces;

namespace ClientApplication
{
    public class ClientSession : IClientSession
    {
        private readonly string _username;
        public string Username => _username;

        public ClientSession(string username)
        {
            _username = username;
        }
    }
}
