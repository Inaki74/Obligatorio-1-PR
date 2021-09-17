using System.Collections.Generic;
using System.Dynamic;

namespace DataAccess
{
    public interface IDataAccess<T>
    {
        T Get(int id);

        T Get(string id);
        List<T> GetALl();
        void Add(T elem);
        void Delete(T elem);
        void Update(T elem);

    }
}