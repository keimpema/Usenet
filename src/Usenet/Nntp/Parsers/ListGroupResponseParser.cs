using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers
{
    internal class ListGroupResponseParser : IMultiLineResponseParser<NntpGroupResponse>
    {
        private static readonly ILogger log = LibraryLogging.Create<GroupResponseParser>();

        public bool IsSuccessResponse(int code)
        {
            return code == 211;
        }

        public NntpGroupResponse Parse(int code, string message, IEnumerable<string> dataBlock)
        {
            if (!IsSuccessResponse(code))
            {
                return new NntpGroupResponse(
                    code, message, false,
                    new NntpGroup(string.Empty, 0, 0, 0, NntpPostingStatus.Unknown,
                        string.Empty, new int[0]));
            }

            string[] responseSplit = message.Split(' ');
            if (responseSplit.Length < 4)
            {
                log.LogWarning("Invalid response message: {Message} Expected: {{count}} {{low}} {{high}} {{group}}", message);
            }

            int.TryParse(responseSplit.Length > 0 ? responseSplit[0] : null, out int articleCount);
            int.TryParse(responseSplit.Length > 1 ? responseSplit[1] : null, out int lowWaterMark);
            int.TryParse(responseSplit.Length > 2 ? responseSplit[2] : null, out int highWaterMark);
            string name = responseSplit.Length > 3 ? responseSplit[3] : string.Empty;

            return new NntpGroupResponse(
                code, message, true,
                new NntpGroup(name, articleCount, lowWaterMark, highWaterMark, NntpPostingStatus.Unknown, string.Empty,
                    EnumerateArticleNumbers(dataBlock)));
        }

        private static IEnumerable<int> EnumerateArticleNumbers(IEnumerable<string> dataBlock)
        {
            if (dataBlock == null)
            {
                yield break;
            }
            foreach (string line in dataBlock)
            {
                if (!int.TryParse(line, out int number))
                {
                    continue;
                }
                yield return number;
            }
        }
    }
}
