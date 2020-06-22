using System;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers
{
    internal class NextResponseParser : IResponseParser<NntpNextResponse>
    {
        private static readonly ILogger log = LibraryLogging.Create<NextResponseParser>();

        public bool IsSuccessResponse(int code) => code == (int) NntpNextResponseType.ArticleExists;

        public NntpNextResponse Parse(int code, string message)
        {
            NntpNextResponseType responseType = Enum.IsDefined(typeof(NntpNextResponseType), code)
                ? (NntpNextResponseType) code
                : NntpNextResponseType.Unknown;

            if (responseType == NntpNextResponseType.Unknown)
            {
                log.LogError("Invalid response code: {Code}", code);
            }

            if (!IsSuccessResponse(code))
            {
                return new NntpNextResponse(code, message, false, responseType, 0, string.Empty);
            }

            // get stat
            string[] responseSplit = message.Split(' ');
            if (responseSplit.Length < 2)
            {
                log.LogError("Invalid response message: {Message} Expected: {{number}} {{messageid}}", message);
            }

            long.TryParse(responseSplit.Length > 0 ? responseSplit[0] : null, out long number);
            string messageId = responseSplit.Length > 1 ? responseSplit[1] : string.Empty;

            return new NntpNextResponse(code, message, true, responseType, number, messageId);
        }
    }
}
