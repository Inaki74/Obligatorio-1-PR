public struct ProcessCommandPair
{
    public string Username;
    public bool Connected;

    public ProcessCommandPair(string username, bool connected)
    {
        Username = username;
        Connected = connected;
    }
}