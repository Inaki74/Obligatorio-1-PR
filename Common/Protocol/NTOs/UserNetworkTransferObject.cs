using System;
using Common.Protocol.Interfaces;

namespace Common.Protocol.NTOs
{
    public class UserNetworkTransferObject : INetworkTransferObject
    {
        public string Username {get; set;}

        public string ToCharacters()
        {
            return Username;
        }
    }
}
