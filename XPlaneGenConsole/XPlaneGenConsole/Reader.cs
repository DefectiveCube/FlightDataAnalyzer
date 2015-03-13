using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class Reader<T> where T : Datapoint<T>
    {
        private MemoryStream inStream;

        public Reader(Stream file)
        {
            inStream = new MemoryStream();
            file.CopyTo(inStream);
            file.Close();
            file.Dispose();

            inStream.Seek(0, SeekOrigin.Begin);
        }

        public IEnumerable<T> ReadAll()
        {
            List<string> allLines = new List<string>();

            using(StreamReader reader = new StreamReader(inStream))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    allLines.Add(reader.ReadLine());
                }
            }

            Console.WriteLine("[{1}] Lines: {0}", allLines.Count, typeof(T).Name);

            for(int i = 0; i < allLines.Count; i++)
            {
                T dp = Activator.CreateInstance<T>();

                dp.Load(allLines.ElementAt(i));

                if (dp.IsValid)
                {
                    dp.GetBytes();
                    yield return dp;                
                }
            }

            yield break;
        }
    }
}
