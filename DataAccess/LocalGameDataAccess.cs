using System;
using System.Collections.Generic;

using System.Linq;
using Domain.BusinessObjects;
using Exceptions.BusinessExceptions;

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
            bool existed = Database.Instance.Games.Remove(elem);

            if(!existed)
            {
                throw new FindGameException();
            }
        }

        public Game Get(string id)
        {
            Game dummyGame = GetCopy(id);
            Game game = Database.Instance.Games.Get(dummyGame);
            return game;
        }

        public Game Get(int id)
        {
            Game dummyGame = GetCopyId(id);
            Game game = Database.Instance.Games.Get(dummyGame);
            return game;
        }

        public Game GetCopy(string title)
        {
            Game game = Database.Instance.Games.GetCopyOfInternalList().FirstOrDefault(g => g.Title == title);
            return game;
        }

        public Game GetCopyId(int id)
        {
            try
            {
                Game game = Database.Instance.Games.GetCopyOfInternalList().First(g => g.Id == id);
                return game;
            }
            catch(ArgumentNullException ane)
            {
                throw new FindGameException();
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindGameException();
            }
        }

        public List<Game> GetAll()
        {
            return Database.Instance.Games.GetCopyOfInternalList();
        }

        public void Update(Game elem)
        {
            Game oldGame = GetCopyId(elem.Id);
            Database.Instance.Games.Remove(oldGame);
            Database.Instance.Games.Add(elem);
        }
    }
}