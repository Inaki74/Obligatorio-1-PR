using System;

namespace Exceptions.BusinessExceptions
{
    public class UserAlreadyAcquiredGame : BusinessException
    {
        public UserAlreadyAcquiredGame() {}
        public UserAlreadyAcquiredGame(string message) 
        {
            _gameOwned = message;
        }

        private string _gameOwned = "";

        public override string Message => $"User already owns this game: {_gameOwned}!";
    }
}
