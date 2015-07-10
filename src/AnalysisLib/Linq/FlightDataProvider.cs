using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace XPlaneGenConsole
{
	public class FlightDataProvider : IQueryProvider
	{
		public IQueryable CreateQuery(Expression exp)
		{
			Type element = TypeSystem.GetElementType (exp.Type);
			throw new NotImplementedException ();
		}

		public IQueryable<T> CreateQuery<T>(Expression exp){
			throw new NotImplementedException ();
		}

		public object Execute(Expression exp){
			throw new NotImplementedException ();
		}

		public T Execute<T>(Expression exp){
			throw new NotImplementedException ();
		}
	}

	internal static class TypeSystem
	{
		internal static Type GetElementType(Type seqType){

			Type iEnum = FindIEnumerable (seqType);

			return iEnum == null ? seqType : iEnum.GetGenericArguments ().First ();
		}

		private static Type FindIEnumerable(Type seqType)
		{
			if (seqType == null || seqType == typeof(string))
				return null;
			if (seqType.IsArray)
				return typeof(IEnumerable<>).MakeGenericType (seqType.GetElementType ());
			if (seqType.IsGenericType) {
				foreach (Type arg in seqType.GetGenericArguments()) {
					Type ienum = typeof(IEnumerable<>).MakeGenericType (arg);
					if (ienum.IsAssignableFrom (seqType)) {
						return ienum;
					}
				}
			}

			Type[] ifaces = seqType.GetInterfaces();
			if (ifaces != null && ifaces.Length > 0) {
				foreach (Type iface in ifaces) {
					Type ienum = FindIEnumerable(iface);
					if (ienum != null) return ienum;
				}
			}

			if (seqType.BaseType != null && seqType.BaseType != typeof(object)) {
				return FindIEnumerable(seqType.BaseType);
			}
			return null;
		}
	}
}