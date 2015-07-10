using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FDA
{
	[Obsolete()]
    public class QueryExpression<T>
    {
        public QueryExpression() { }

        public QueryExpression(string path) { }

        public QueryExpression(IEnumerable<string> paths) { }

        public IEnumerable<T> Data { get; set; }

        public IQueryable<T> Query
        {
            get { return Data.AsQueryable(); }
        }

		public string QueryString { get; set; }

        public IEnumerable<T> Results()
        {
            foreach (var datapoint in Evaluate())
            {
                yield return datapoint;
            }

            yield break;
        }

        public IQueryable<T> Evaluate()
        {
            var pe = Expression.Parameter(typeof(T), "flight");

            var left = Expression.Property(pe, typeof(T).GetProperty("VerticalSpeed"));
            var right = Expression.Constant((short)0);
            //var exp = Expression.GreaterThan(left, right);


            var exp = Expression.MakeBinary(ExpressionType.GreaterThan, left, right);

			//Expression.

            var lamdba = Expression.Lambda<Func<T, bool>>(exp, new ParameterExpression[] { pe });
            var whereCall = Expression.Call(typeof(Queryable), "Where", new Type[] { Query.ElementType }, Query.Expression, lamdba);

            return Query.Provider.CreateQuery<T>(whereCall);
        }
    }
}
