using System.Collections.Generic;
using System.Dynamic;

namespace DataAccess
{
    public interface IDataAccess<T>
    {
        T Get(string id);

        T Get(int id);
        T GetCopy(string id);
        
        T GetCopyId(int id);
        List<T> GetAll();
        void Add(T elem);
        void Delete(T elem);
        void Update(T elem);

    }
}