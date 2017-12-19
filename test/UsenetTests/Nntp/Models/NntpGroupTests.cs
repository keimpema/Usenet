using System.Collections.Generic;
using Lib;
using Newtonsoft.Json;
using Usenet.Nntp.Models;
using Xunit;

namespace UsenetTests.Nntp.Models
{
    public class NntpGroupTests
    {
        [Theory]
        [InlineData("test", 10, 2, 11, NntpPostingStatus.PostingPermitted, null, null)]
        [InlineData("alt.rfc-writers.recovery", 0, 1, 4, NntpPostingStatus.PostingPermitted, "", new long[0])]
        [InlineData("tx.natives.recovery", 0, 56, 89, NntpPostingStatus.PostingPermitted, "", new long[0])]
        [InlineData("misc.test", 1234, 3000234, 3002322, NntpPostingStatus.PostingPermitted, "", new long[0])]
        [InlineData("rec.food.drink.tea", 3, 51, 100, NntpPostingStatus.PostingPermitted, "", new long[0])]
        public void SerializedInstanceShouldBeDeserializedCorrectly(
            string name, 
            long articleCount, 
            long lowWaterMark,
            long highWaterMark,
            NntpPostingStatus postingStatus,
            string otherGroup,
            long[] articleNumbers)
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

        public static IEnumerable<object[]> EqualsWithSameValues = new[]
        {
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, null, new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, null, new long[]{1,2,3})),
            },
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", null)),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", null)),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithSameValues))]
        public void EqualsWithSameValuesShouldReturnTrue(XSerializable<NntpGroup> group1, XSerializable<NntpGroup> group2)
        {
            Assert.Equal(group1.Object, group2.Object);
        }

        public static IEnumerable<object[]> EqualsWithDifferentValues = new[]
        {
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group2", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
            },
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 11, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
            },
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 2, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
            },
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 11, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
            },
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.ArticlesFromPeersNotPermitted, "other", new long[]{1,2,3})),
            },
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "aaaaa", new long[]{1,2,3})),
            },
            new object[]
            {
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", new long[]{1,2,3})),
                new XSerializable<NntpGroup>(new NntpGroup("group1", 10, 1, 10, NntpPostingStatus.PostingPermitted, "other", null)),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithDifferentValues))]
        public void EqualsWithDifferentValuesShouldReturnFalse(XSerializable<NntpGroup> group1, XSerializable<NntpGroup> group2)
        {
            Assert.NotEqual(group1.Object, group2.Object);
        }
    }
}
