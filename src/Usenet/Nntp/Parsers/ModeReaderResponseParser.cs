using System;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Responses;
using Usenet.Util;

namespace Usenet.Nntp.Parsers
{
    internal class ModeReaderResponseParser : IResponseParser<NntpModeReaderResponse>
    {
        private static readonly ILogger log = LibraryLogging.Create<ModeReaderResponseParser>();

        public bool IsSuccessResponse(int code)
        {
            throw new NotImplementedException();
        }

        public NntpModeReaderResponse Parse(int code, string message)
        {

            NntpModeReaderResponseType responseType = Enum.IsDefined(typeof(NntpModeReaderResponseType), code)
                ? (NntpModeReaderResponseType) code
                : NntpModeReaderResponseType.Unknown;

            bool success = responseType != NntpModeReaderResponseType.Unknown;
            if (!success)
            {
                log.LogError("Invalid response code: {Code}", code);
            }

            return new NntpModeReaderResponse(code, message, success, responseType);
        }
    }
}
