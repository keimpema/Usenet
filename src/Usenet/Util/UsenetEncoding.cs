using System.Text;

namespace Usenet.Util
{
    /// <summary>
    /// This class defines the default usenet character encoding.
    /// </summary>
    public static class UsenetEncoding
    {
        /// <summary>
        /// Returns iso-8859-1, the default usenet character encoding.
        /// </summary>
        public static readonly Encoding Default = Encoding.GetEncoding("iso-8859-1");
    }
}
