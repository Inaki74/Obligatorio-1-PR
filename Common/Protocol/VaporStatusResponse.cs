using System;
using System.Collections.Generic;
using Common.Protocol.Interfaces;
using Domain.BusinessObjects;

namespace Common.Protocol
{
    public class VaporStatusResponse
    {
        public int Code { get; }

        public string Message { get; }

        // POSSIBLE PAYLOADS
        // SELECT GAME
        public int SelectedGameId {get; set;}
        // GET GAMES
        public List<Game> GamesList { get; set; } 

        // GET GAME REVIEWS AND SCORE
        public List<Review> ReviewsList { get; set; }
        public Review Review { get; set; }
        
        public Game Game { get; set; }
        public float GameScore { get; set; }

        public VaporStatusResponse(){}

        public VaporStatusResponse(int code, string msg)
        {
            Code = code;
            Message = msg;
        }
    }
}
