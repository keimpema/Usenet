using System;
using System.Threading.Tasks;
using Usenet.Nntp;
using System.Linq;
using System.Threading;

namespace Common
{
    //Hacky and temporary helper class to try to get a better view of the lifetime of NNTP connections
    static public class NntpExtensions
    {
        static int clientCount;
        public static int HowManyClients => clientCount;
        public static void IncrementClientCount()
        {
            Interlocked.Increment(ref clientCount);
        }
        public static void DecramentClientCount()
        {
            Interlocked.Decrement(ref clientCount);
        }

        private static Func<ConnectionInfoAndCredentials> callback;
        public static void SetCredGetter(Func<ConnectionInfoAndCredentials> func)
        {
            callback = func;
        }

        public static async Task<NntpClient> GetClient()
        {
            var creds = await GetCreds();
            return await GetClient(creds.ServerName, creds.Port, creds.Username, creds.Password);
        }

        private static async Task<NntpClient> GetClient(string serverName, int port, string username, string password)
        {
            var client = new NntpClient(new NntpConnection());
            var connectResult = await client.ConnectAsync(serverName, port, true);
            bool authResult;
            int attempts = 0;
            do
            {
                attempts++;
                authResult = client.Authenticate(username, password);
            } while (authResult == false && attempts <= 5);

            if( authResult && attempts != 1)
            {
                Console.WriteLine("Retry successful");
            }

            if( authResult == false)
            {
                //System.Diagnostics.Debugger.Break();
            }

            return client;
        }

        private static async Task<ConnectionInfoAndCredentials> GetCreds()
        {
            if (callback != null)
            {
                return callback();
            }
            await Task.Delay(0); //To make compiler happy.
            return GetCredsFromEnvironment(); //TODO: Return to getting creds from environment or secrets
            //return await GetCredsFromKV() ?? GetDefaultCreds();
            //log.LogInformation("KV read successful");
        }

        private static ConnectionInfoAndCredentials GetCredsFromEnvironment()
        {
            string serverName = Environment.GetEnvironmentVariable("UsenetServerName");
            int portNumber = int.Parse(Environment.GetEnvironmentVariable("UsenetPortNumber"));
            string username = Environment.GetEnvironmentVariable("UsenetUsername");
            string password = Environment.GetEnvironmentVariable("UsenetPassword");

            return new ConnectionInfoAndCredentials(serverName, portNumber, username, password);
        }

        public class ConnectionInfoAndCredentials
        {
            public ConnectionInfoAndCredentials(string serverName, int port, string username, string password)
            {
                this.ServerName = serverName;
                this.Port = port;
                this.Username = username;
                this.Password = password;
            }

            public string ServerName { get; set; }
            public int Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
