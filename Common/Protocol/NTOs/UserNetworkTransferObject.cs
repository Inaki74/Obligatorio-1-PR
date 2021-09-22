using System;
using Common.Protocol.Interfaces;
using Domain;

namespace Common.Protocol.NTOs
{
    public class UserNetworkTransferObject : INetworkTransferObject<User>
    {
        public string Username {get; set;}

        public void Load(User obj)
        {
            Username = obj.Username;
        }

        public string Encode()
        {
            return Username;
        }

        public User Decode(string toDecode)
        {
            return new User(toDecode, -1);
        }
    }
}
