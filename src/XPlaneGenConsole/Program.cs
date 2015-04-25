using System;
using System.Diagnostics;

namespace XPlaneGenConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new ConsoleApp().Run(args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
