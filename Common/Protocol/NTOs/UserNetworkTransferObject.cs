using System;
using Common.Protocol.Interfaces;

namespace Common.Protocol.NTOs
{
    public class UserNetworkTransferObject : INetworkTransferObject
    {
        public string Username {get; set;}
        public int GetLength()
        {
            return Username.Length;
        }

        public string ToCharacters()
        {
            return Username;
        }
    }
}
