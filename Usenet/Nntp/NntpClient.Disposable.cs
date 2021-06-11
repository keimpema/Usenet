using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using Usenet.Extensions;

namespace Usenet.Nntp
{
    /// <summary>
    /// An NNTP client that is compliant with
    /// <a href="https://tools.ietf.org/html/rfc2980">RFC 2980</a>,
    /// <a href="https://tools.ietf.org/html/rfc3977">RFC 3977</a>,
    /// <a href="https://tools.ietf.org/html/rfc4643">RFC 4643</a> and
    /// <a href="https://tools.ietf.org/html/rfc6048">RFC 6048</a>.
    /// Based on Kristian Hellang's NntpLib.Net project https://github.com/khellang/NntpLib.Net.
    /// </summary>
    public partial class NntpClient : IDisposable
    {
        ~NntpClient()
        {
            Dispose();
            
        }

        //Borrowed from https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

        // To detect redundant calls
        private bool _disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_disposed)
                {
                    return;
                }

                if (disposing)
                {
                    //this.Quit();
                    // Dispose managed state (managed objects).
                    this.connection?.Dispose();
                    _safeHandle?.Dispose();
                    Common.NntpExtensions.DecramentClientCount();
                }

                _disposed = true;
            }
        }
    }
}
