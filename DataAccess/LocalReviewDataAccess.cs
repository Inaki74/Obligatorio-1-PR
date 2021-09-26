using System;
using System.Collections.Generic;

using System.Linq;
using Domain.BusinessObjects;

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
            Database.Instance.Reviews.Add(elem);
        }

        public void Delete(Review elem)
        {
            Database.Instance.Reviews.Remove(elem);
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
            //Review game = Database.Instance.Reviews.GetInternalList().FirstOrDefault(g => g.Title == title);
            //return game;

            throw new NotImplementedException();
        }

        public Review GetCopyId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Review> GetAll()
        {
            return Database.Instance.Reviews.GetCopyOfInternalList();
        }

        public void Update(Review elem)
        {
            throw new NotImplementedException();
        }
    }
}