using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Usenet.Exceptions;
using Usenet.Logging;
using Usenet.Nntp.Parsers;
using Usenet.Nntp.Responses;
using Usenet.Util;

namespace Usenet.Nntp
{
    /// <summary>
    /// A standard implementation of an NNTP connection.
    /// Based on Kristian Hellang's NntpLib.Net project https://github.com/khellang/NntpLib.Net.
    /// </summary>
    /// <remarks>This implementation of the <see cref="INntpConnection"/> interface does support SSL encryption but
    /// does not support compressed multi-line results.</remarks>
    public class NntpConnection : INntpConnection
    {
        private static readonly ILog log = LogProvider.For<NntpConnection>();

        private readonly TcpClient client = new TcpClient();
        private StreamWriter writer;
        private NntpStreamReader reader;

        /// <inheritdoc/>
        public CountingStream Stream { get; private set; }

        /// <inheritdoc/>
        public async Task<TResponse> ConnectAsync<TResponse>(string hostname, int port, bool useSsl, IResponseParser<TResponse> parser)
        {
            log.Info("Connecting: {hostname} {port} (Use SSl = {useSsl})", hostname, port, useSsl);
            await client.ConnectAsync(hostname, port);
            Stream = await GetStreamAsync(hostname, useSsl);
            writer = new StreamWriter(Stream, UsenetEncoding.Default) { AutoFlush = true };
            reader = new NntpStreamReader(Stream, UsenetEncoding.Default);
            return GetResponse(parser);
        }

        /// <inheritdoc/>
        public TResponse Command<TResponse>(string command, IResponseParser<TResponse> parser)
        {
            ThrowIfNotConnected();
            log.Info("Sending command: {Command}",command.StartsWith("AUTHINFO PASS", StringComparison.Ordinal) ? "AUTHINFO PASS [omitted]" : command);
            writer.WriteLine(command);
            return GetResponse(parser);
        }

        /// <inheritdoc/>
        public TResponse MultiLineCommand<TResponse>(string command, IMultiLineResponseParser<TResponse> parser) //, bool decompress = false)
        {
            NntpResponse response = Command(command, new ResponseParser());

            IEnumerable<string> dataBlock = parser.IsSuccessResponse(response.Code)
                ? ReadMultiLineDataBlock()
                : new string[0];

            return parser.Parse(response.Code, response.Message, dataBlock);
        }

        /// <inheritdoc/>
        public TResponse GetResponse<TResponse>(IResponseParser<TResponse> parser)
        {
            string responseText = reader.ReadLine();
            log.Info("Response received: {Response}", responseText);
            
            if (responseText == null)
            {
                throw new NntpException("Received no response.");
            }
            if (responseText.Length < 3 || !int.TryParse(responseText.Substring(0, 3), out int code))
            {
                throw new NntpException("Received invalid response.");
            }
            return parser.Parse(code, responseText.Substring(3).Trim());
        }

        /// <inheritdoc/>
        public void WriteLine(string line)
        {
            ThrowIfNotConnected();
            writer.WriteLine(line);
        }

        private void ThrowIfNotConnected()
        {
            if (!client.Connected)
            {
                throw new NntpException("Client not connected.");
            }
        }

        private async Task<CountingStream> GetStreamAsync(string hostname, bool useSsl)
        {
            NetworkStream stream = client.GetStream();
            if (!useSsl)
            {
                return new CountingStream(stream);
            }
            var sslStream = new SslStream(stream);
            await sslStream.AuthenticateAsClientAsync(hostname);
            return new CountingStream(sslStream);
        }

        private IEnumerable<string> ReadMultiLineDataBlock()
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            client?.Dispose();
            writer?.Dispose();
            reader?.Dispose();
        }
    }
}
