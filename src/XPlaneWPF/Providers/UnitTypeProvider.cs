using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UnitsNet;
using UnitsNet.Units;
using XPlaneWPF.Models;

namespace XPlaneWPF.Controls
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
            infoList.Add(new UnitTypeInfo(typeof(Temperature), typeof(TemperatureUnit)));

            base.OnQueryFinished(Info);
        }
    }
}