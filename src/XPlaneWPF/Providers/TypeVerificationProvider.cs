using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using XPlaneGenConsole;
using XPlaneWPF.ViewModels;

namespace XPlaneWPF.Providers
{
    /// <summary>
    /// This class is responsible for verifying the integrity and format of assemblies that are about to be loaded
    /// </summary>
    public class TypeVerificationProvider : DataSourceProvider
    {
        protected override void BeginQuery()
        {
            var paths = App.Current.TryFindResource("PathProvider") as PathProvider;

            foreach (var file in Directory.EnumerateFiles(paths.ModelDirectory, "*.dll"))
            {
                //Load(file);
            }

            base.OnQueryFinished(null);
        }
        
        public void Load(string path)
        {
            Assembly asm;

            try
            {
                asm = Assembly.ReflectionOnlyLoadFrom(path);

                var asmTypes = asm.GetTypes();
                var modelTypes = asm.GetCustomAttributesData();

                // Assembly must have one or more occurances of DatapointAttribute
                if (modelTypes == null)
                {
                    //return;
                }


                // Assembly cannot contain extra types
                // Types must be public, not-static

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private bool VerifyType(Type type)
        {
            var check =
                type.IsClass &&
                !type.IsGenericType &&
                type.IsPublic;

            return check;
        }
    }
}