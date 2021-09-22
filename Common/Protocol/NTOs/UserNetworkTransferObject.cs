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
            throw new NotImplementedException();
        }

        public string Encode()
        {
            return Username;
        }

        public User Decode(string toDecode)
        {
            throw new NotImplementedException();
        }
    }
}
