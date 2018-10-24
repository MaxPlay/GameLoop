/*
 * Usage of GZip in C# as advised here:
 * https://stackoverflow.com/a/7343623/5593150
 */

using System.IO;
using System.IO.Compression;
using System.Text;

namespace GameLoop.Compression
{
    public static class GZip
    {
        #region Public Methods

        public static string Unzip(byte[] data)
        {
            using (var msi = new MemoryStream(data))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static byte[] Zip(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return mso.ToArray();
            }
        }

        #endregion Public Methods
    }
}