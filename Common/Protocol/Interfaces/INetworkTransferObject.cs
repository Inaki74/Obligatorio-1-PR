using System;

namespace Common.Protocol.Interfaces
{
    public interface INetworkTransferObject<T>
    {
        string Encode();
        T Decode(string toDecode);
        void Load(T obj);
    }
}
