using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

using UnitsNet;
using UnitsNet.Units;
using System.IO;

namespace Builder
{
    public class Builder
    {
        CodeCompileUnit unit;
        CodeNamespace ns;
        CodeTypeDeclaration targetType;
        Dictionary<string, CodeTypeDeclaration> types;

        static void Main(string[] args)
        {
            int option = -1;
            var sb = new StringBuilder();

            sb.AppendLine("Systems: None");
            sb.AppendLine("Fields: 0");
            sb.AppendLine();
            sb.AppendLine("1. Add New System");
            sb.AppendLine("2. Remove System");
            sb.AppendLine("3. Add New Property");
            sb.AppendLine("4. Remove Property");
            sb.AppendLine("5. Generate");
            sb.AppendLine("0. Exit");

            while (option != 0)
            {
                Console.Clear();
                Console.Write(sb.ToString());

                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                        case 2:
                        default:
                            continue;
                    }
                }
                else
                {
                    option = -1;
                }
            }
            // Add New System
            // Add New Field


            // How many different systems?
            // Do you know how many fields?
            // If yes, how many


            // Add/Remove Field

            // Name?
            // Select a unit category
            // Select a unit

            // Continous or Discrete?

            // Define precision

            var b = new Builder();
            b.AddNamespace();
            b.AddSystem("EngineDatapoint");
            b.AddProperty("OilTemperature", typeof(Temperature));
            b.AddProperty("OilPressure", typeof(Pressure));
            b.AddProperty("EngineRPM", typeof(RotationalSpeed));
            b.AddProperty("EngineManifold", typeof(Pressure));
            b.AddProperty("EngineTIT", typeof(Temperature));
            b.AddProperty("CHT_1", typeof(Temperature));
            b.AddProperty("CHT_2", typeof(Temperature));
            b.AddProperty("CHT_3", typeof(Temperature));
            b.AddProperty("CHT_4", typeof(Temperature));
            b.AddProperty("CHT_5", typeof(Temperature));
            b.AddProperty("CHT_6", typeof(Temperature));

            b.AddSystem("FlightDatapoint");
            b.AddSystem("SystemDatapoint");

            b.Generate();
        }

        private Builder()
        {
            unit = new CodeCompileUnit();
            types = new Dictionary<string, CodeTypeDeclaration>();
        }

        void Build()
        {
            foreach (var type in types.Values)
            {
                ns.Types.Add(type);
            }
        }

        void Generate()
        {
            Build();

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions()
            {
                BracingStyle = "C"
            };
            var param = new CompilerParameters()
            {
                GenerateExecutable = true,
                GenerateInMemory = false,
                OutputAssembly = "test.dll"
            };


            provider.CompileAssemblyFromDom(param, unit);

            using (var writer = new StreamWriter(File.Open("test.cs", FileMode.Create)))
            {
                provider.GenerateCodeFromCompileUnit(unit, writer, options);
            }
        }

        void AddNamespace()
        {
            ns = new CodeNamespace("Kirk");

            unit.Namespaces.Add(ns);

            ns.Imports.Add(new CodeNamespaceImport("UnitsNet"));
            ns.Imports.Add(new CodeNamespaceImport("UnitsNet.Units"));
        }

        void AddSystem(string name)
        {
            Console.WriteLine("Enter a simple name");



            var cls = new CodeTypeDeclaration(name);
            cls.IsClass = true;
            cls.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;

            cls.BaseTypes.Add(new CodeTypeReference("BinaryDatapoint"));

            types.Add(name, cls);

            targetType = cls;
        }

        void RemoveSystem(string name)
        {
            types.Remove(name);
        }

        void AddProperty(string name, Type type, params Type[] attrTypes)
        {
            var prop = new CodeMemberField();
            prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            prop.Name = name + " { get; set; }";
            prop.Type = new CodeTypeReference(new CodeTypeParameter(type.Name));

            prop.CustomAttributes.AddRange(
                new CodeAttributeDeclaration[] {

                new CodeAttributeDeclaration()
            {
                Name = "CSVField",
                Arguments =
                {
                    new CodeAttributeArgument(){ Value = new CodePrimitiveExpression(targetType.Members.Count) },
                    new CodeAttributeArgument(){ Value = new CodeTypeOfExpression(typeof(float)) }
                }
            },
                new CodeAttributeDeclaration()
            {
                Name = "Format",
                Arguments =
                {
                    new CodeAttributeArgument() {
                        Value = new CodeFieldReferenceExpression(
                            new CodeTypeReferenceExpression("TemperatureUnit"),"DegreeFahrenheit")
                    }
                }
            }
                }
                );


            targetType.Members.Add(prop);
        }
    }
}
