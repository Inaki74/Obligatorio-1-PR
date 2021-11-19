
namespace Common.Protocol
{
    public class VaporProtocolSpecification
    {
        // COMMAND PROTOCOL
        public const int REQ_FIXED_SIZE = 3;
        public const int CMD_FIXED_SIZE = 2;
        public const int LENGTH_FIXED_SIZE = 4;

        // FILE PROTOCOL
        public const int COVER_CONFIRM_FIXED_SIZE = 1;
        public const int COVER_FILENAMELENGTH_FIXED_SIZE = 4;
        public const int COVER_FILESIZE_FIXED_SIZE = 8;

        // GENERAL PROTOCOL
        public const int STATUS_CODE_FIXED_SIZE = 2;
        public const int MAX_PACKET_SIZE = 32768;

        // NTO SUB-PROTOCOLS
        public const int GAME_AVERAGE_SCORE_MAXSIZE = 1;
        public const int GAME_SINGULAR_SCORE_MAXSIZE = 1;
        public const int GAME_ID_MAXSIZE = 2;
        public const int GAME_TITLE_MAXSIZE = 2;
        public const int GAME_GENRE_MAXSIZE = 2;
        public const int GAME_ESRB_MAXSIZE = 1;
        public const int GAME_SYNOPSIS_MAXSIZE = 3;
        public const int USER_NAME_MAXSIZE = 2;
        public const int REVIEW_DESCRIPTION_MAXSIZE = 3;
        public const int LIST_ELEMENTS_MAXSIZE = 1;

        public const int GAME_INPUTS_FIXED_SIZE = 4;
    }
}