using System;
using System.Collections.Generic;

using System.Linq;
using Domain;

namespace DataAccess
{
    public class LocalGameDataAccess : IDataAccess<Game>
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

        public void Add(Game elem)
        {
            Database.Instance.Games.Add(elem);
        }

        public void Delete(Game elem)
        {
            throw new NotImplementedException();
        }

        public Game Get(int id)
        {
            throw new NotImplementedException();
        }

        public Game Get(string title)
        {
            Game game = Database.Instance.Games.GetInternalList().FirstOrDefault(g => g.Title == title);
            return game;
        }

        public List<Game> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Game elem)
        {
            throw new NotImplementedException();
        }
    }
}