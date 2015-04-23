using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPlaneGenConsole
{
    public class XPlaneHeader
    {
        public string Aircraft { get; set; }

        public string Tail { get; set; }

        public double Pressure { get; set; }

        public double Temperature { get; set; }
    }

    public class XPlaneDatapoint
    {
        /*
        COMM

        ACFT
        TAIL

        TIME
        DATE
        PRES
        TEMP
        WIND
        CALI


        WARN
        TEXT
        MARK
        EVNT

        DATA




        */

		public bool IsNoseGearDown{ get; set; }
		public bool IsLeftGearDown{ get; set; }
		public bool IsRightGearDown{ get; set; }


		public void SetThrottleRatio(params float[] values){
			
		}

		public void SetPropellerRPM(params byte[] values){

		}

		public void SetPropellerDegree(params short[] values){

		}

		public void SetN1(params byte[] values){ }
		public void SetN2(params byte[] values){ }
		public void SetFuelFlow(params float[] values){}
		public void SetTurboInletTemp(params short[] values){}
		public void SetExhaustGasTemp(params float[] values){}
		public void SetCylinderHeadTemp(params float[] values){}
    }
}