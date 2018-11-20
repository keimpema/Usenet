using System.Collections.Generic;
using Usenet.Nntp.Builders;
using Xunit;

namespace UsenetTests.Nntp.Builders
{
    public class NntpGroupsBuilderTests
    {
        [Fact]
        public void AddNullShouldResultInEmptyCollection()
        {
            NntpGroupsBuilder builder = new NntpGroupsBuilder().Add((string)null);
            Assert.Equal(new string[0], builder.Groups);
        }

        [Fact]
        public void AddNullEnumerableShouldResultInEmptyCollection()
        {
            NntpGroupsBuilder builder = new NntpGroupsBuilder().Add((IEnumerable<string>)null);
            Assert.Equal(new string[0], builder.Groups);
        }

        [Fact]
        public void AddSingleGroupShouldResultInSingleGroupString()
        {
            NntpGroupsBuilder builder = new NntpGroupsBuilder().Add("group1");
            Assert.Equal(new[] { "group1" }, builder.Groups);
        }

        [Fact]
        public void AddMultipleGroupsShouldResultInMultipleGroupsString()
        {
            NntpGroupsBuilder builder = new NntpGroupsBuilder().Add(new[] { "group1", "group2" }).Add("group3");
            Assert.Equal(new []{"group1","group2","group3" }, builder.Groups);
        }

        [Fact]
        public void EqualsWithDifferentOrderShouldReturnTrue()
        {
            NntpGroupsBuilder builder1 = new NntpGroupsBuilder().Add("group1").Add("group2");
            NntpGroupsBuilder builder2 = new NntpGroupsBuilder().Add("group2").Add("group1");
            Assert.Equal(builder1.Build(), builder2.Build());
        }

        [Fact]
        public void EqualsOperatorWithDifferentOrderShouldReturnTrue()
        {
            NntpGroupsBuilder builder1 = new NntpGroupsBuilder().Add("group1").Add("group2");
            NntpGroupsBuilder builder2 = new NntpGroupsBuilder().Add("group2").Add("group1");
            Assert.True(builder1.Build() == builder2.Build());
        }
    }
}
