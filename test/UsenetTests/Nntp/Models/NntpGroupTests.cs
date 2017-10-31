using Newtonsoft.Json;
using Usenet.Nntp.Models;
using Xunit;

namespace UsenetTests.Nntp.Models
{
    public class NntpGroupTests
    {
        [Theory]
        [InlineData("test", 10, 2, 11, NntpPostingStatus.PostingPermitted, null, null)]
        [InlineData("alt.rfc-writers.recovery", 0, 1, 4, NntpPostingStatus.PostingPermitted, "", new int[0])]
        [InlineData("tx.natives.recovery", 0, 56, 89, NntpPostingStatus.PostingPermitted, "", new int[0])]
        [InlineData("misc.test", 1234, 3000234, 3002322, NntpPostingStatus.PostingPermitted, "", new int[0])]
        [InlineData("rec.food.drink.tea", 3, 51, 100, NntpPostingStatus.PostingPermitted, "", new int[0])]
        public void NntpGroupShouldBeSerializedAndDeserializedCorrectly(
            string name, 
            int articleCount, 
            int lowWaterMark, 
            int highWaterMark,
            NntpPostingStatus postingStatus,
            string otherGroup, 
            int[] articleNumbers)
        {
            var expected = new NntpGroup(
                name,
                articleCount,
                lowWaterMark,
                highWaterMark,
                postingStatus,
                otherGroup,
                articleNumbers);

            string json = JsonConvert.SerializeObject(expected);
            var actual = JsonConvert.DeserializeObject<NntpGroup>(json);
            Assert.Equal(expected, actual);
        }
    }
}
