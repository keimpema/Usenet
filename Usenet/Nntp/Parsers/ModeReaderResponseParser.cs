using System;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers
{
    internal class ModeReaderResponseParser : IResponseParser<NntpModeReaderResponse>
    {
        private readonly ILogger log = Logger.Create<ModeReaderResponseParser>();

        public bool IsSuccessResponse(int code) => GetResponseType(code) != NntpModeReaderResponseType.Unknown;

        public NntpModeReaderResponse Parse(int code, string message)
        {
            NntpModeReaderResponseType responseType = GetResponseType(code);
            bool success = responseType != NntpModeReaderResponseType.Unknown;
            if (!success)
            {
                log.LogError("Invalid response code: {Code}", code);
            }
            return new NntpModeReaderResponse(code, message, success, responseType);
        }

        private static NntpModeReaderResponseType GetResponseType(int code)
        {
            return Enum.IsDefined(typeof(NntpModeReaderResponseType), code)
                ? (NntpModeReaderResponseType)code
                : NntpModeReaderResponseType.Unknown;
        }
    }
}
