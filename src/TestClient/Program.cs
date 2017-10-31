using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using TestClient.Configuration;
using TestClient.Util;
using Usenet;
using Usenet.Nntp;
using Usenet.Nntp.Builders;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;
using Usenet.Nzb;
using Usenet.Util;
using Usenet.Yenc;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace TestClient
{
    public class Program
    {
        private static readonly ILogger log  = ApplicationLogging.Create<Program>();
        private static IConfigurationRoot config;

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            // init logging
            LibraryLogging.Factory = ApplicationLogging.Factory;
            log.LogInformation("Program started with {Arguments}.", (object)args);

            // init config
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            config = configBuilder.Build();
            
            // setup DI
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<Nntp>(config.GetSection(nameof(Nntp)))
                .AddSingleton(ApplicationLogging.Factory)
                .BuildServiceProvider();

            // read config
            Nntp nntpOptions = serviceProvider.GetService<IOptions<Nntp>>().Value;
            Server server;

            bool streaming = args.Length > 0 && args[0] == "stream";
            log.LogInformation("Decode streaming: {streaming}", streaming);

            while ((server = ChooseServer(nntpOptions.Servers)) != null)
            {
                try
                {
                    NntpClient client = await ConnectAsync(server);
                    if (client == null)
                    {
                        continue;
                    }

                    TestDownloadNzb(client, @"Testfile.nzb");
                    //TestDownloadNzbStreaming(client, @"Testfile.nzb");

                    //TestDate(client);
                    //TestCapabilities(client);
                    //TestHelp(client);
                    //TestListActiveTimes(client);

                    //NntpGroup group = client.Group("alt.test.clienttest").Group;
                    //ShowGroup(group);

                    //TestArticle(client, @group);
                    //TestPost(client);

                    //TestListCounts(client);
                    //TestListOverviewFormat(client);
                    //TestListNewsGroups(client);
                    //TestListDistribPats(client);
                    //TestListHeaders(client);

                    //TestXover(client, group);
                    //TestXhdr(client, group);

                    //TestListDistributions(client);

                    //sw.Restart();
                    //TestListActive(client);
                    //Console.WriteLine(sw.Elapsed);

                    //TestNewGroups(client);

                    //TestDecompression(client, @group);

                    // quit - close connection
                    client.Quit();
                }
                catch (Exception exception)
                {
                    log.LogError("Exception: {exception}", exception);
                }
            }
        }

        private static void TestDownloadNzb(NntpClient client, string nzbFileName)
        {
            string fullPath = Path.Combine(nzbFileName);
            string nzbData = File.ReadAllText(fullPath, UsenetEncoding.Default);
            NzbDocument nzbDocument = NzbParser.Parse(nzbData);

            string downloadDir = Path.Combine("downloads", nzbFileName);
            Directory.CreateDirectory(downloadDir);

            log.LogInformation("Downloading nzb {nzbFileName}", nzbFileName);
            foreach (NzbFile file in nzbDocument.Files)
            {
                log.LogInformation("Downloading file {subject}", file.Subject);
                foreach (NzbSegment segment in file.Segments)
                {
                    log.LogInformation("Downloading article {messageId}", segment.MessageId);
                    NntpArticleResponse response = client.Article(segment.MessageId);
                    YencArticle decodedArticle = YencArticleDecoder.Decode(response.Article.Body);
                    YencHeader header = decodedArticle.Header;

                    string fileName = Path.Combine(downloadDir, header.FileName);
                    if (!File.Exists(fileName))
                    {
                        log.LogInformation("Creating file {fileName}", fileName);
                        // create file and pre-allocate disk space for it
                        using (FileStream stream = File.Create(fileName))
                        {
                            stream.SetLength(decodedArticle.Header.FileSize);
                        }
                    }

                    log.LogInformation("Writing {size} bytes to file {fileName} at offset {offset}",
                        header.FileSize, fileName, header.PartOffset);

                    using (FileStream stream = File.Open(
                        fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        stream.Seek(header.PartOffset, SeekOrigin.Begin);
                        stream.Write(decodedArticle.Data, 0, decodedArticle.Data.Length);
                    }
                }
            }
        }

        private static void TestDownloadNzbStreaming(NntpClient client, string nzbFileName)
        {
            string fullPath = Path.Combine(nzbFileName);
            string nzbData = File.ReadAllText(fullPath, UsenetEncoding.Default);
            NzbDocument nzbDocument = NzbParser.Parse(nzbData);

            string downloadDir = Path.Combine("downloads", nzbFileName);
            Directory.CreateDirectory(downloadDir);

            var sw = new Stopwatch();
            sw.Restart();

            log.LogInformation("Downloading nzb {nzbFileName}", nzbFileName);
            foreach (NzbFile file in nzbDocument.Files)
            {
                log.LogInformation("Downloading file {subject}", file.Subject);
                foreach (NzbSegment segment in file.Segments)
                {
                    log.LogInformation("Downloading article {messageId}", segment.MessageId);
                    NntpArticleResponse response = client.Article(segment.MessageId);
                    using (YencStream yencStream = YencStreamDecoder.Decode(response.Article.Body))
                    {
                        YencHeader header = yencStream.Header;

                        string fileName = Path.Combine(downloadDir, header.FileName);
                        if (!File.Exists(fileName))
                        {
                            log.LogInformation("Creating file {fileName}", fileName);
                            // create file and pre-allocate disk space for it
                            using (FileStream stream = File.Create(fileName))
                            {
                                stream.SetLength(header.FileSize);
                            }
                        }

                        log.LogInformation("Writing {size} bytes to file {fileName} at offset {offset}",
                            header.PartSize, fileName, header.PartOffset);

                        using (FileStream stream = File.Open(
                            fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                        {
                            stream.Seek(header.PartOffset, SeekOrigin.Begin);
                            yencStream.CopyTo(stream);
                        }
                    }
                }
            }
            log.LogInformation("Nzb downloaded in {elapsed}", sw.Elapsed);
        }

        private static void TestNewGroups(NntpClient client)
        {
            // get new groups
            foreach (NntpGroup g in client.NewGroups(DateTimeOffset.Now.AddYears(-10)).Groups)
            {
                Console.WriteLine($"{g.Name} {g.LowWaterMark} {g.HighWaterMark} {g.ArticleCount}");
            }
        }

        private static void TestListActive(NntpClient client)
        {
            Console.WriteLine("Number of groups: " + client.ListActive().Groups.Count());

            //// list active
            //foreach (NntpGroup g in client.ListActive2().Groups)
            //{
            //    Console.WriteLine($"{g.Name} {g.LowWaterMark} {g.HighWaterMark} {g.ArticleCount}");
            //}

            //// list active with wildmat
            //foreach (NntpGroup g in client.ListActive("*.recovery").Groups)
            //{
            //    Console.WriteLine($"{g.Name} {g.LowWaterMark} {g.HighWaterMark} {g.ArticleCount}");
            //}
        }

        private static void TestListDistributions(NntpClient client)
        {
            // list distributions
            ShowLines(client.ListDistributions().Lines);
        }

        private static void TestListActiveTimes(NntpClient client)
        {
            // list active.times with wildmat
            foreach (NntpGroupOrigin groupOrigin in client.ListActiveTimes("*.recovery").GroupOrigins)
            {
                Console.WriteLine($"{groupOrigin.Name} {groupOrigin.CreatedAt} {groupOrigin.CreatedBy}");
            }

            // list active.times
            foreach (NntpGroupOrigin groupOrigin in client.ListActiveTimes().GroupOrigins)
            {
                Console.WriteLine($"{groupOrigin.Name} {groupOrigin.CreatedAt} {groupOrigin.CreatedBy}");
            }
        }

        private static void TestArticle(NntpClient client, NntpGroup @group)
        {
            // get article by message id
            ShowArticle(client.Article("<53637872-51a5-434f-87ac-30425ca9cb2b@googlegroups.com>").Article);

            // select group alt.test.clienttest with article numbers            
            ShowGroup(client.ListGroup("alt.test.clienttest").Group);

            // get first article in group
            ShowArticle(client.Article().Article);

            // get last article in group
            ShowArticle(client.Article(@group.HighWaterMark).Article);
        }

        private static void TestDecompression(NntpClient client, NntpGroup @group)
        {
            // enable gzip
            client.XfeatureCompressGzip(false);

            // xzhdr
            ShowLines(client.Xzhdr("Subject", @group.LowWaterMark, @group.HighWaterMark).Lines);

            // xzver
            ShowLines(client.Xzver(@group.HighWaterMark, @group.HighWaterMark).Lines);

            //TestListDistributions(client);
            TestListActive(client);
            //TestNewGroups(client);

        }

        private static void TestXhdr(NntpClient client, NntpGroup group)
        {
            // xhdr
            ShowLines(client.Xhdr("Subject", NntpArticleRange.Range(group.HighWaterMark, group.HighWaterMark)).Lines);
        }

        private static void TestXover(NntpClient client, NntpGroup group)
        {
            // xover
            ShowLines(client.Xover(NntpArticleRange.Range(group.HighWaterMark, group.HighWaterMark)).Lines);
        }

        private static void TestListHeaders(NntpClient client)
        {
            // list headers
            ShowLines(client.ListHeaders().Lines);
        }

        private static void TestListDistribPats(NntpClient client)
        {
            // list distrib.pats
            ShowLines(client.ListDistribPats().Lines);
        }

        private static void TestListNewsGroups(NntpClient client)
        {
            // list newsgroups
            ShowLines(client.ListNewsgroups("*.recovery").Lines);
        }

        private static void TestListOverviewFormat(NntpClient client)
        {
            // list overview.fmt
            ShowLines(client.ListOverviewFormat().Lines);
        }

        private static void TestListCounts(NntpClient client)
        {
            // list counts
            foreach (NntpGroup g in client.ListCounts("*.recovery").Groups)
            {
                Console.WriteLine($"{g.Name} {g.LowWaterMark} {g.HighWaterMark} {g.ArticleCount} {g.PostingStatus}");
            }
        }

        private static async Task<NntpClient> ConnectAsync(Server server)
        {
            // connect to nntp server
            var client = new NntpClient(new NntpConnection());
            //var client = new NntpClient(new Usenet.Nntp.Experimental.NntpConnection());

            if (!await client.ConnectAsync(server.Hostname, server.Port, server.UseSsl))
            {
                Console.WriteLine("Failed to connect.");
                return null;
            }

            // authenticate
            if (!server.NeedsAuthentication || Authenticate(client, server.Username, server.Password))
            {
                return client;
            }
            Console.WriteLine("Login failed, bye.");
            client.Quit();
            return null;
        }

        private static void TestHelp(NntpClient client)
        {
            // get help
            ShowLines(client.Help().Lines);
        }

        private static void TestCapabilities(NntpClient client)
        {
            // get capabilities
            ShowLines(client.Capabilities().Lines);
        }

        private static void TestDate(NntpClient client)
        {
            // get server date
            Console.WriteLine($"Server date: {client.Date()}");
        }

        private static string TestPost(NntpClient client)
        {
            string messageId = $"cucumber_{Guid.NewGuid()}@hhh.net";

            NntpArticle newArticle = new NntpArticleBuilder()
                .SetMessageId(messageId)
                .SetFrom("Superuser <super@hhh.net>")
                .SetSubject(messageId)
                .AddGroup("alt.test.clienttest")
                .AddGroup("alt.test")
                .AddLine("This is a message with id " + messageId)
                .AddLine("bla bla bla")
                .Build();

            // post
            client.Post(newArticle);

            // read back and show
            ShowArticle(client.Article(messageId).Article);

            return messageId;
        }

        private static Server ChooseServer(Server[] servers)
        {
            for (var i = 0; i < servers.Length; i++)
            {                
                Console.WriteLine($"{i+1}. {servers[i].Name} - {servers[i].Hostname}:{servers[i].Port}");
            }
            while (true)
            {
                Console.Write("Choose server or quit: ");
                string line = Console.ReadLine();
                if (line.StartsWith("q", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                if (!int.TryParse(line, out int choice))
                {
                    continue;
                }
                if (choice >= 1 && choice <= servers.Length)
                {
                    return servers[choice - 1];
                }
            }
        }

        private static bool Authenticate(NntpClient client, string username, string password)
        {
            // try to authenticate with username and password from options
            if (!string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(password) &&
                client.Authenticate(username, password))
            {
                return true;
            }

            // get password and username if not provided in options
            for (var tries = 3; tries > 0; tries--)
            {
                // get username 
                string defaultUsername = string.IsNullOrWhiteSpace(username) ? null : $"[default = {username}]";
                Console.Write($"Username{defaultUsername}: ");
                string userName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    username = userName;
                }
                if (string.IsNullOrWhiteSpace(username))
                {
                    return false;
                }

                // get password
                Console.Write("Password: ");
                password = ConsoleHelper.ReadPassword();

                // authenticate
                if (client.Authenticate(username, password))
                {
                    return true;
                }

                Console.WriteLine("Invalid credentials.");
            }
            return false;
        }

        private static void ShowGroup(NntpGroup group)
        {
            Console.WriteLine($"Group: {group.Name}");
            Console.WriteLine($"Article count: {group.ArticleCount}");
            Console.WriteLine($"Low water mark: {group.LowWaterMark}");
            Console.WriteLine($"High water mark: {group.HighWaterMark}");
            Console.WriteLine($"Posting status: {group.PostingStatus}");
            foreach (int articleNumber in group.ArticleNumbers)
            {
                Console.Write(articleNumber);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        private static void ShowArticle(NntpArticle article)
        {
            if (article == null)
            {
                return;
            }

            Console.WriteLine($"Article number: {article.Number}, message id: {article.MessageId}");
            foreach (KeyValuePair<string, ICollection<string>> header in article.Headers)
            {
                foreach (string value in header.Value)
                {
                    Console.WriteLine($"{header.Key}: {value}");
                }
            }
            foreach (string str in article.Body)
            {
                Console.WriteLine(str);
            }
        }

        private static void ShowLines(IEnumerable<string> lines)
        {
            if (lines == null)
            {
                return;
            }
            foreach (string capability in lines)
            {
                Console.WriteLine(capability);
            }
        }
    }
}