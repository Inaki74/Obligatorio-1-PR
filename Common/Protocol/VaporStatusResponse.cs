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

        // GET GAMES
        public List<Game> GamesList { get; set; } 
        

        public VaporStatusResponse(){}

        public VaporStatusResponse(int code, string msg)
        {
            Code = code;
            Message = msg;
        }
    }
}
