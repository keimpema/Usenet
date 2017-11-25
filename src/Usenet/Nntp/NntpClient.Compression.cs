using System;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp
{
    public partial class NntpClient
    {
        public NntpResponse XfeatureCompressGzip(bool withTerminator) => throw new NotImplementedException();
        //connection.Command($"XFEATURE COMPRESS GZIP{(withTerminator ? " TERMINATOR" : string.Empty)}", new ResponseParser(290));

        public NntpMultiLineResponse Xzhdr(string field, NntpMessageId messageId) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZHDR {field} {messageId}", new MultiLineResponseParser(221), true);

        public NntpMultiLineResponse Xzhdr(string field, NntpArticleRange range) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZHDR {field} {RangeFormatter.Format(from, to)}", new MultiLineResponseParser(221), true);

        public NntpMultiLineResponse Xzhdr(string field) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZHDR {field}", new MultiLineResponseParser(221), true);

        public NntpMultiLineResponse Xzver(NntpArticleRange range) => throw new NotImplementedException();
        //connection.MultiLineCommand($"XZVER {RangeFormatter.Format(from, to)}", new MultiLineResponseParser(224), true);

        public NntpMultiLineResponse Xzver() => throw new NotImplementedException();
        //connection.MultiLineCommand("XZVER", new MultiLineResponseParser(224), true);

    }
}
