using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UnitsNet;
using UnitsNet.Units;
using XPlaneWPF.Models;

namespace XPlaneWPF.Providers
{
    public class UnitTypeInfoProvider : DataSourceProvider
    {
        private List<UnitTypeInfo> infoList = new List<UnitTypeInfo>();

        public UnitTypeInfo[] Info { 
            get { return infoList.ToArray(); }
        }

        protected override void BeginQuery()
        {
            infoList.Add(new UnitTypeInfo(typeof(Acceleration), typeof(AccelerationUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Angle), typeof(AngleUnit)));
            infoList.Add(new UnitTypeInfo(typeof(ElectricCurrent), typeof(ElectricCurrentUnit)));
            infoList.Add(new UnitTypeInfo(typeof(ElectricPotential), typeof(ElectricPotentialUnit)));
            infoList.Add(new UnitTypeInfo(typeof(ElectricResistance), typeof(ElectricResistanceUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Frequency), typeof(FrequencyUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Ratio), typeof(RatioUnit)));
            infoList.Add(new UnitTypeInfo(typeof(RotationalSpeed), typeof(RotationalSpeedUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Speed), typeof(SpeedUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Temperature), typeof(TemperatureUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Torque), typeof(TorqueUnit)));
            infoList.Add(new UnitTypeInfo(typeof(Volume), typeof(VolumeUnit)));

            base.OnQueryFinished(Info);
        }
    }
}