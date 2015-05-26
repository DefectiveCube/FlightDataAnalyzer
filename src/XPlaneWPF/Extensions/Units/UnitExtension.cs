using System;
using System.Windows.Markup;
using UnitsNet;
using UnitsNet.Units;

namespace XPlaneWPF.Extensions.Units
{
    public abstract class UnitExtension<U> : MarkupExtension
        where U : struct
    {
        [ConstructorArgument("Value")]
        public double Value { get; set; }

        [ConstructorArgument("Unit")]
        public U Unit { get; set; }
    }

    public class AngleExtension : UnitExtension<AngleUnit>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Angle.From(Value, Unit);
        }
    }

    public class AccelerationExtension : UnitExtension<AccelerationUnit>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Acceleration.From(Value, Unit);
        }
    }

    public class FrequencyExtension : UnitExtension<FrequencyUnit>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Frequency.From(Value, Unit);
        }
    }

    public class LengthExtension : UnitExtension<LengthUnit>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Length.From(Value, Unit);
        }
    }

    public class TemperatureExtension : UnitExtension<TemperatureUnit>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Temperature.From(Value, Unit);
        }
    }

    public class TorqueExtension : UnitExtension<TorqueUnit>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Torque.From(Value, Unit);
        }
    }

    public class VolumeExtension : UnitExtension<VolumeUnit>
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Volume.From(Value, Unit);
        }
    }
}