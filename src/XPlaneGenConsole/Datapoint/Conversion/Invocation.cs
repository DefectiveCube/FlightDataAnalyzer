using System;

namespace XPlaneGenConsole
{
	public class Invocation
	{
		public bool IsAction{ get { return Type == typeof(void); } }
		public bool IsFunc{ get { return Type != typeof(void); } }
		public bool IsNull{ get { return Value == null; } }
		public Type Type{ get; private set; }
		public Type[] Arguments{ get; private set; }

		public dynamic Value{ get; private set; }

		protected Invocation(){
			Arguments = new Type[]{ };
			Type = typeof(void);
		}

		protected Invocation(Type type, params Type[] args) {
			Type = type;
			Arguments = args;
		}

		protected bool Verify(object[] args)
		{
			if (args.Length != Arguments.Length) {
				throw new ArgumentException ("Incorrect number of arguments");
			}

			var passed = true;

			for (int i = 0; i < args.Length; i++) {
				if (args [i].GetType () != Arguments [i]) {
					passed = false;
					break;
				}
			}

			return passed;
		}

		public virtual object Invoke(params object[] args){
			if (!Verify (args)) {
				throw new ArgumentException ("Passed wrong type");
			}

			try {
				switch (args.Length) {
				case 0:
					return Value.Invoke ();
				case 1:
					return Value.Invoke (args [0]);
				case 2:
					return Value.Invoke (args [0], args [1]);
				case 3:
					return Value.Invoke (args [0], args [1], args [2]);
				case 4:
					return Value.Invoke (args [0], args [1], args [2], args [3]);
				default:
					return Value.DynamicInvoke (args);
				}
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
				Console.WriteLine (ex.StackTrace);
				return null;
			}
		}

		public static Invocation Use(Action action){
			return new Invocation () {
				Value = action,
			};
		}

		public static Invocation Use<T>(Action<T> action){
			return new Invocation (typeof(void), typeof(T)) {
				Value = action,
			};
		}

		public static Invocation Use<T1,T2>(Action<T1,T2> action){
			return new Invocation (typeof(void), typeof(T1), typeof(T2)) {
				Value = action
			};
		}

		public static Invocation Use<T>(Func<T> func){
			return new Invocation (typeof(T)) {
				Value = func
			};
		}

		public static Invocation Use<T,TResult>(Func<T,TResult> func){
			return new Invocation<T,TResult> (typeof(TResult), typeof(T)) {
				Value = func
			};
		}

		public static Invocation Use<T1,T2,TResult>(Func<T1,T2,TResult> func){
			return new Invocation<T1,T2,TResult> (typeof(TResult), typeof(T1), typeof(T2)) {
				Value = func
			};
		}
	}

	public class Invocations<T,U,V> 
	{

		public static T Get(string value){
			value.As<V> ();


			return default(T);
		}
	}

	public class Invocation<T,TResult> : Invocation
	{
		internal Invocation() : base(){}

		internal Invocation(Type type, params Type[] args) : base(type,args){

		}

		public override object Invoke (params object[] args)
		{
			return (TResult)Value.Invoke ((T)args [0]);
		}
	}

	public class Invocation<T1,T2,TResult> : Invocation
	{
		internal Invocation() : base(){
		}

		internal Invocation(Type type, params Type[] args): base(type,args){
		}

		public override object Invoke (params object[] args)
		{
			if (!Verify (args)) {
				throw new ArgumentException ();
			}

			return (TResult)Value.Invoke ((T1)args [0], (T2)args [1]);
		}
	}
}