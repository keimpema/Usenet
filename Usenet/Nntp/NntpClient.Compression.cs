using System;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp
{
    public partial class NntpClient
    {
        /// <summary>
        /// Needs a <a href="https://gist.github.com/keimpema/ec962384d5fe3eb7a5f5030353ba9e2b">decompressing connection</a>.
        /// </summary>
        /// <param name="withTerminator"></param>
        /// <returns></returns>
        public NntpResponse XfeatureCompressGzip(bool withTerminator) => throw new NotImplementedException();
        //connection.Command($"XFEATURE COMPRESS GZIP{(withTerminator ? " TERMINATOR" : string.Empty)}", new ResponseParser(290));

        /// <summary>
        /// Needs a <a href="https://gist.github.com/keimpema/ec962384d5fe3eb7a5f5030353ba9e2b">decompressing connection</a>.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public NntpMultiLineResponse Xzhdr(string field, NntpMessageId messageId) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZHDR {field} {messageId}", new MultiLineResponseParser(221), true);

        /// <summary>
        /// Needs a <a href="https://gist.github.com/keimpema/ec962384d5fe3eb7a5f5030353ba9e2b">decompressing connection</a>.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public NntpMultiLineResponse Xzhdr(string field, NntpArticleRange range) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZHDR {field} {RangeFormatter.Format(from, to)}", new MultiLineResponseParser(221), true);

        /// <summary>
        /// Needs a <a href="https://gist.github.com/keimpema/ec962384d5fe3eb7a5f5030353ba9e2b">decompressing connection</a>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public NntpMultiLineResponse Xzhdr(string field) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZHDR {field}", new MultiLineResponseParser(221), true);

        /// <summary>
        /// Needs a <a href="https://gist.github.com/keimpema/ec962384d5fe3eb7a5f5030353ba9e2b">decompressing connection</a>.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public NntpMultiLineResponse Xzver(NntpArticleRange range) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZVER {RangeFormatter.Format(from, to)}", new MultiLineResponseParser(224), true);

        /// <summary>
        /// Needs a <a href="https://gist.github.com/keimpema/ec962384d5fe3eb7a5f5030353ba9e2b">decompressing connection</a>.
        /// </summary>
        /// <returns></returns>
        public NntpMultiLineResponse Xzver() => throw new NotImplementedException();
        //connection.MultiLineCommand("XZVER", new MultiLineResponseParser(224), true);

    }
}
