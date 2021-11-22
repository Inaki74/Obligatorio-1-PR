using System;

namespace Exceptions.BusinessExceptions
{
    public class UserDidntAcquireGame : BusinessException
    {
        public UserDidntAcquireGame() {}
        public UserDidntAcquireGame(string message) 
        {
            _gameOwned = message;
        }

        private string _gameOwned = "";

        public override string Message => $"User didn't own this game: {_gameOwned}!";
    }
}
