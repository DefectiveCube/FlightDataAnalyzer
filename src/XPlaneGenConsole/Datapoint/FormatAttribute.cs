﻿using System;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneGenConsole
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class FormatAttribute : Attribute
    {
        public readonly string format;
        public readonly Type type;
        public readonly int value;
        public Func<double, double> conversion;

        private FormatAttribute(Type type, string conversion, int value)
        {
            this.type = type;
            this.value = value;

            // Create expression from string

            if(value == 0 && conversion == string.Empty)
            {
                this.conversion = f => f;
            }
        }        

        public bool Defined
        {
            get { return value != 0; }
        }


        public FormatAttribute(AccelerationUnit unit, string conversion = "", string format = "") : this(typeof(AccelerationUnit),conversion,(int)unit)
        {

        }

        public FormatAttribute(AngleUnit unit, string conversion = "", string format = "") : this(typeof(AngleUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(ElectricCurrentUnit unit, string conversion = "", string format = "") : this(typeof(ElectricCurrentUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(ElectricPotentialUnit unit, string conversion = "", string format = "") : this(typeof(ElectricPotentialUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(LengthUnit unit, string conversion = "", string format = "") : this(typeof(LengthUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(MassUnit unit, string conversion = "", string format = "") : this(typeof(MassUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(PressureUnit unit, string conversion = "", string format = "") : this(typeof(PressureUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(RatioUnit unit, string conversion = "", string format = "") : this(typeof(RatioUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(RotationalSpeedUnit unit, string conversion = "", string format = "") : this(typeof(RotationalSpeedUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(SpeedUnit unit, string conversion = "", string format = "") : this(typeof(SpeedUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(TemperatureUnit unit, string conversion = "", string format = "") : this(typeof(TemperatureUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(TorqueUnit unit, string conversion = "", string format = "") : this(typeof(TorqueUnit), conversion, (int)unit)
        {

        }

        public FormatAttribute(VolumeUnit unit, string conversion = "", string format = "") : this(typeof(VolumeUnit), conversion, (int)unit)
        {

        }
    }
}