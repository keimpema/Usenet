using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;

namespace Usenet.Nntp.Parsers
{
    internal class GroupOriginsResponseParser : IMultiLineResponseParser<NntpGroupOriginsResponse>
    {
        private static readonly ILogger log = LibraryLogging.Create<GroupOriginsResponseParser>();

        public bool IsSuccessResponse(int code)
        {
            return code == 215;
        }

        public NntpGroupOriginsResponse Parse(int code, string message, IEnumerable<string> dataBlock)
        {
            if (!IsSuccessResponse(code) || dataBlock == null)
            {
                return new NntpGroupOriginsResponse(code, message, false, new NntpGroupOrigin[0]);
            }
            
            return new NntpGroupOriginsResponse(code, message, true, EnumerateGroupOrigins(dataBlock));
        }

        private static IEnumerable<NntpGroupOrigin> EnumerateGroupOrigins(IEnumerable<string> dataBlock)
        {
            if (dataBlock == null)
            {
                yield break;
            }

            foreach (string line in dataBlock)
            {
                string[] lineSplit = line.Split(' ');
                if (lineSplit.Length < 3)
                {
                    log.LogError("Invalid newsgroup origin line: {Line} Expected: {{group}} {{timestamp}} {{createdby}}", line);
                    continue;
                }

                long.TryParse(lineSplit[1], out long createdAtTimestamp);

                yield return new NntpGroupOrigin(
                    lineSplit[0],
                    DateTimeOffset.FromUnixTimeSeconds(createdAtTimestamp),
                    lineSplit[2]);
            }

        }
    }
}
