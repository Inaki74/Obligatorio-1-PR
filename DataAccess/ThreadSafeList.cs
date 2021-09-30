using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Domain.BusinessObjects;
using Exceptions.BusinessExceptions;

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
                try
                {
                    T element = _internalList.First(t => t.Equals(toGet));
                    return element;
                }
                catch(ArgumentNullException ane)
                {
                    throw new FindException();
                }
                catch(InvalidOperationException ioe)
                {
                    throw new FindException();
                }
            }  
        }
        
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
