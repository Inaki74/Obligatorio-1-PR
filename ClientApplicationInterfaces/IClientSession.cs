using System;

namespace ClientApplicationInterfaces
{
    public interface IClientSession
    {
        string Username {get;}
        int GameSelectedId { get; set; }
    }
}
