using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnitsNet;
using UnitsNet.Units;

namespace FDA.IO
{
    /// <summary>
    /// Creates Types from XML files. Incomplete
    /// </summary>
    public class TypeCreater
    {
        private readonly static TypeAttributes attributes = 
            TypeAttributes.Sealed | 
            TypeAttributes.Public | 
            TypeAttributes.Class | 
            TypeAttributes.AnsiClass | 
            TypeAttributes.AutoClass | 
            TypeAttributes.BeforeFieldInit | 
            TypeAttributes.AutoLayout;
        private readonly static MethodAttributes propertyAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
        private static AssemblyBuilder assemblyBuilder;
        private static ModuleBuilder moduleBuilder;
        private static TypeBuilder typeBuilder;
        private static string groupName;

        public static void LoadModels(string path)
        {
            CreateAssembly();
            Type type;
            foreach (var file in Directory.EnumerateFiles(path, "*.xml"))
            {
                Load(file);
                type = typeBuilder.CreateType();
            }

            var dp = assemblyBuilder.GetTypes();
            var obj = assemblyBuilder.DefinedTypes;
        }

        private static void CreateAssembly()
        {
            var asmName = new AssemblyName("CustomModels");
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            moduleBuilder = assemblyBuilder.DefineDynamicModule("Models");
        }

        private static void CreateType(string name,string ns)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ns))
            {
                throw new ArgumentNullException();
            }

            typeBuilder = moduleBuilder.DefineType(ns + "." + name, attributes);
            var ctor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, Type.EmptyTypes);

            var il = ctor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));
            il.Emit(OpCodes.Ret);
        }

        private static void CreateProperty(string name, Type type)
        {
            var backingField = typeBuilder.DefineField("_" + name, type, FieldAttributes.Private);
            var property = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, null);
            var getProperty = typeBuilder.DefineMethod("get_" + name, propertyAttributes, type, Type.EmptyTypes);
            var setProperty = typeBuilder.DefineMethod("set_" + name, propertyAttributes, null, new[] { type });
            var getterIL = getProperty.GetILGenerator();
            var setterIL = setProperty.GetILGenerator();
            var lbl = setterIL.DefineLabel();
            var exitLbl = setterIL.DefineLabel();

            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, backingField);
            getterIL.Emit(OpCodes.Ret);

            setterIL.MarkLabel(lbl);
            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, backingField);
            setterIL.Emit(OpCodes.Nop);
            setterIL.MarkLabel(exitLbl);
            setterIL.Emit(OpCodes.Ret);

            property.SetGetMethod(getProperty);
            property.SetSetMethod(setProperty);           
        }

        public static Type GetUnitType(string name)
        {
            switch (name)
            {
                case "Acceleration":
                    return typeof(Acceleration);
                case "Angle":
                    return typeof(Angle);
                case "ElectricCurrent":
                    return typeof(ElectricCurrent);
                case "ElectricPotential":
                    return typeof(ElectricPotential);
                case "ElectricResistance":
                    return typeof(ElectricResistance);
                case "Pressure":
                    return typeof(Pressure);
                case "Ratio":
                    return typeof(Ratio);                    
                case "RotationalSpeed":
                    return typeof(RotationalSpeed);
                case "Temperature":
                    return typeof(Temperature);
                case "Volume":
                    return typeof(Volume);
                case "byte":
                    return typeof(byte);
                case "int":
                    return typeof(int);
                case "float":
                    return typeof(float);
                case "ushort":
                    return typeof(ushort);
                case "DateTime":
                    return typeof(DateTime);
                case "TimeSpan":
                    return typeof(TimeSpan);
                default:
                    Console.WriteLine(name);
                    return null;
            }
        }

        public static void Load(string path)
        {
            var settings = new XmlReaderSettings() { Async = false };

            using (var reader = XmlReader.Create(File.OpenRead(path),settings))
            {
                while (reader.Read())
                {
                    if (!reader.IsStartElement())
                    {
                        if (reader.Name == "Group")
                        {
                            groupName = string.Empty;
                        }

                        continue;
                    }

                    var name = reader.Name;

                    switch (name)
                    {
                        case "Model":
                            // Create Type
                            var typeName = reader.GetAttribute("Name");
                            CreateType(typeName, "Generated");
                            break;
                        case "Group":
                            // Store Group Name
                            groupName = reader.GetAttribute("Name");
                            break;
                        case "Property":
                            // Create Property
                            var propertyName = reader.GetAttribute("Name");
                            var unitType = reader.GetAttribute("UnitType");
                            var unit = string.Empty;
                            var type = GetUnitType(unitType);

                            if (type != null)
                            {
                                if (!type.IsPrimitive)
                                {
                                    unit = reader.GetAttribute("Unit");
                                }
                            }
                            else
                            {
                                // primitive
                            }

                            //CreateProperty(propertyName, type);
                            break;
                        case "CSV:Field":
                            // Apply Attribute
                            break;
                        case "Storage":
                            // Apply Attribute
                            break;
                    }

                    //Console.WriteLine("---");
                    //Console.WriteLine(reader.Name);
                    //Console.WriteLine(reader.LocalName);
                    //Console.WriteLine("---");
                }
            }
        }
    }
}
