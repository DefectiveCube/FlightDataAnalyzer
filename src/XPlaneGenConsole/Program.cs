using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
//using MathNet.Symbolics;
//using Expr = MathNet.Symbolics.Expression;
//using Expression = System.Linq.Expressions.Expression;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
    class Program
    {
        /*
        /// <summary>
        /// Calculates the denominator portion of the polynomial
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        static IEnumerable<int> BaseTerms(IEnumerable<int> values)
        {
            for(int i = 0; i < values.Count(); i++)
            {
                int value = values.ElementAt(i);
                int total = 1;

                foreach (var v in values.Where((v,index) => index != i))
                {
                    total *= (value - v);
                }

                yield return total;
            }

            yield break;        
        }

        /// <summary>
        /// Reduces the y[i] / (x[i] - x[j]) portion into a scalar value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static IEnumerable<int> Combine(IEnumerable<int> x, IEnumerable<int> y)
        {
            for (int i = 0; i < x.Count(); i++)
            {
                yield return y.ElementAt(i) != 0 ? x.ElementAt(i) / y.ElementAt(i) : 0;
            }

            yield break;
        }

        /// <summary>
        /// Creates an expanded expression that passes through all points
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        static Expr Evaluate(IEnumerable<Tuple<int,int>> values)
        {
            Expr symbol = Expr.Symbol("x");
            Expr total = Expr.Zero;
            Expr accum;

            var xValues = values.Select(t => t.Item1);
            var scalars = Combine(BaseTerms(xValues), values.Select(t => t.Item2));

            for (int i = 0; i < xValues.Count(); i++)
            {
                accum = Expr.One;

                if (scalars.ElementAt(i) != 0)
                {
                    foreach (var val in xValues.Where((value, index) => index != i))
                    {
                        accum *= (symbol - val);
                    }

                    accum /= Expr.FromInt32(scalars.ElementAt(i));
                    total += accum;
                }
            }

            return Algebraic.Expand(total);
        }*/

        static void Main(string[] args)
        {
            var p = Expression.Parameter(typeof(Temperature), "t");
            var d = Expression.Parameter(typeof(double), "d");
            var tu = Expression.Parameter(typeof(TemperatureUnit), "tu");
            var t = typeof(Temperature);

            var call = Expression.Call(t.GetMethod("From"), d, tu);
            var l = Expression.Lambda<Func<double, TemperatureUnit, Temperature>>(call, d, tu);

            var func = l.Compile();

            var te = func(50.0, TemperatureUnit.DegreeFahrenheit);
            Console.WriteLine(l.ToString());


            //var get = Expression.Property(p, "Flight");
/*            var assign = Expression.Assign(
                    Expression.Property(p, "Flight"),
Expression.Call(typeof(Temperature),typeof
                //                    Expression.Parameter(typeof(int), "Flight")
                );
            var block = Expression.Block(assign);*/



            ///var f = Expression.Lambda<Func<FlightDatapoint, int>>(get, p).Compile();
            //var g = Expression.Lambda(assign, p);


            //Console.WriteLine(g.ToString());
            ///Console.WriteLine(f.ToString());

            return;
            //Conversion.Convert<FlightDatapoint, FlightCsvDatapoint>(null, null);
            //Console.ReadLine();
            //return;

            //MathNet.Numerics.LinearRegression.WeightedRegression.

            /*var x = Expr.Symbol("x");

            var list = new List<Tuple<int, int>>();
            list.Add(new Tuple<int, int>(0, 1));
            list.Add(new Tuple<int, int>(1, 1));
            list.Add(new Tuple<int, int>(2, 1));
            list.Add(new Tuple<int, int>(3, 1));
            list.Add(new Tuple<int, int>(4, 2));
            list.Add(new Tuple<int, int>(5, 2));
            list.Add(new Tuple<int, int>(6, 1));
            list.Add(new Tuple<int, int>(7, 1));
            list.Add(new Tuple<int, int>(8, 1));
            list.Add(new Tuple<int, int>(9, 1));

            var s = Evaluate(list);*/

            //Console.WriteLine(Infix.Print(s));
            //Console.ReadLine();
            //return;

            /*try
            {
                new ConsoleApp().Run(args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                Console.ReadLine();
            }*/
        }
    }
}