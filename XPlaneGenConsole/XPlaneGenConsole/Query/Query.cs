using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace XPlaneGenConsole
{
	public class Query<T>
		where T: BinaryDatapoint
	{
		public Query ()
		{
		}

		public IEnumerable<T> Data { get; set; }

		public IQueryable<T> Queryable
		{
			get { return Data.AsQueryable(); }
		}

		public string QueryString { get; set; }


		void Add(Type type, string name){

		}

		void Clear(){

		}

		void Remove(Type type, string name){

		}
	}
}