using System;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers
{
    internal class StatResponseParser : IResponseParser<NntpStatResponse>
    {
        private static readonly ILogger log = LibraryLogging.Create<StatResponseParser>();

        public bool IsSuccessResponse(int code) => code == (int) NntpStatResponseType.ArticleExists;

        public NntpStatResponse Parse(int code, string message)
        {
            NntpStatResponseType responseType = Enum.IsDefined(typeof(NntpStatResponseType), code)
                ? (NntpStatResponseType) code
                : NntpStatResponseType.Unknown;

            if (responseType == NntpStatResponseType.Unknown)
            {
                log.LogError("Invalid response code: {Code}", code);
            }

            if (!IsSuccessResponse(code))
            {
                return new NntpStatResponse(code, message, false, responseType, 0, string.Empty);
            }

            // get stat
            string[] responseSplit = message.Split(' ');
            if (responseSplit.Length < 2)
            {
                log.LogError("Invalid response message: {Message} Expected: {{number}} {{messageid}}", message);
            }

            long.TryParse(responseSplit.Length > 0 ? responseSplit[0] : null, out long number);
            string messageId = responseSplit.Length > 1 ? responseSplit[1] : string.Empty;

            return new NntpStatResponse(code, message, true, responseType, number, messageId);
        }
    }
}
