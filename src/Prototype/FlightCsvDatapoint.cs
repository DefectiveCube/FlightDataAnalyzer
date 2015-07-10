using System;
using System.Collections.Generic;
using System.Linq;
using FDA;

namespace Prototype
{
    /*public class FlightCsvDatapoint : CsvDatapoint<FlightCsvDatapoint>
    {
        private const int FIELDS_COUNT = 30;

        private string[] values;

        public FlightCsvDatapoint()
        {
            Fields = FIELDS_COUNT;
        }

        public override IEnumerable<string> Value
        {
            get { return values; }
        }

        public override void Load(string value)
        {
            Load(value.Split(','));
        }

        public override void Load(string[] values)
        {
            // Two conditions to verify a valid row
            // 1. There must a be specific amount of CSV fields per record (there is a constant value defined in each type of datapoint)
            // 2. All fields after the 3rd element should be defined. "-" signifies a null value

            IsValid = values.Length == Fields && !values.Skip(3).All(v => string.IsNullOrEmpty(v) || v.Equals("-"));

            // If the row is 4 fields long, then that is a new flight
            if (!IsValid)
            {
                if (values.Length == 4)
                {
                    Key = r.Next();
                }

                return;
            }

            Flight = Key;

            this.values = values;
        }

        public void Parse(string[] values)
        {
            /*Timestamp = values[0].AsInt();
            DateTime = values[1].AsDateTime().Add(values[2].AsTimeSpan());
            NormalAcceleration = values[3].AsFloat();
            LongitudinalAcceleration = values[4].AsFloat();
            LateralAcceleration = values[5].AsFloat();
            ADAHRUsed = values[6].Equals("0") || string.IsNullOrWhiteSpace(values[6]); // can this be improved?
            AHRSSStatus = values[7].GetHexBytes<byte>().FirstOrDefault();
            Heading = values[8].AsFloat();
            Pitch = values[9].AsFloat();
            Roll = values[10].AsFloat();
            FlightDirectorPitch = values[11].AsFloat();
            FlightDirectorRoll = values[12].AsFloat();
            HeadingRate = values[13].AsFloat();
            PressureAltitude = values.AsShort(14);
            IndicatedAirspeed = values[15].AsByte();//ParseByte (values [15]);
            TrueAirspeed = values[16].AsByte();//ParseByte (values [16]);
            VerticalSpeed = values.AsShort(17);
            GPSLatitude = values[18].AsFloat();
            GPSLongitude = values[19].AsFloat();
            BodyYawRate = values[20].AsFloat();
            BodyPitchRate = values[21].AsFloat();
            BodyRollRate = values[22].AsFloat();
            IRUStatus = values[23].GetHexBytes<byte>().FirstOrDefault();
            MPUStatus = values[24].GetHexBytes<byte>().FirstOrDefault();
            ADCStatus = values[25].GetHexBytes<byte>().FirstOrDefault();
            AHRSSeq = values[26].GetHexBytes<byte>().FirstOrDefault();
            ADCSeq = values[27].AsByte();//ParseByte (values [27]);
            AHRSStartupMode = values[28].AsByte();//ParseByte (values [28]);           
        }
    }*/
}
