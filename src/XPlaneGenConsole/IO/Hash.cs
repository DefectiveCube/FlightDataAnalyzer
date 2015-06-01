using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace XPlaneGenConsole.IO
{
    public static class Hash
    {
        private static HashAlgorithm Algorithm = SHA256Managed.Create();

        public static byte[] ComputeHash(string file)
        {
            return ComputeHash(File.OpenRead(file));
        }

        public static byte[] ComputeHash(Stream stream)
        {
            return Algorithm.ComputeHash(stream);
        }
    }
}