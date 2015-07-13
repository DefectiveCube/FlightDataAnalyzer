using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FDA.Attributes;

namespace FDA
{
    /// <summary>
    /// Represents a datapoint using binary types to store information
    /// </summary>
    public abstract class BinaryDatapoint
    {
        /// <summary>
        /// Generates a Func delegate that will values for T through a BinaryReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
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

            // Create a list of expressions
            List<Expression> exps = new List<Expression>()
            {
                Expression.Assign(instance, Expression.New(typeof(T))) // instance = new T();
            };

            var propertyInfo = typeof(T).GetProperties()
                .Select(s => new
            {
                Name = s.Name,
                Type = s.PropertyType,
                Storage = s.GetCustomAttribute<StorageAttribute>(),
                Format = s.GetCustomAttribute<FormatAttribute>()
            })
            .Where(s => s.Storage != null || s.Format != null)
            .OrderBy(s => s.Storage.Index)
            .Select(s => s);

            foreach (var item in propertyInfo)
            {
                paramType = item.Storage.Type ?? item.Type; // Storage.Type will be null if Item.Type is a primitive (i.e. not a UnitsNet type)

                // Find a suitable Read method
                var methods = typeof(BinaryReader).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => 
                        m.ReturnType == paramType &&
                        m.GetParameters().Count() == 0 &&
                        m.Name != "Read" &&
                        m.Name.StartsWith("Read"));

                if (methods.Count() != 1)
                {
                    throw new Exception(string.Format("No read method found for {0}", paramType.FullName));
                }

                readMethod = methods.First();

                property = Expression.Property(instance, item.Name);

                if (item.Format != null && item.Format.IsDefinedUnit ^ item.Format.IsCustomized)
                {
                    // UnitsNet types have a From method for conversions
                    var method = item.Type.GetMethod("From");
                    var unitType = method.GetParameters().Last().ParameterType;

                    //
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
                else if (item.Type == typeof(DateTime))
                {
                    var readValue = Expression.Call(reader, readMethod);                

                    readExpression = Expression.Call(typeof(DateTime).GetMethod("FromBinary"), readValue);
                }
                else if (item.Type != typeof(DateTime))
                {
                    readExpression = Expression.Call(reader, readMethod);
                }

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
                    var method = typeof(Extensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                        .Where(m => m.Name.Equals("Value") && m.GetParameters().Where(p => p.ParameterType == item.Type).Count() == 1);

                    if (method.Count() != 1)
                    {
                        throw new Exception();
                    }

                    var extCall = Expression.Call(method.First(), property);
                    writeExpression = Expression.Convert(extCall, paramType);
                }
                else if(item.Format != null && !item.Format.IsDefinedUnit){
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
}