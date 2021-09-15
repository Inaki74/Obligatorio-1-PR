using Common.Commands.Interfaces;
using Common.Protocol;

namespace Common.Commands
{
    public class LoginCommand : Interfaces.ICommand
    {
        public int command {get;}

        //Build header, send to server
        public void ActionReq(IPayload payload)
        {
            VaporHeader header = new VaporHeader(HeaderConstants.Request, CommandConstants.COMMAND_LOGIN_CODE, payload.length);
            
            
        }

        //build the payload for the response
        public void ActionRes(IPayload reqPayload)
        {
            
        }
    }
}