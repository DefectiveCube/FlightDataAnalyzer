using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace XPlaneGenConsole
{
	public struct Term
	{
		public double Coefficient;
		public string Identifier;
		public double Exponent;

		public bool IsScalar {
			get { return string.IsNullOrEmpty (Identifier); }
		}

		public Term(){
			Coefficient = 1;
			Identifier = string.Empty;
			Exponent = 1;
		}

		public Expression<Func<double,double>> GetScalarExpression(){
			var left = Expression.Constant (Coefficient, typeof(double));
			var right = Expression.Constant (Exponent, typeof(double));

			return Expression.Lambda<Func<double,double>> (Expression.Multiply (left, right), new ParameterExpression[]{ });
		}

		public Expression<Func<double,double>> GetExpression(){
			var param = Expression.Parameter (typeof(double), Identifier);
			var left = Expression.Constant (Coefficient, typeof(double));
			var right = Expression.Power (param, Expression.Constant (Exponent, typeof(double)));
			var lambda = Expression.Lambda<Func<double,double>> (Expression.Multiply (left, right), new ParameterExpression[]{ param });

			return lambda;
		}

		public double GetValue(double input = 1){
			return Math.Pow (input, Exponent) * Coefficient;
		}

		public static Term operator + (Term a, Term b)
		{
			if (a.Identifier.Equals (b.Identifier) && a.Exponent == b.Exponent) {

				return new Term () {
					Coefficient = a.Coefficient + b.Coefficient,
					Identifier = a.Identifier,
					Exponent = a.Exponent
				};

			} else {
				throw new Exception ("Cannot add different terms!");
			}
		}

		public static Term operator * (Term a, Term b)
		{
			Term t = new Term ();

			if (!a.IsScalar && !b.IsScalar) {
				if (a.Identifier != b.Identifier) {
					throw new Exception ("Multiple variables not supported");
				}

				t.Coefficient = a.Coefficient * b.Coefficient;
				t.Identifier = a.Identifier;
				t.Exponent = a.Exponent + b.Exponent;

			} else if(a.IsScalar ^ b.IsScalar){
				var check = a.IsScalar;

				t.Coefficient = a.IsScalar ? a.GetValue () * b.Coefficient : b.GetValue () * a.Coefficient;
				t.Identifier = a.IsScalar ? b.Identifier : a.Identifier;
				t.Exponent = a.IsScalar ? b.Exponent : a.Exponent;

			} else {
				t.Coefficient = a.GetValue () + b.GetValue ();
			}	

			return t;
		}

		public static Term operator ^ (Term a, Term b){
			Debug.WriteLineIf (!string.IsNullOrEmpty (b.Identifier), "Notice: variable in exponent. Not supported");
			Debug.Write ("Multplying: ");
			Debug.Write (a.ToString ());
			Debug.Write (" and ");
			Debug.WriteLine (b.ToString ());

			return new Term () {
				Coefficient = a.Coefficient,
				Identifier = a.Identifier,
				Exponent = b.Coefficient
			};
		}

		public static Term Parse(string value, params string[] names)
		{
			var hasExponent = value.Contains ("^");
			var name = names.Where (n => value.IndexOf (n) > -1).FirstOrDefault ();
			var isSymbolic = !string.IsNullOrEmpty (name);

			/*Console.Write ("Parsing Term: ");
			Console.WriteLine (value);
			Console.WriteLine ("Symbolic Term: {0}", isSymbolic);
			Console.WriteLine ("Name: {0}", name);
			Console.WriteLine ("Exponent: {0}", hasExponent);*/

			string[] str = new string[]{ };
			double[] results = new double[]{ };

			if (isSymbolic) {
				str = value.Split (new string[]{ name }, StringSplitOptions.RemoveEmptyEntries);

				if (str [0].Contains ("^")) {
					str = new string[2]{ "1", str [0].Replace("^","") };
				} else if (hasExponent && str [1].Contains ("^")) {
					str [1] = str [1].Replace ("^", "");
				}

			} else if (hasExponent) {
				str = value.Split ('^');	
			} else {
				results = new []{ double.Parse (value) };
			}

			if (str.Length > 0) {
				results = new double[str.Length];

				for (int i = 0; i < str.Length; i++) {
					if (!double.TryParse (str [i], out results [i])) {
						throw new FormatException ("Invalid Format");
					}
				}
			}
		
			return new Term () {
				Coefficient = results.Count () > 0 ? results.First () : 1,
				Identifier = string.IsNullOrEmpty (name) ? string.Empty : name,
				Exponent = hasExponent ? results.Last () : 1
			};
		}

		public override string ToString ()
		{
			return string.Format ("({0}*{1}^{2})", Coefficient, Identifier, Exponent);
		}
	}
}