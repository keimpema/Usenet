namespace TestClient.Configuration
{
    public class Server
    {
        public string Name { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public bool NeedsAuthentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
