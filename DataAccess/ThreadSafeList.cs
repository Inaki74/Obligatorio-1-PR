using System;
using System.Collections;
using System.Collections.Generic;

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

        public void Remove(T toRemove)
        {
            lock(_lock)
            {
                _internalList.Remove(toRemove);
            }
        }

        public List<T> GetInternalList()
        {
            return _internalList;
        }
    }
}
