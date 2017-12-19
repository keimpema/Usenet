using System.Collections.Generic;
using System.Collections.Immutable;
using Usenet.Nntp.Models;

namespace Usenet.Nntp.Writers
{
    internal class ArticleWriter
    {
        private const int maxHeaderLength = 998;

        public static void Write(INntpConnection connection, NntpArticle article)
        {
            WriteHeaders(connection, article);
            connection.WriteLine(string.Empty);
            WriteBody(connection, article);
            connection.WriteLine(".");
        }

        private static void WriteHeaders(INntpConnection connection, NntpArticle article)
        {
            WriteHeader(connection, NntpHeaders.MessageId, article.MessageId);
            WriteHeader(connection, NntpHeaders.Newsgroups, article.Groups.ToString());
            foreach (KeyValuePair<string, ImmutableHashSet<string>> header in article.Headers)
            {
                if (header.Key == NntpHeaders.MessageId ||
                    header.Key == NntpHeaders.Newsgroups)
                {
                    // skip message-id and newsgroups, they are already written
                    continue;
                }
                foreach (string value in header.Value)
                {
                    WriteHeader(connection, header.Key, value);
                }
            }
        }

        private static void WriteHeader(INntpConnection connection, string key, string val)
        {
            if (key == NntpHeaders.MessageId)
            {
                val = new NntpMessageId(val);
            }
            string line = $"{key}: {val}";
            if (line.Length <= maxHeaderLength)
            {
                connection.WriteLine(line);
                return;
            }

            // header line is too long, fold it
            connection.WriteLine(line.Substring(0, maxHeaderLength));
            line = line.Substring(maxHeaderLength);
            while (line.Length > maxHeaderLength)
            {
                connection.WriteLine("\t" + line.Substring(0, maxHeaderLength - 1));
                line = line.Substring(maxHeaderLength - 1);
            }
            connection.WriteLine("\t" + line);
        }

        private static void WriteBody(INntpConnection connection, NntpArticle article)
        {
            foreach (string line in article.Body)
            {
                if (line.Length > 0 && line[0] == '.')
                {
                    connection.WriteLine("." + line);
                    continue;
                }
                connection.WriteLine(line);
            }
        }
    }
}
