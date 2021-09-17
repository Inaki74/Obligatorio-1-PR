
namespace Common.Protocol
{
    public class VaporProtocol
    {
        public VaporProtocol()
        {
        }

        public VaporHeader CreateHeader(string requestType, int command)
        {
            VaporHeader header = new VaporHeader(requestType, command);

            return header;
        }

        // Devolver lo que recibio procesado.
        public VaporProcessedPacket Receive()
        {
            // Cuando server recibe mensaje:
                // Server sabe que:
                //  primero viene REQ/RES
                //  luego viene CMD
                //  luego viene LARGO
                //  finalmente PAYLOAD

            return null;
        }

        public void Send<T>(ReqResHeader request, int command, T data)
        {
            // Arma el paquete

            throw new System.NotImplementedException();
        }
    }
}