using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

using UnitsNet;
using UnitsNet.Units;
using XPlaneGenConsole;
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
			var b = new Builder();


			b.ShowMainMenu ();
            /*int option = -1;
            var sb = new StringBuilder();

			sb.AppendLine (char.ConvertFromUtf32 (218));
            sb.AppendLine("Systems: None");
            sb.Append("Fields: 0");
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
				//Console.ForegroundColor = ConsoleColor.DarkBlue;
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
            }*/
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


		void ShowMainMenu()
		{
			Console.Clear ();

			var sb = new StringBuilder ();

			sb.AppendLine ("Builder v1.0 -- Build Data Models for FDA");
			sb.AppendLine ();

			sb.Append ("Systems: ");
			sb.AppendLine (types.Count ().ToString ());
			sb.Append ("Fields: ");
			sb.AppendLine (0.ToString());
			sb.AppendLine ();
			sb.AppendLine ("1. Add New System");

			if (types.Count () > 0) {
				sb.AppendLine ("2. Remove System");
				sb.AppendLine ("3. Edit Properties");
			}

			sb.AppendLine ("Mark as Abstract");
			sb.AppendLine ("Import Model");
			sb.AppendLine ("0. Exit");


			Console.Write (sb.ToString ());

			int num = -1;

			while (!int.TryParse (Console.ReadLine (), out num)) {
				
			}
				
			switch (num) {
			case 1:
				MenuAddSystem ();
				break;
			case 2:
				MenuRemoveSystem ();
				break;
			case 3:
				break;
			}
		}

		void MenuAddSystem()
		{
			int num = -1;


			var sb = new StringBuilder ();
			sb.AppendLine ("Name of system?");

			// limit characters
			Console.Clear ();
			Console.WriteLine ("Name of system?");

			string name = Console.ReadLine ();

			while (name == string.Empty) {
				Console.Clear ();
				Console.WriteLine ("Name of system?");
				Console.WriteLine ("Try again");
				name = Console.ReadLine ();
			}

			Console.Clear ();
			Console.WriteLine ("How many fields per row?");

			while (!int.TryParse (Console.ReadLine (), out num) && num < 1) {

			}

			Console.Clear ();
			MenuSetFieldType ();
		}

		void MenuRemoveSystem(){


		}

		void MenuSetFieldType()
		{
			var name = "Field#1";
			var sb = new StringBuilder ();
			sb.Append ("Field: ");
			sb.AppendLine (name);
			sb.AppendLine ();
			sb.AppendLine ("1. Acceleration");
			// Amplitude Ratio
			sb.AppendLine ("2. Angle");
			// Area
			// Density
			// Duration
			sb.AppendLine ("3. Electric Current");
			sb.AppendLine ("4. Electric Potential");
			// Electrical Resistance
			// Energy
			// Flow
			// Force
			// Frequency
			// Information
			// Kinematic Viscosity
			sb.AppendLine ("5. Length");
			// Level
			// Mass
			// Power
			// PowerRatio
			sb.AppendLine ("6. Pressure");
			sb.AppendLine ("7. Ratio");
			sb.AppendLine ("8. Rotational Speed");
			// Specific Weight
			sb.AppendLine ("9. Speed");
			sb.AppendLine ("10. Temperature");
			sb.AppendLine ("11. Torque");
			sb.AppendLine ("12. Volume");

			Console.Write (sb.ToString ());
			int num = -1;

			while(true){
				while(!int.TryParse(Console.ReadLine(), out num) && num < 0)
				{
					Console.Clear ();
					Console.Write (sb.ToString ());
				}

				switch (num) {
				case 1:
					MenuSetFieldUnit<Acceleration> ();
					break;
				case 2:
					MenuSetFieldUnit<Angle> ();
					break;
				case 3:
					MenuSetFieldUnit<ElectricCurrent> ();
					return;
				default:
					continue;
				}

				break;
			}
		}

		void MenuSetFieldUnit<T>()
			where T : struct
		{
			var names = XPlaneGenConsole.UnitInfo.GetUnitNames (typeof(T));

			Console.Clear ();

			foreach (var name in names) {
				Console.WriteLine (name);
			}

			Console.ReadLine ();
		}

        private Builder()
        {
            unit = new CodeCompileUnit();
            types = new Dictionary<string, CodeTypeDeclaration>();
        }


		void PositionElement()
		{
			/*
			 * 1. Move to Top
			 * 2. Insert Before
			 * 3. Swap
			 * 4. Insert After
			 * 5. Move to Bottom
			 * 6. Rotate (Ascend)
			 * 7. Rotate (Descend)
			 */
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