using System.Collections.Generic;
using Newtonsoft.Json;
using Usenet.Util;
using Xunit;

namespace UsenetTests.Util
{
    public class MultiValueDictionaryTests
    {
        [Fact]
        public void MultipleValuesWithSameKeyShouldBeAdded()
        {

            var dict = new MultiValueDictionary<int, string>(() => new HashSet<string>())
            {
                {1, "one"},
                {1, "een"},
                {2, "two"},
                {2, "twee"},
                {2, "deux"}
            };

            Assert.Equal(5, dict.Count);
            Assert.Equal(2, dict[1].Count);
            Assert.Equal(3, dict[2].Count);

            Assert.True(new HashSet<string> { "one", "een" }.SetEquals(dict[1]));
            Assert.True(new HashSet<string> { "two", "twee", "deux" }.SetEquals(dict[2]));
        }

        [Fact]
        public void SameValueWithSameKeyShouldNotBeAddedWhenUsingHashSet()
        {

            var dict = new MultiValueDictionary<int, string>(() => new HashSet<string>())
            {
                {1, "one"},
                {1, "one"},
            };

            Assert.Equal(1, dict.Count);
            Assert.Equal(1, dict[1].Count);

            Assert.True(new HashSet<string> { "one" }.SetEquals(dict[1]));
        }

        [Fact]
        public void SameValueWithSameKeyShouldBeAddedWhenUsingList()
        {

            var dict = new MultiValueDictionary<int, string>(() => new List<string>())
            {
                {1, "one"},
                {1, "one"},
            };

            Assert.Equal(2, dict.Count);
            Assert.Equal(2, dict[1].Count);

            Assert.Equal(new List<string> { "one", "one" }, dict[1]);
        }

        [Fact]
        public void RemovingItemsShouldDecreaseCount()
        {
            var dict = new MultiValueDictionary<int, string>(() => new HashSet<string>())
            {
                {1, "one"},
                {1, "een"},
                {2, "two"},
                {2, "twee"},
                {2, "deux"}
            };
            dict.Remove(2, "twee");
            dict.Remove(1);

            Assert.Equal(2, dict.Count);
            Assert.Equal(2, dict[2].Count);
        }

        [Fact]
        public void ClearingDictionaryShouldResultInCountZero()
        {
            var dict = new MultiValueDictionary<int, string>(() => new HashSet<string>())
            {
                {1, "one"},
                {1, "een"},
                {2, "two"},
                {2, "twee"},
                {2, "deux"}
            };
            dict.Clear();

            Assert.Equal(0, dict.Count);
        }

        [Fact]
        public void DictionariesShouldBeEqualIndependentOfOrder()
        {
            var dict1 = new MultiValueDictionary<int, string>(() => new HashSet<string>())
            {
                {2, "twee"},
                {2, "two"},
                {1, "een"},
                {2, "deux"},
                {1, "one"},
            };

            var dict2 = new MultiValueDictionary<int, string>(() => new HashSet<string>())
            {
                {1, "one"},
                {1, "een"},
                {2, "two"},
                {2, "twee"},
                {2, "deux"}
            };

            Assert.Equal(dict1, dict2);
            Assert.True(dict1 == dict2);
            Assert.True(dict1.Equals(dict2));
        }

        [Fact]
        public void DictionaryCanBeSerializedToJsonAndDeserializedBackToDictionary()
        {
            var expected = new MultiValueDictionary<int, string>(() => new HashSet<string>())
            {
                {2, "twee"},
                {2, "two"},
                {1, "een"},
                {2, "deux"},
                {1, "one"},
            };

            string json = JsonConvert.SerializeObject(expected);
            var actual = JsonConvert.DeserializeObject<MultiValueDictionary<int, string>>(json);
            Assert.Equal(expected, actual);
        }
    }
}
