using System;
using System.Globalization;
using UnitsNet;
using UnitsNet.Units;
//using FDA;
using FDA.Attributes;

[assembly: Datapoint(typeof(Prototype.EngineDatapoint))]

namespace Prototype
{
    /// <summary>
    /// Prototype Datapoint for Engine System
    /// </summary>  
   
    [CsvRecord(FIELDS_COUNT)]
    public sealed class EngineDatapoint : Datapoint
    {
        public const int BYTES_COUNT = 122; // is this usable anymore?
        public const int FIELDS_COUNT = 33;

        [CsvField(3)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(24, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Oil")]
        public Temperature OilTemperature { get; set; }

        [CsvField(4)]
        [Format(PressureUnit.Undefined)]
        [Storage(25, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Oil")]
        public Pressure OilPressure { get; set; }

        [CsvField(5)]
        [Format(RotationalSpeedUnit.RevolutionPerMinute)]
        [Storage(26, typeof(short))]
        [Graph(GraphData.Continuous)]
        [Group("Engine")]
        public RotationalSpeed EngineRPM { get; set; }

        [CsvField(6)]
        [Format(PressureUnit.Undefined)]
        [Storage(3, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Engine")]
        public Pressure EngineManifold { get; set; }

        [CsvField(7)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(4, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Engine")]
        public Temperature EngineTIT { get; set; }

        [CsvField(8)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(5, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Cylinder Head")]
        public Temperature CHT_1 { get; set; }

        [CsvField(9)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(6, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Cylinder Head")]
        public Temperature CHT_2 { get; set; }

        [CsvField(10)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(7, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Cylinder Head")]
        public Temperature CHT_3 { get; set; }

        [CsvField(11)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(8, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Cylinder Head")]
        public Temperature CHT_4 { get; set; }

        [CsvField(12)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(9, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Cylinder Head")]
        public Temperature CHT_5 { get; set; }

        [CsvField(13)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(10, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Cylinder Head")]
        public Temperature CHT_6 { get; set; }

        [CsvField(14)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(11, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Exhaust")]
        public Temperature EGT_1 { get; set; }

        [CsvField(15)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(12, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Exhaust")]
        public Temperature EGT_2 { get; set; }

        [CsvField(16)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(13, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Exhaust")]
        public Temperature EGT_3 { get; set; }

        [CsvField(17)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(14, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Exhaust")]
        public Temperature EGT_4 { get; set; }

        [CsvField(18)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(15, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Exhaust")]
        public Temperature EGT_5 { get; set; }

        [CsvField(19)]
        [Format(TemperatureUnit.DegreeFahrenheit)]
        [Storage(16, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Exhaust")]
        public Temperature EGT_6 { get; set; }

        [CsvField(20)]
        [Format(RatioUnit.Percent)]
        [Storage(17, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Engine")]
        public Ratio EnginePercentPower { get; set; }

        [CsvField(21)]
        [Format(VolumeUnit.UsGallon)]
        [Storage(18, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Fuel")]
        public Volume FuelFlow { get; set; }

        [CsvField(22)]
        [Format(VolumeUnit.UsGallon)]
        [Storage(19, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Fuel")]
        public Volume FuelUsed { get; set; }

        [CsvField(23)]
        [Format(VolumeUnit.UsGallon)]
        [Storage(20, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Fuel")]
        public Volume FuelRemaining { get; set; }

        [CsvField(25)]
        [Format(RatioUnit.Undefined)]
        [Storage(21, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Fuel")]
        public Ratio FuelEconomy { get; set; }

        [CsvField(26)]
        [Format(ElectricCurrentUnit.Ampere)]
        [Storage(28, typeof(byte))]
        [Graph(GraphData.Continuous)]
        [Group("Current")]
        public ElectricCurrent AlternatorCurrent_1 { get; set; }

        [CsvField(27)]
        [Format(ElectricCurrentUnit.Ampere)]
        [Storage(29, typeof(byte))]
        [Graph(GraphData.Continuous)]
        [Group("Current")]
        public ElectricCurrent AlternatorCurrent_2 { get; set; }

        [CsvField(28)]
        [Format(ElectricCurrentUnit.Ampere)]
        [Storage(27, typeof(ushort))]
        [Graph(GraphData.Continuous)]
        [Group("Current")]
        public ElectricCurrent BatteryCurrent { get; set; }

        [CsvField(29)]
        [Format(ElectricPotentialUnit.Volt)]
        [Storage(22, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Voltage")]
        public ElectricPotential BusVoltage_1 { get; set; }

        [CsvField(30)]
        [Format(ElectricPotentialUnit.Volt)]
        [Storage(23, typeof(float))]
        [Graph(GraphData.Continuous)]
        [Group("Voltage")]
        public ElectricPotential BusVoltage_2 { get; set; }

        [CsvField(31)]
        [Format(NumberStyles.HexNumber)]
        [Storage(30, typeof(byte))]
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte DiscreteInputs { get; set; }

        [CsvField(32)]
        [Format(NumberStyles.HexNumber)]
        [Storage(31, typeof(byte))]
        [Graph(GraphData.Discrete)]
        [Group("Flags")]
        public byte DiscreteOutputs { get; set; }
    }
}