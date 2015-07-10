using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FDA
{
    public class Query
    {
        public static Query Create(Type type)
        {
            var generic = typeof(Query<>).MakeGenericType(type);            

            var query = new Query()
            {
                ElementType = type
            };

            return query;
        }

        public Type ElementType { get; set; }

        public Type GenericType
        {
            get { return ElementType.MakeGenericType(typeof(Query<>).MakeGenericType(ElementType)); }
        }

        public void Evaluate()
        {

        }

        public string QueryString { get; set; }

        public void AddWhereClause()
        {
         
        }
    }

	public class Query<T> : Query
		where T: BinaryDatapoint, new()
	{
		public IEnumerable<T> Data { get; set; }

		public IQueryable<T> Queryable
		{
			get { return Data.AsQueryable(); }
		}

		void Add(Type type, string name){

		}

		void Clear(){

		}

		void Remove(Type type, string name){

		}
	}
}