using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XPlaneGenConsole
{
	public static class EquationExt
	{
		public static IEnumerable<Term> Reduce(this IEnumerable<Term> source){


			return source;
		}

		public static Term Combine(this IEnumerable<Term> source, Func<Term, double> func){
			Term t = new Term ();
			t.Coefficient = source.Select (func).Sum ();
			t.Identifier = source.First ().Identifier;
			t.Exponent = source.First ().Exponent;

			return t;
		}

		public static Term Product(this IEnumerable<Term> source){
			Term t = new Term () {
				Coefficient = 1,
				Identifier = source.First ().Identifier,
				Exponent = 1
			};


			foreach (var term in source) {
				t *= term;
			}

			return t;
		}
	}
		
	public class MathToken
	{
		public MathToken leftToken;
		public MathToken rightToken;
		public Expression expression;
	}

	public class MathOperatorToken : MathToken{
		public MathOperation Value;
		public MathAssociativity Associativity;
		public int Precedence;

		public MathOperatorToken(MathOperation operation){
			Value = operation;
			Associativity = MathAssociativity.Left;

			switch (Value) {
			case MathOperation.Add:
			case MathOperation.Subtract:
				Precedence = 1;
				break;
			case MathOperation.Multiply:
			case MathOperation.Divide:
				Precedence = 2;
				break;
			case MathOperation.Exponent:
				Associativity = MathAssociativity.Right;
				Precedence = 3;
				break;
			}
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}
	public class MathOperandToken : MathToken {
		public double Value;

		public MathOperandToken(double value){
			Value = value;
		}

		public override string ToString ()
		{
			return Value.ToString ();
		}
	}

	public class MathVariableToken : MathToken {
		public string Value;

		public MathVariableToken(string value){
			Value = value;
		}

		public override string ToString ()
		{
			return Value;
		}
	}


	public enum MathOperation
	{
		None,
		Add,
		Subtract,
		Multiply,
		Divide,
		Exponent,
		OpenGroup,
		CloseGroup
	}

	public enum MathAssociativity
	{
		Left,
		Right
	}

	public class Equation
	{
		private Dictionary<string,ParameterExpression> Parameters;
		public List<string> Vars{ get; private set; } // name of variables being used
		public List<Term> Terms{get;private set;} // contains terms that are not in a group

		public List<Expression> ExpressionTerms{ get; private set; }

		public Expression<Func<double,double>> CompiledExpression {get; private set;}

		private Func<double,double> Eval;

		public string Text {get; private set;}
	
		public string FormattedString { get; private set; }

		public List<Equation> Groups{ get; set; }

		public bool IsSingleTerm { get { return Groups.Count == 0 && Terms.Count == 1; } }
		public bool IsBinomial { get { return false; } }

		public Equation()
		{
			Groups = new List<Equation> ();
			Terms = new List<Term> ();
			Vars = new List<string> ();
			Parameters = new Dictionary<string, ParameterExpression> ();
			ExpressionTerms = new List<Expression> ();
		}

		public static Equation operator + (Equation a, Equation b)
		{
			var e = new Equation ();

			e.Terms.AddRange (a.Terms);
			e.Terms.AddRange (b.Terms);

			return e;
		}

		public static Equation operator * (Equation a, Equation b)
		{
			List<Term> terms = new List<Term> ();

			foreach(Term t in a.Terms)
			{
				foreach (Term u in b.Terms) {
					terms.Add (t * u);
				}
			}

			return new Equation () {
				FormattedString = string.Format ("({0}) * ({1})", a.FormattedString, b.FormattedString),
				Terms = terms
			};
		}

		void Distribute()
		{
			Equation first;
			if (Groups.Count () > 1) {
				first = Groups.First ();

				for (int i = 1; i < Groups.Count (); i++) {
					first = first * Groups.ElementAt (i);
				}
			}

		}

		/// <summary>
		/// Promotes scalar terms from child groups to this equation
		/// </summary>
		public void Flatten()
		{
			var groups = Groups.Where (g => g.IsSingleTerm);

			foreach (var g in groups) {
				Terms.Add (g.Terms.First ());
			}

			Groups.RemoveAll (g => g.IsSingleTerm);

			Distribute ();
			Simplify ();
		}

		/// <summary>
		/// Combines like-terms in equation
		/// </summary>
		public void Simplify()
		{
			bool changed = false;
			var termGroups = Terms.GroupBy (t => new {t.Identifier, t.Exponent});

			foreach (var g in termGroups) {

				if (string.IsNullOrEmpty (g.Key.Identifier)) {
					return;
				}

				Console.WriteLine ("{0},{1}", g.Key.Identifier, g.Key.Exponent);

				if (g.Count () > 1) {

					var terms = Terms.Where (t => t.Identifier == g.Key.Identifier && t.Exponent == g.Key.Exponent);
					var term = terms.Combine(f => f.Coefficient);

					Terms.RemoveAll (t => t.Identifier == g.Key.Identifier && t.Exponent == g.Key.Exponent);
					Terms.Add (term);
					changed = true;
				}
			}


			if (changed) {
				Debug.WriteLine ("Simplified Equation");
				Rewrite ();
			}
		}

		public override string ToString ()
		{
			return CompiledExpression.ToString ();
		}

		private void Rewrite()
		{
			var sb = new StringBuilder ();

			foreach (var t in Terms) {
				sb.Append (t.ToString ());
				sb.Append ("+");
			}

			sb.Length--;
			Text = sb.ToString ();

			Parse ();
		}

		private static Queue<MathToken> Lex(string value)
		{
			Queue<MathToken> outputQueue = new Queue<MathToken> ();
			Stack<MathOperatorToken> opStack = new Stack<MathOperatorToken> ();
			StringReader reader = new StringReader (value);

			while (reader.Peek () < char.MaxValue) {
				ReadWhitespace (ref reader);

				char val = (char)reader.Peek ();

				if (char.IsDigit (val)) {
					outputQueue.Enqueue (ReadOperand (ref reader));
				} else if (char.IsLetter (val)) {
					outputQueue.Enqueue (ReadVariable (ref reader));
				} else {
					if (val == '(') {
						outputQueue.Enqueue (new MathOperatorToken (MathOperation.OpenGroup));
						opStack.Push (new MathOperatorToken (MathOperation.OpenGroup));
						reader.Read ();
					} else if (val == ')') {
						while (opStack.Peek ().Value != MathOperation.OpenGroup) {
							outputQueue.Enqueue (opStack.Pop ());
						}
						opStack.Pop ();
						reader.Read ();
						outputQueue.Enqueue (new MathOperatorToken (MathOperation.CloseGroup));
					} else if ((val + "").IndexOfAny ("+-*/^".ToCharArray ()) > -1) {
						var op = ReadOperator (ref reader);

						while (opStack.Count() > 0) {

							if (op.Associativity == MathAssociativity.Left && op.Precedence <= opStack.Peek ().Precedence ||
							    op.Associativity == MathAssociativity.Right && op.Precedence < opStack.Peek ().Precedence) {
								outputQueue.Enqueue (opStack.Pop ());
								continue;
							}

							break;
						}

						opStack.Push (op);
					} else if (val == char.MaxValue) {
						break;					
					} else {
						throw new ArgumentOutOfRangeException ();
					}
				}
			}

			while (opStack.Count () > 0) {

				if (opStack.Peek ().Value == MathOperation.None) {
					throw new FormatException ();
				}


				outputQueue.Enqueue (opStack.Pop ());
			}

			return outputQueue;
		}

		private void LoadParameters(){
			string[] names = Vars.ToArray ();

			Parameters = new Dictionary<string, ParameterExpression> (names.Length);

			foreach (var n in names) {
				Parameters.Add (n, Expression.Parameter (typeof(double), n));
			}

		}

		public Equation ToGeneralForm(){
			//var result = Terms.Product ();

			return new Equation ();
		}

		/// <summary>
		/// Parse groupings in the specified equation.
		/// </summary>
		/// <param name="equation">Equation.</param>
		void Parse(string equation)
		{
			int start = equation.IndexOf ("(");
			int end;
			int count = 0;

			string eq = equation;

			if (start > -1) {
				for (int i = start + 1; i < equation.Length; i++) {
					if (equation [i] == '(') {
						count++;
					} else if (equation [i] == ')') {
						if (count > 0) {
							count--;
						} else if (count == 0) {
							// process
							var str = eq.Substring (start+1, i - start-1);
															
							Debug.Write ("Group: ");
							Debug.WriteLine (str);

							Groups.Add (Equation.Parse (str, Vars.ToArray ()));
							start = equation.IndexOf ("(", i);

							if (start > -1) {
								i = start;
							} else {
								break;
							}
						} else {
							throw new Exception ();
						}
					}
				}
			}

			FormattedString = eq;

			Flatten ();

			Debug.WriteLine ("-----");
		}

		public double Evaluate(double input)
		{
			if (Eval == null) {
				Eval = CompiledExpression.Compile ();
			}

			return Eval (input);
		}

		/// <summary>
		/// Lexes input, then parses into Expression
		/// </summary>
		void Parse()
		{
			var outputQueue = Lex (Text);
			var expStack = new Stack<MathToken> ();
			var termStack = new Stack<IEnumerable<Term>> ();
			var store = new List<Term> ();

			MathToken left = null, right = null;

			Stack<int> counts = new Stack<int> (); // used to count terms

			while (outputQueue.Count () > 0) {
				if (outputQueue.Peek () is MathOperatorToken) {
					var op = outputQueue.Dequeue () as MathOperatorToken;

					switch (op.Value) {
					case MathOperation.Add:
						right = expStack.Pop ();
						left = expStack.Pop ();

						expStack.Push (new MathToken () {
							expression = Expression.Add (left.expression, right.expression) 
						});
						break;
					case MathOperation.Subtract:
						right = expStack.Pop ();
						left = expStack.Pop ();

						expStack.Push (new MathToken () {
							expression = Expression.Subtract (left.expression, right.expression)
						});
						break;
					case MathOperation.Multiply:
						right = expStack.Pop ();
						left = expStack.Pop ();

						//var rightTerm = termStack.Pop ();
						//var leftTerm = termStack.Pop ();

						//store.Add (leftTerm * rightTerm);
						//termStack.Push (leftTerm * rightTerm);

						expStack.Push (new MathToken () {
							expression = Expression.Multiply (left.expression, right.expression)
						});
						break;
					case MathOperation.Divide:
						right = expStack.Pop ();
						left = expStack.Pop ();

						expStack.Push (new MathToken () {
							expression = Expression.Divide (left.expression, right.expression)
						});
						break;
					case MathOperation.Exponent:
						var exponent = termStack.Pop ();
						var baseTerm = termStack.Pop ();

						//termStack.Push (baseTerm ^ exponent);

						expStack.Push (new MathToken () {
							expression = Expression.Power (left.expression, right.expression)
						});
						break;
					case MathOperation.OpenGroup:
						store.Clear ();
						break;
					case MathOperation.CloseGroup:
						if (store.Count () > 0) {
							termStack.Push (store);
						}
						break;
					default:
						throw new Exception ();
					}
				} else {
					if (outputQueue.Peek () is MathOperandToken) {
						MathOperandToken op = outputQueue.Dequeue () as MathOperandToken;

						store.Add (new Term () {
							Coefficient = op.Value
						});

						expStack.Push (op);
					} else if (outputQueue.Peek () is MathVariableToken) {
						MathVariableToken op = outputQueue.Dequeue () as MathVariableToken;
						expStack.Push (new MathToken (){ expression = Parameters [op.Value] });

						store.Add (new Term () {
							Identifier = op.Value
						});
					}
				}
			}

			CompiledExpression = Expression.Lambda<Func<double,double>> (expStack.Pop ().expression, Parameters.Values);

/*			while (termStack.Count () > 0) {
 * 
			}*/

			if (expStack.Count () != 0) {
				throw new FormatException ();
			}

			//Parse (Text);
		}

		public static Equation Parse(string value, params string[] names)
		{
			Debug.WriteLine ("-----");
			Debug.Write ("Parsing: ");
			Debug.WriteLine (value);

			var e = new Equation ();
			e.Vars.AddRange (names);
			e.LoadParameters ();
			e.Text = value;

			e.Parse ();

			//Debug.WriteLine (e.Text);
			//Debug.WriteLine (e.ToString ());
			return e;
		}

		private static void ReadWhitespace(ref StringReader reader){
			char c = (char)reader.Peek ();
		
			while (c == ' ' || char.IsControl (c)) {
				reader.Read ();
				c = (char)reader.Peek ();
			}

			//Debug.Write ("Whitespace: ");
			//Debug.WriteLine (c);
		}

		private static MathOperatorToken ReadOperator(ref StringReader reader){
			char c = (char)reader.Peek ();
			MathOperation op = MathOperation.None;

			switch (c) {
			case '+':
				op = MathOperation.Add;
				break;
			case '-':
				op = MathOperation.Subtract;
				break;
			case '*':
				op = MathOperation.Multiply;
				break;
			case '/':
				op = MathOperation.Divide;
				break;
			case '^':
				op = MathOperation.Exponent;
				break;
			default:
				throw new FormatException ();
			}

			reader.Read ();

			return new MathOperatorToken (op);
		}

		private static MathOperandToken ReadOperand(ref StringReader reader){
			var sb = new StringBuilder ();
			var c = (char)reader.Peek ();
			double result;

			while (char.IsDigit (c) || c == '.') {
				sb.Append ((char)reader.Read ());
				c = (char)reader.Peek ();
			}

			if (!double.TryParse (sb.ToString (), out result)) {
				throw new FormatException ();
			}

			return new MathOperandToken (result){ expression = Expression.Constant (result, typeof(double)) };
		}

		private static MathVariableToken ReadVariable(ref StringReader reader){
			var sb = new StringBuilder ();
			var c = (char)reader.Peek ();

			while (char.IsLetter (c)) {
				sb.Append ((char)reader.Read ());
				c = (char)reader.Peek ();
			}

			//Debug.Write ("Variable: ");
			//Debug.WriteLine (sb.ToString ());

			return new MathVariableToken (sb.ToString ());
		}
	}
}