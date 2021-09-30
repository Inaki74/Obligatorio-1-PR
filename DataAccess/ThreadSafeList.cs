using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Domain.BusinessObjects;

namespace DataAccess
{
    public class ThreadSafeList<T>
    {
        private static readonly object _lock = new object();

        private readonly List<T> _internalList = new List<T>();

        public void Add(T toAdd)
        {
            lock(_lock)
            {
                _internalList.Add(toAdd);
            }
        }

        public bool Remove(T toRemove)
        {
            lock(_lock)
            {
                return _internalList.Remove(toRemove);
            }
        }

        public T Get(T toGet)
        {
            lock (_lock)
            {
                return _internalList.First(t => t.Equals(toGet));
            }  
        }

        // Hace una copia de la lista interna y la devuelve.
        // Esto hace que sea thread-safe ya que nunca vamos a estar leyendo una lista que puede ser modificada en un momento.
        // Por ende, nunca hacer Add o Remove en esta copia porque obviamente no va a afectar a la lista real.
        // Usar solo para consultar a la lista.
        public List<T> GetCopyOfInternalList()
        {
            List<T> copy = new List<T>();

            lock (_lock)
            {
                _internalList.ForEach(elem => copy.Add(elem.DeepClone<T>()));
            }   

            return copy;
        }
    }
}
