using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using UnitsNet;

namespace XPlaneWPF.Converters
{
    [ValueConversion(typeof(double), typeof(Temperature))]
    public class TemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert Double to Temperature
            Double num = (Double)value;
            Temperature temp = new Temperature(num);


            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert Temperature to Double
            Temperature temp = (Temperature)value;

            if (parameter != null)
            {

            }

            return temp.Kelvins;
        }
    }
}