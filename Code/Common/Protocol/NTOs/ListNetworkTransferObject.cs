using System.Collections.Generic;
using Common.Protocol.Interfaces;

namespace Common.Protocol.NTOs
{
    public class ListNetworkTransferObject<T> : INetworkTransferObject<List<T>>
    {
        private INetworkTransferObject<T> _tNTO;

        private List<T> _tList;


        public ListNetworkTransferObject(INetworkTransferObject<T> tNto)
        {
            _tNTO = tNto;
        }

        public string Encode()
        {
            string encoded = "";
            int cantReviews = _tList.Count;
            encoded += VaporProtocolHelper.FillNumber(cantReviews, VaporProtocolSpecification.LIST_ELEMENTS_MAXSIZE);

            foreach(T obj in _tList)
            {
                _tNTO.Load(obj);
                encoded += _tNTO.Encode();
            }

            return encoded;
        }

        public List<T> Decode(string toDecode)
        {
            List<T> ret = new List<T>();
            int cantReviews = int.Parse(toDecode.Substring(0, VaporProtocolSpecification.LIST_ELEMENTS_MAXSIZE));
            string restOfData = toDecode.Substring(VaporProtocolSpecification.LIST_ELEMENTS_MAXSIZE, toDecode.Length - VaporProtocolSpecification.LIST_ELEMENTS_MAXSIZE);

            int index = 0;
            for(int i = 0; i < cantReviews; i++)
            {
                string data = restOfData.Substring(index);

                T obj = _tNTO.Decode(data);
                ret.Add(obj);

                _tNTO.Load(obj);
                index += _tNTO.Encode().Length;
            }
            return ret;
        }

        public void Load(List<T> obj)
        {
            _tList = obj;
        }
        
    }
}