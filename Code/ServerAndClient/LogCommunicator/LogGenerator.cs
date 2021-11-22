using System;
using Models;
using LogCommunicatorInterfaces;
using Business;
using BusinessInterfaces;

namespace LogCommunicator
{
    public class LogGenerator : ILogGenerator
    {
        private const string NAME_IF_GAMENAME_EMPTY = "GameEmpty";
        private readonly IGameLogic _gameLogic;
        public LogGenerator()
        {
            _gameLogic = new GameLogic();
        }

        public LogModel CreateLog(string username, int gameid, bool isError, string message)
        {
            LogModel model = new LogModel();

            model.Date = DateTime.Now;
            model.Username = username;
            model.Description = message;

            if(gameid == -1)
            {
                model.Gamename = NAME_IF_GAMENAME_EMPTY;
            }
            else
            {
                model.Gamename = _gameLogic.GetGame(gameid).Title;
            }

            if(!isError)
            {
                model.LogType = LogType.INFO;
            }
            else
            {
                model.LogType = LogType.ERROR;
            }

            return model;
        }
    }
}