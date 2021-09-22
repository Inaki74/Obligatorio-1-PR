using System;
using Common.Protocol.Interfaces;
using Domain;

namespace Common.Protocol.NTOs
{
    public class GameSearchQueryNetworkTransferObject : INetworkTransferObject<GameSearchQuery>
    {
        private GameSearchQuery _gameQuery;
        public GameSearchQuery Decode(string toDecode)
        {
            throw new NotImplementedException();
        }

        public string Encode()
        {
            throw new NotImplementedException();
        }

        public void Load(GameSearchQuery obj)
        {
            _gameQuery = obj;
        }
    }
}
