using System;
using System.Linq;

namespace XPlaneGenConsole
{
	/// <summary>
	/// Represents a datapoint using binary types to store information
	/// </summary>
	public abstract class BinaryDatapoint : Datapoint
	{ 	
		private Random r;

		public BinaryDatapoint(int bytes)
		{
			//BYTES_COUNT = bytes;
			r = new Random();
		}

		protected int Key { get; set; }
		protected virtual int Fields{ get; set; }
		protected virtual int Bytes{ get; set ;}

		/// <summary>
		/// True, if datapoint has usable data
		/// </summary>
		public bool IsValid { get; set; }

		public virtual int Flight { get; internal set; }

		public virtual int Timestamp { get; internal set; }

		public virtual DateTime DateTime { get; internal set;}

		public virtual byte[] Data { get; internal set; }

		public virtual void Load(byte[] data) { 
			// TODO: ensure data is valid
			Data = data;

			SetBytes ();
		}

		public virtual void Load(string value){
			var values = value.Split (',');

			Load (values);
		}

		public virtual void Load(string[] values){

			// Two conditions to verify a valid row
			// 1. There must a be specific amount of CSV fields per record (there is a constant value (SIZE) defined in each type of datapoint)
			// 2. All fields after the 3rd element should be defined. "-" signifies a null value

			var IsValid = values.Length == Fields && !values.Skip (3).All (v => string.IsNullOrEmpty (v) || v.Equals ("-"));

			// If the row is 4 fields long, then that is a new flight
			if (!IsValid) {
				if (values.Length == 4) {
					Key = r.Next ();
					//FlightTimes.Add (ParseDateTime (values [1] + " " + values [2]));
				}               


				return;
			}

			Flight = Key;

			(this as IDatapointParse).Parse (values);
		}

		internal virtual byte[] GetBytes (){return new byte[]{};}

		internal virtual void SetBytes (){}
	}
}