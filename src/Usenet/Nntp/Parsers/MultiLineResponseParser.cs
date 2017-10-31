using System.Collections.Generic;
using System.Linq;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers
{
    internal class MultiLineResponseParser : IMultiLineResponseParser<NntpMultiLineResponse>
    {
        private readonly int[] successCodes;

        public MultiLineResponseParser(params int[] successCodes)
        {
            this.successCodes = successCodes ?? new int[0];
        }

        public bool IsSuccessResponse(int code)
        {
            return successCodes.Contains(code);
        }

        public NntpMultiLineResponse Parse(int code, string message, IEnumerable<string> dataBlock)
        {
            return new NntpMultiLineResponse(code, message, IsSuccessResponse(code), dataBlock);
        }
    }
}
