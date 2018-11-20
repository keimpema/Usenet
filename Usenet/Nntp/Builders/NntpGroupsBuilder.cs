using System.Collections.Generic;
using Usenet.Nntp.Models;
using Usenet.Nntp.Parsers;

namespace Usenet.Nntp.Builders
{
    /// <summary>
    /// Represents a mutable <see cref="NntpGroups"/>.
    /// </summary>
    public class NntpGroupsBuilder
    {
        private readonly List<string> groups;

        /// <summary>
        /// The raw groups collection.
        /// </summary>
        public IEnumerable<string> Groups => groups;

        /// <summary>
        /// Creates a new instance of the <see cref="NntpGroupsBuilder"/> class.
        /// </summary>
        public NntpGroupsBuilder()
        {
            groups = new List<string>();
        }

        /// <summary>
        /// Gets a value that indicates whether this list is empty.
        /// </summary>
        public bool IsEmpty => groups.Count == 0;

        /// <summary>
        /// Adds a new value to the <see cref="NntpGroups"/> object.
        /// </summary>
        /// <param name="value">One or more NNTP newsgroup names seperated by the ';' character.</param>
        /// <returns>The <see cref="NntpGroups"/> object so that additional calls can be chained.</returns>
        public NntpGroupsBuilder Add(string value)
        {
            AddGroups(GroupsParser.Parse(value));
            return this;
        }

        /// <summary>
        /// Adds new values to the <see cref="NntpGroups"/> object.
        /// </summary>
        /// <param name="values">One or more NNTP newsgroup names seperated by the ';' character.</param>
        /// <returns>The <see cref="NntpGroups"/> object so that additional calls can be chained.</returns>
        public NntpGroupsBuilder Add(IEnumerable<string> values)
        {
            AddGroups(GroupsParser.Parse(values));
            return this;
        }

        /// <summary>
        /// Removes a new value from the <see cref="NntpGroups"/> object.
        /// </summary>
        /// <param name="value">One or more NNTP newsgroup names seperated by the ';' character.</param>
        /// <returns>The <see cref="NntpGroups"/> object so that additional calls can be chained.</returns>
        public NntpGroupsBuilder Remove(string value)
        {
            RemoveGroups(GroupsParser.Parse(value));
            return this;
        }

        /// <summary>
        /// Removes values from the <see cref="NntpGroups"/> object.
        /// </summary>
        /// <param name="values">One or more NNTP newsgroup names seperated by the ';' character.</param>
        /// <returns>The <see cref="NntpGroups"/> object so that additional calls can be chained.</returns>
        public NntpGroupsBuilder Remove(IEnumerable<string> values)
        {
            RemoveGroups(GroupsParser.Parse(values));
            return this;
        }

        /// <summary>
        /// Creates a <see cref="NntpGroups"/> with al the properties from the <see cref="NntpGroupsBuilder"/>.
        /// </summary>
        /// <returns>The <see cref="NntpGroups"/>.</returns>
        public NntpGroups Build() => new NntpGroups(groups, false);

        private void AddGroups(IEnumerable<string> values)
        {
            if (values == null)
            {
                return;
            }
            foreach (string group in values)
            {
                if (!groups.Contains(group))
                {
                    groups.Add(group);
                }
            }
        }

        private void RemoveGroups(IEnumerable<string> values)
        {
            if (values == null)
            {
                return;
            }
            foreach (string group in values)
            {
                groups.RemoveAll(g => g == group);
            }
        }
    }
}
