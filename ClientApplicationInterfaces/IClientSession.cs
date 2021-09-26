using System;

namespace ClientApplicationInterfaces
{
    public interface IClientSession
    {
        string Username {get;}
        
        string gameSelected { get; set; }
        int gameSelectedId { get; set; }
    }
}
