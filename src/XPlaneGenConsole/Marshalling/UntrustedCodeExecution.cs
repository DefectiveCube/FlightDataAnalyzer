using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace XPlaneGenConsole
{
    public class UntrustedCodeExecution : MarshalByRefObject
    {
        public string Assemblies
        {
            get { return AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName).First(); }
        }


        public void DisplayAssemblies()
        {
            foreach (var str in AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName))
            {
                Console.WriteLine(str);
            }
        }

        public bool LoadAssembly(string name)
        {
            var asm = Assembly.ReflectionOnlyLoad("WMU");

            var list = asm.GetCustomAttributesData();

            return true;
            //return (asm.GetCustomAttributesData()//GetCustomAttribute<DatapointAttribute>() == null);
        }

        public void LoadCsv(string path)
        {
            //using(var parser = CsvParser.GetParser<)


            // What type of datapoint ???
        }

        public void Read(string file)
        {
            BinaryDatapoint.GetReadAction<FlightDatapoint>();
        }
        public void Write()
        {
            BinaryDatapoint.GetWriteAction<FlightDatapoint>();
        }

        public void DoNothing()
        {
            try
            {
                Console.WriteLine("--- Loading Sandbox ---");

                //if (LoadAssembly("WMU"))
                {
                    //  return;
                }

                var asm = AppDomain.CurrentDomain.Load("WMU");

                if (asm == null)
                {
                    Console.WriteLine("Assembly not found");
                    return;
                }

                var type = asm.GetTypes().Where(t => t.Name.Equals("Test"));

                if (type.Count() == 0)
                {
                    Console.WriteLine("Type not found");
                    return;
                }

                var method = type.First().GetMethod("Read");

                if (method == null)
                {
                    Console.WriteLine("Method not found");
                    return;
                }

                Console.WriteLine(Thread.GetDomain().FriendlyName);

                try
                {
                    method.Invoke(null, new object[] { });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }

            }
            catch (SecurityException ex)
            {
                (new PermissionSet(PermissionState.Unrestricted)).Assert();
                Console.WriteLine("SecurityException caught:\n{0}", ex.ToString());
                CodeAccessPermission.RevertAssert();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
