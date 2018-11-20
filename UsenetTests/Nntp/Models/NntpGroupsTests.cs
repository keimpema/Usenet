using System.Collections.Generic;
using Usenet.Nntp.Models;
using Xunit;

namespace UsenetTests.Nntp.Models
{
    public class NntpGroupsTests
    {
        [Fact]
        public void ConstrctWithEmptyStringShouldReturnEmptyString()
        {
            var groups = NntpGroups.Empty;
            Assert.Equal("", groups.ToString());
        }

        [Fact]
        public void ConstructWithNullShouldReturnEmptyString()
        {
            var groups = new NntpGroups((string)null);
            Assert.Equal("", groups.ToString());
        }

        [Fact]
        public void ConstructWithNullEnumerableShouldReturnEmptyString()
        {
            var groups = new NntpGroups((IEnumerable<string>)null);
            Assert.Equal("", groups.ToString());
        }

        [Fact]
        public void ConstructWithMultipleGroupsShouldReturnMultipleGroupsString()
        {
            var groups = new NntpGroups(new [] { "group1", "group2"});
            Assert.Equal("group1;group2", groups.ToString());
        }

        [Fact]
        public void ConstructWithSameGroupsShouldReturnSingleGroupString()
        {
            var groups = new NntpGroups(new[] { "group1", "group1" });
            Assert.Equal("group1", groups.ToString());
        }

        public static IEnumerable<object[]> EqualsWithSameValues = new[]
        {
            new object[]
            {
                new XSerializable<NntpGroups>(new NntpGroups("group1;group2")),
                new XSerializable<NntpGroups>(new NntpGroups("group1;group2")),
            },
            new object[]
            {
                new XSerializable<NntpGroups>(new NntpGroups("group3;group4")),
                new XSerializable<NntpGroups>(new NntpGroups("group4;group3")),
            },
            new object[]
            {
                new XSerializable<NntpGroups>(new NntpGroups("group5;group6")),
                new XSerializable<NntpGroups>(new NntpGroups("group6;group5;group5")),
            },
        };

        [Theory]
        [MemberData(nameof(EqualsWithSameValues))]
        public void EqualsWithSameValuesShouldReturnTrue(XSerializable<NntpGroups> groups1, XSerializable<NntpGroups> groups2)
        {
            Assert.Equal(groups1.Object, groups2.Object);
        }
    }
}
