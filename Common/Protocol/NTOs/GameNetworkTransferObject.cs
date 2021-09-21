using System;
using Common.Protocol.Interfaces;

namespace Common.Protocol.NTOs
{
    public class GameNetworkTransferObject : INetworkTransferObject
    {
        public string OwnerName {get; set;}
        public string Title {get; set;}
        public string Genre {get; set;}
        public string ESRB {get; set;}
        public string Synopsis {get; set;}
        public string CoverPath {get; set;}

        public string ToCharacters()
        {
            string input = "";

            input += VaporProtocolHelper.FillNumber(OwnerName.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + OwnerName;

            input += VaporProtocolHelper.FillNumber(Title.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Title;

            input += VaporProtocolHelper.FillNumber(Genre.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Genre;

            input += VaporProtocolHelper.FillNumber(ESRB.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + ESRB;

            input += VaporProtocolHelper.FillNumber(Synopsis.Length,VaporProtocolSpecification.GAME_INPUTS_FIXED_SIZE) + Synopsis;

            return input;
        }
    }
}
