using System;

namespace ServerApplication
{
    public struct ClientCommandExecutionStatus
    {
        public bool ExecutingCommand { get; set; }
        public int ConnectionId { get; set; }

        public ClientCommandExecutionStatus(int id)
        {
            ExecutingCommand = false;
            ConnectionId = id;
        }
    }
}
