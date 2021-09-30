using System;
using System.Collections.Generic;
using Database;
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
            InMemoryDatabase.Instance.Games.Add(elem);
        }

        public void Delete(Game elem)
        {
            bool existed = InMemoryDatabase.Instance.Games.Remove(elem);

            if(!existed)
            {
                throw new FindGameException();
            }
        }

        public Game Get(string id)
        {
            Game dummyGame = GetCopy(id);
            Game game = InMemoryDatabase.Instance.Games.Get(dummyGame);
            return game;
        }

        public Game Get(int id)
        {
            Game dummyGame = GetCopyId(id);
            Game game = InMemoryDatabase.Instance.Games.Get(dummyGame);
            return game;
        }

        public Game GetCopy(string title)
        {
            try
            {
                Game game = InMemoryDatabase.Instance.Games.GetCopyOfInternalList().First(g => g.Title == title);
                return game;
            }
            catch(ArgumentNullException ane)
            {
                throw new FindGameException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindGameException(ioe.Message);
            }
        }

        public Game GetCopyId(int id)
        {
            try
            {
                Game game = InMemoryDatabase.Instance.Games.GetCopyOfInternalList().First(g => g.Id == id);
                return game;
            }
            catch(ArgumentNullException ane)
            {
                throw new FindGameException(ane.Message);
            }
            catch(InvalidOperationException ioe)
            {
                throw new FindGameException(ioe.Message);
            }
        }

        public List<Game> GetAll()
        {
            return InMemoryDatabase.Instance.Games.GetCopyOfInternalList();
        }

        public void Update(Game elem)
        {
            Game oldGame = GetCopyId(elem.Id);
            InMemoryDatabase.Instance.Games.Remove(oldGame);
            InMemoryDatabase.Instance.Games.Add(elem);
        }
    }
}