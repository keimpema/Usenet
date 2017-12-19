using Usenet.Nntp.Models;

namespace Usenet.Nntp.Parsers
{
    internal static class PostingStatusParser
    {
        public static NntpPostingStatus Parse(string input, out string otherGroup)
        {
            otherGroup = string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                return NntpPostingStatus.Unknown;
            }
            switch (input[0])
            {
                case 'y': return NntpPostingStatus.PostingPermitted;
                case 'n': return NntpPostingStatus.PostingNotPermitted;
                case 'm': return NntpPostingStatus.PostingsWillBeReviewed;
                case 'x': return NntpPostingStatus.ArticlesFromPeersNotPermitted;
                case 'j': return NntpPostingStatus.OnlyArticlesFromPeersPermittedNotFiledLocally;

                case '=':
                    {
                        otherGroup = input.Substring(1);
                        return NntpPostingStatus.OnlyArticlesFromPeersPermittedFiledLocally;
                    }

                default: return NntpPostingStatus.Unknown;
            }
        }
    }
}
