using System.IO;
using System.Security.Cryptography;

namespace XPlaneGenConsole
{
    public sealed class Hash
    {
        private Hash() { }

        public static byte[] ComputeSHA1Hash(string path)
        {
            return ComputeSHA1Hash(File.OpenRead(path));
        }

        public static byte[] ComputeSHA1Hash(Stream stream)
        {
            return new SHA1Managed().ComputeHash(stream);
        }

        public static byte[] ComputeSHA1Hash(byte[] data)
        {
            return new SHA1Managed().ComputeHash(data);
        }
    }
}
