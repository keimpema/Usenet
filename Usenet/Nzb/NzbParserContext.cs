using System.Xml.Linq;

namespace Usenet.Nzb
{
    /// <summary>
    /// Represents a context for the <see cref="NzbParser"/>.
    /// </summary>
    internal class NzbParserContext
    {
        /// <summary>
        /// The <see cref="XNamespace"/> to use.
        /// </summary>
        public XNamespace Namespace { get; set; }
    }
}
