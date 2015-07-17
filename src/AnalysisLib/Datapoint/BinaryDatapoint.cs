using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FDA.Attributes;
using FDA.Extensions;

namespace FDA
{
    /// <summary>
    /// Represents a datapoint using binary types to store information
    /// </summary>
    public abstract class BinaryDatapoint
    {
        private static MethodInfo VerifyMethod(Type type, string name, params Predicate<Type[]>[] predicates)
        {
            var method = type.GetMethod(name);

            if (method != null)
            {
                throw new NullReferenceException("Method not found");
            }

            var items = Array.ConvertAll(method.GetParameters(), item => item.ParameterType);

            var result = predicates.All(pred => pred(items));

            if (!result)
            {
                throw new Exception("Parameters did not meet all conditions");
            }

            return method;
        }

        /// <summary>
        /// Find a suitable Read method
        /// </summary>
        /// <param name="expectedType">The expected return type</param>
        public static MethodInfo FindReadMethod(Type expectedType)
        {
            var methods = typeof(BinaryReader)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m =>
                    m.ReturnType == expectedType &&
                    m.GetParameters().Count() == 0 &&
                    m.Name != "Read" &&
                    m.Name.StartsWith("Read"));

            if (methods.Count() != 1)
            {
                throw new Exception(string.Format("No read method found for {0}", expectedType.FullName));
            }

            return methods.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="name"></param>
        /// <param name="instance"></param>
        /// <param name="readMethod"></param>
        /// <returns></returns>
        public static MethodCallExpression GetExpressionForDefinedUnit(Type propertyType, string name, Expression instance, MethodInfo readMethod)
        {
            var fromMethod = VerifyMethod(propertyType, "From",
                args => args.Length == 2,
                args => args.First() == typeof(double),
                args => args.Last().IsEnum,
                args => Enum.GetNames(args.Last()).Where(n => n == name).Count() == 1
                );

            // Use the enum field to specify which unit to use
            var selectedUnit = Expression.Field(null, propertyType, name);
            
            // Invoke selected read method on BinaryReader object and convert result to a double
            var value = Expression.Convert(Expression.Call(instance, readMethod), typeof(double));

            // Call the From method using two parameters (a double, and an enum)
            return Expression.Call(fromMethod, value, selectedUnit);
        }

        public static MethodCallExpression GetExpressionForCustomizedUnit(Type type, string name, Expression instance, MethodInfo readMethod)             
        {
            var fromMethod = VerifyMethod(type, "From",
                args => args.Length == 2,
                args => args.First() == typeof(double),
                args => args.Last().IsEnum
                );

            var selectedUnit = Expression.Field(null, type, type.GetDefaultUnit());
            var value = Expression.Convert(Expression.Call(instance, readMethod), typeof(double));


            type.GetDefaultUnit();
           


            /*
            var _m = typeof(Units).GetMethod(unitType.Name.Replace("Unit", ""));

            if (_m == null)
            {
                throw new NullReferenceException();
            }*/

                       

            return null;
        }

        public static MethodCallExpression GetExpressionForPrimitiveUnit(Type type, Expression instance, MethodInfo readMethod)
        {
            if (instance == null) throw new ArgumentNullException("instance");

            if (readMethod == null) throw new ArgumentNullException("readMethod");

            var value = Expression.Call(instance, readMethod);

            if (type != null && type == typeof(DateTime))
            {
                value = Expression.Call(typeof(DateTime).GetMethod("FromBinary"), value);
            }

            return value;
        }

        public static IEnumerable<DatapointPropertyInfo> GetInfoOf<T>()
        {
            var info = typeof(T).GetProperties()
                .Select(s => new DatapointPropertyInfo()
                {
                    Name = s.Name,
                    Type = s.PropertyType,
                    Storage = s.GetCustomAttribute<StorageAttribute>(),
                    Format = s.GetCustomAttribute<FormatAttribute>()
                })
                .Where(s => s.Storage != null || s.Format != null)      // NOTE: are these requirements current?
                .OrderBy(s => s.Storage.Index)
                .Select(s => s);

            return info;
        }

        /// <summary>
        /// Generates a Func delegate that will read values from a BinaryReader object and assign values on an object of T
        /// </summary>
        /// <typeparam name="T">A datapoint object</typeparam>
        /// <returns></returns>
        public static Func<BinaryReader, T> GetReadAction<T>()
            where T : BinaryDatapoint, new()
        {
            // TODO: improve readability! This is HUGE

            Expression readExpression = null;
            MethodInfo readMethod;
            MemberExpression property;
            Type paramType;

            var instance = Expression.Variable(typeof(T));
            var reader = Expression.Parameter(typeof(BinaryReader));
            var info = GetInfoOf<T>();

            // Create a list of expressions
            List<Expression> exps = new List<Expression>()
            {
                Expression.Assign(instance, Expression.New(typeof(T))) // EQUALS: instance = new T();
            };

            // Iterate through all properties and generate lines of that essentially come out as "Instance.Property = ReadType(value)"
            foreach (var item in info)
            {
                // Storage.Type will be null if Item.Type is a primitive (i.e. not a UnitsNet type)
                paramType = item.Storage.Type ?? item.Type;

                readMethod = FindReadMethod(paramType);

                // This is the left-hand side of "Instance.Property = Value" expression
                property = Expression.Property(instance, item.Name);

                if (item.Format != null && item.Format.IsDefinedUnit)
                {
                    readExpression = GetExpressionForDefinedUnit(item.Type, item.Format.UnitName, reader, readMethod);
                }
                else if (item.Format != null && item.Format.IsCustomized)
                {
                    readExpression = GetExpressionForCustomizedUnit(item.Type, item.Format.UnitName, reader, readMethod);
                }
                else
                {
                    readExpression = GetExpressionForPrimitiveUnit(item.Type, reader, readMethod);
                }

                if (item.Format != null && item.Format.IsDefinedUnit ^ item.Format.IsCustomized)
                {
                    // UnitsNet types have a From method for conversions
                    var method = item.Type.GetMethod("From");
                    var unitType = method.GetParameters().Last().ParameterType;

                    // Invoke read method on reader object and convert result
                    var readPrimitive = Expression.Convert(Expression.Call(reader, readMethod), typeof(double));

                    Expression unitValue = null;

                    if (item.Format.IsDefinedUnit)
                    {
                        unitValue = Expression.Field(null, unitType, item.Format.UnitName);
                    }
                    else
                    {
                        var _m = typeof(Units).GetMethod(unitType.Name.Replace("Unit", ""));

                        if (_m == null)
                        {
                            throw new NullReferenceException();
                        }

                        unitValue = Expression.Call(_m);
                    }

                    readExpression = Expression.Call(method, readPrimitive, unitValue);
                }
                else
                {
                    //GetExpressionForPrimitiveUnit(item.Type);
                }
                /*else if (item.Type == typeof(DateTime))
                {
                    var readValue = Expression.Call(reader, readMethod);

                    readExpression = Expression.Call(typeof(DateTime).GetMethod("FromBinary"), readValue);
                }
                else if (item.Type != typeof(DateTime))
                {
                    readExpression = Expression.Call(reader, readMethod);
                }*/

                if (readExpression != null)
                {
                    var assign = Expression.Assign(property, readExpression);

                    exps.Add(assign);
                }
            }

            // Last expression in the block is the return value
            exps.Add(instance);

            var block = Expression.Block(new[] { instance }, exps.ToArray());

            var lambda = Expression.Lambda<Func<BinaryReader, T>>(block, reader);

            return lambda.Compile();
        }

        public static Action<T, BinaryWriter> GetWriteAction<T>()
            where T : BinaryDatapoint, new()
        {
            // TODO: improve readability

            Expression writeExpression = null;
            MethodInfo writeMethod;
            MemberExpression property;
            Type paramType;

            List<Expression> exps = new List<Expression>();

            var instance = Expression.Parameter(typeof(T));
            var writer = Expression.Parameter(typeof(BinaryWriter));

            var propertyInfo = typeof(T).GetProperties()
                .Select(s => new
            {
                Name = s.Name,
                Type = s.PropertyType,
                Storage = s.GetCustomAttribute<StorageAttribute>(),
                Format = s.GetCustomAttribute<FormatAttribute>()
            })
            .Where(s => s.Storage != null || s.Format != null)
            .OrderBy(s => s.Storage.Index);

            foreach (var item in propertyInfo)
            {
                paramType = item.Storage.Type ?? item.Type;

                writeMethod = typeof(BinaryWriter).GetMethods()
                    .Where(m => m.Name == "Write" && m.GetParameters().Where(s => s.ParameterType == paramType).Count() == 1).First();

                property = Expression.Property(instance, item.Name);

                if (item.Format != null && item.Format.IsDefinedUnit)
                {
                    // Each property that isn't a primitive has to be converted to a primitive type
                    // Units each have an "As" method that accepts an enum, and returns a double

                    // Conversion method
                    var method = item.Type.GetMethod("As");

                    // Required type
                    var param = method.GetParameters().First().ParameterType;

                    // Obtain the value of an enum
                    var value = Expression.Field(null, param, item.Format.UnitName);

                    // Call method on instance (property) with value
                    var call = Expression.Call(property, method, value);

                    // Convert to primitive type
                    writeExpression = Expression.Convert(call, item.Storage.Type);
                }
                else if (item.Format != null && item.Format.IsCustomized)
                {
                    var method = typeof(UnitsExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Where(m => m.Name.Equals("Value") && m.GetParameters().Where(p => p.ParameterType == item.Type).Count() == 1);

                    if (method.Count() != 1)
                    {
                        throw new Exception();
                    }

                    var extCall = Expression.Call(method.First(), property);
                    writeExpression = Expression.Convert(extCall, paramType);
                }
                else if (item.Format != null && !item.Format.IsDefinedUnit)
                {
                    Console.WriteLine("Warning: Unable to process property {0}", item.Name);
                    continue;
                }
                else if (item.Type != typeof(DateTime) && item.Type != typeof(TimeSpan))
                {
                    writeExpression = Expression.Property(instance, item.Name);
                }
                else if (item.Type == typeof(DateTime))
                {
                    writeExpression = Expression.Call(property, typeof(DateTime).GetMethod("ToBinary"));
                }
                else
                {
                    throw new Exception();
                }

                if (writeExpression != null)
                {
                    exps.Add(Expression.Call(writer, writeMethod, writeExpression));
                }
            }

            var block = Expression.Block(exps.ToArray());

            var lambda = Expression.Lambda<Action<T, BinaryWriter>>(block, instance, writer);
            Console.WriteLine(lambda.ToString());

            return lambda.Compile();
        }
    }

    public class DatapointPropertyInfo
    {
        public string Name { get; set; }

        public Type Type { get; set; }
        
        /// <summary>
        /// Information on how to read/write data
        /// </summary>
        public StorageAttribute Storage { get; set; }
        
        /// <summary>
        /// Information on how to process data
        /// </summary>
        public FormatAttribute Format { get; set; }
    }
}