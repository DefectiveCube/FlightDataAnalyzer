﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace XPlaneGenConsole
{
	public class QueryBuilder
	{
		private static readonly Dictionary<string,QueryOperations> ops;

		static QueryBuilder()
		{
			ops = QueryOperator.Operations;

			if (ops == null || ops.Count == 0) {
				throw new Exception ("No Operations Loaded");
			}
		}

		private static IEnumerable<ParameterExpression> BuildParameters(params Type[] parameters)
		{
			var names = 
				from l in Enumerable.Range (97, parameters.Count ())
				select new {
					Type = parameters [l - 97],
					Value = new String ((char)l, 1)
				};

			foreach (var p in names) {
				yield return Expression.Parameter (p.Type, p.Value);
			}

			yield break;

		}
		public static Expression Build(string query, params Type[] parameters)
		{
			var lambda = Expression.Lambda (Build (query), BuildParameters (parameters).ToArray ());

			return lambda;
		}

		public static Expression Build(string query)
		{
			var builder = new QueryBuilder (query);
			var stack = new Stack<QueryToken> ();
			var queue = new Queue<QueryToken> ();
			var eval = new Stack<Expression> ();
			var last = TokenType.None;
			QueryToken value;

			while (!builder.IsEmpty) {

				// Operands should not appear consecutively
				if (last != TokenType.Operand) {
					value = builder.ReadOperand ();

					if (value.Token == TokenType.Operand) {
						queue.Enqueue (value);
						last = value.Token;
						continue;
					}
				}

				// Operators should not appear consecutively
				if (last != TokenType.Operator) {
					value = builder.ReadOperator ();

					if (value.Token == TokenType.Operator) {
						stack.Push (value);
						last = value.Token;
						continue;
					}
				}

				value = builder.ReadGroupingSymbol ();

				if (value.Token == TokenType.Grouping) {
					if (value.ToString () == "(") {
						stack.Push (value);
						last = value.Token;
					} else {
						QueryToken q;
						bool found = false;

						while (stack.Count > 0) {
							q = stack.Pop ();

							if (q.Token != TokenType.Grouping) {
								queue.Enqueue (q);
							} else {
								found = true;
								break;
							}
						}

						if (!found) {
							throw new ArgumentException ("Missing right parenthesis");
						}
					}

					continue;
				}

				throw new Exception ("Unrecognized sequence");
			}
				

			while (stack.Count > 0) {
				var v = stack.Pop ();

				if (v.Token != TokenType.Grouping) {
					queue.Enqueue (v);
				} else if (v.ToString () == "(") {
					throw new ArgumentException ("Mismatched Parenthesis");
				}
			}

			while (queue.Count > 0) {
				var e = queue.Dequeue ();

				if (e.Token == TokenType.Operand) {
					eval.Push (e.GetExpression ());
				} else if (e.Token == TokenType.Operator) {
					var right = eval.Pop ();

					var left = eval.Pop ();

					var exp = e.GetExpression (left, right);

					eval.Push (exp);
				} else {
					throw new Exception ("Parse Error");
				}
			}

			if (eval.Count () == 1) {
				return eval.First ();
			} else {
				throw new ArgumentException ("Invalid Expression");
			}
		}

		readonly string Symbols;
		//readonly string[] FunctionNames;
		//readonly string[] ConstantNames;
		StringBuilder sb;
		readonly string initialState; 

		private QueryBuilder(string query)
		{
			initialState = query;

			Symbols = @"+-*/^!|%&=><'""";
			//FunctionNames = new[]{ "FLOOR", "CEILING", "ROUND", "COS", "SIN", "TAN" };
			//ConstantNames = new[]{ "PI", "E" };

			sb = new StringBuilder (query.Trim ());
		}

		bool IsEmpty{ get { return sb.Length == 0; } }

		int Peek()
		{
			return !IsEmpty ? sb [0] : -1;
		}

		void Next()
		{
			while(!IsEmpty && Peek() == ' ')
			{
				sb.Remove (0, 1);
			}
		}

		void Reset()
		{
			sb = new StringBuilder (initialState.Trim ());
		}

		TokenType Evaluate()
		{
			return Evaluate (Peek ());
		}

		TokenType Evaluate(int c)
		{
			return Evaluate ((char)c);
		}

		TokenType Evaluate(char c)
		{
			if(Symbols.Contains(c)){
				return TokenType.Operator;
			}

			if (Char.IsDigit (c)) {
				return TokenType.Operand;
			}

			if (c == '(' || c == ')') {
				return TokenType.Grouping;
			}

			if (c == ',') {
				return TokenType.Separator;
			}

			return TokenType.None;
		}


		char Read()
		{
			var c = sb [0];

			sb.Remove (0, 1);

			Next ();

			return c;
		}

		IEnumerable<char> Read(int pos)
		{			
			if (IsEmpty) {
				yield break;
			}

			if (pos < 0 || pos + 1 > sb.Length) {
				throw new ArgumentOutOfRangeException ("pos");
			}

			for (int i = 0; i <= pos; i++) {
				yield return sb [i];
			}

			sb.Remove (0, pos + 1);
			Next ();

			yield break;
		}

		QueryToken ReadOperand()
		{
			Next ();

			int pos = -1;

			for (int i = 0; i < sb.Length; i++) {
				if (Evaluate (sb [i]) == TokenType.Operand || sb [i] == '.') {
					pos = i;
				} else {
					break;
				}
			}

			if (pos == 0 && Peek () == '.') {
				throw new ArgumentException ("A decimal point must be either preceded or succeeded by a digit");
			}
				
			if (pos < 0) {
				return QueryToken.Empty;
			}

			var str = new string (Read (pos).ToArray ());

			//Console.WriteLine ("Operand: '{0}'", str);

			if (str != string.Empty) {
				if (str.IndexOf (".") > -1) {
					return new QueryToken<double> (double.Parse (str), TokenType.Operand);
				} else {
					return new QueryToken<int> (int.Parse (str), TokenType.Operand);
				}
			} else {
				return QueryToken.Empty;						
			}
		}

		QueryToken ReadOperator()
		{
			Next ();

			if (Evaluate() != TokenType.Operator) {
				return QueryToken.Empty;
			}

			// Some operators are multiple-characters (e.g. logical-* (and,or))

			// Read until next character is not an operator
			var sb = new StringBuilder ();

			while (Evaluate () == TokenType.Operator) {
				sb.Append (Read ());
			}

			var option = QueryOperator.Match (sb.ToString ());

			//Console.WriteLine ("Operator: {0} -> {1}", sb.ToString (), option);

			return option != QueryOperations.None ? new QueryOperator (option) : QueryToken.Empty;
		}

		QueryToken ReadGroupingSymbol()
		{
			Next ();

			var e = Evaluate ();

			if (e == TokenType.Grouping) {				
				return new QueryToken<char> (Read (), TokenType.Grouping);
			}

			return QueryToken.Empty;
		}

		QueryToken ReadIdentifier()
		{
			return QueryToken.Empty;
		}

		QueryToken ReadField()
		{
			Next ();

			var e = Evaluate ();

			var sb = new StringBuilder ();

			while (e == TokenType.Field) {
				sb.Append (Read ());
			}

			return QueryToken.Empty;
		}

		QueryToken ReadProperty()
		{
			return QueryToken.Empty;
		}

		QueryToken ReadFunction()
		{
			Next ();

			var e = Evaluate ();

			var sb = new StringBuilder ();

			while (e == TokenType.Function) {
				sb.Append (Read ());
			}

			// Read functions arguments

			return QueryToken.Empty;
		}
	}
}