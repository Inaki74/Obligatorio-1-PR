using System;

namespace Common.Protocol.Interfaces
{
    public interface IVaporHeader
    {
        byte[] Create();
        int GetLength();
    }
}
