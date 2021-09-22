using System;

namespace Common.Protocol.Interfaces
{
    public interface INetworkTransferObject<T>
    {
        string Encode();
        T Decode();
        void Load(T obj);
    }
}
