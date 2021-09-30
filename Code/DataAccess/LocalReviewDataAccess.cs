using System;
using System.Collections.Generic;

using System.Linq;
using Database;
using Domain.BusinessObjects;
using Exceptions.BusinessExceptions;

namespace DataAccess
{
    public class LocalReviewDataAccess : IDataAccess<Review>
    {
        private static int _currentId = -1;
        public static int CurrentId
        {
            get
            {
                _currentId++;
                return _currentId;
            }
        }

        public void Add(Review elem)
        {
            InMemoryDatabase.Instance.Reviews.Add(elem);
        }

        public void Delete(Review elem)
        {
            bool existed = InMemoryDatabase.Instance.Reviews.Remove(elem);

            if(!existed)
            {
                throw new FindReviewException();
            }
        }

        public Review Get(string id)
        {
            throw new NotImplementedException();
        }

        public Review Get(int id)
        {
            throw new NotImplementedException();
        }

        public Review GetCopy(string title)
        {
            throw new NotImplementedException();
        }

        public Review GetCopyId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Review> GetAll()
        {
            return InMemoryDatabase.Instance.Reviews.GetCopyOfInternalList();
        }

        public void Update(Review elem)
        {
            throw new NotImplementedException();
        }
    }
}