using Usenet.Nntp.Models;
using Usenet.Util;
using Xunit;

namespace UsenetTests.Nntp.Models
{
    public class NntpArticleTests
    {
        [Fact]
        public void ArticlesShouldBeEqual()
        {
            var article1 = new NntpArticle(0, "123@bla.nl", new MultiValueDictionary<string, string>
            {
                {"h1", "val1"},
                {"h3", "val3"},
                {"h2", "val2"},
                {"h3", "val4"},
            }, EmptyList<string>.Instance);

            var article2 = new NntpArticle(0, "123@bla.nl", new MultiValueDictionary<string, string>
            {
                {"h3", "val4"},
                {"h3", "val3"},
                {"h2", "val2"},
                {"h1", "val1"},
            }, EmptyList<string>.Instance);

            Assert.Equal(article1, article2);
        }
    }
}
