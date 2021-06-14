using System;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers
{
    internal class LastResponseParser : IResponseParser<NntpLastResponse>
    {
        private readonly ILogger log = Logger.Create<LastResponseParser>();

        public bool IsSuccessResponse(int code) => code == (int) NntpLastResponseType.ArticleExists;

        public NntpLastResponse Parse(int code, string message)
        {
            NntpLastResponseType responseType = Enum.IsDefined(typeof(NntpLastResponseType), code)
                ? (NntpLastResponseType)code
                : NntpLastResponseType.Unknown;

            if (responseType == NntpLastResponseType.Unknown)
            {
                log.LogError("Invalid response code: {Code}", code);
            }

            if (!IsSuccessResponse(code))
            {
                return new NntpLastResponse(code, message, false, responseType, 0, string.Empty);
            }

            // get stat
            string[] responseSplit = message.Split(' ');
            if (responseSplit.Length < 2)
            {
                log.LogError("Invalid response message: {Message} Expected: {{number}} {{messageid}}", message);
            }

            long.TryParse(responseSplit.Length > 0 ? responseSplit[0] : null, out long number);
            string messageId = responseSplit.Length > 1 ? responseSplit[1] : string.Empty;

            return new NntpLastResponse(code, message, true, responseType, number, messageId);
        }
    }
}
