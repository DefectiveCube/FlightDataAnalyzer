using System;
using System.Windows;
using System.Windows.Data;

namespace XPlaneWPF.Converters
{
    /// <summary>
    /// Implements the conversion between a <code>bool?</code> 
    /// and the <code>Visibility</code> enum:
    /// null  = Hidden
    /// false = Collapsed
    /// true  = Visible
    /// </summary>
    public class BoolToWindowStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? visible = (bool?)value;
            if (visible.HasValue)
                return visible.Value ? WindowState.Maximized : WindowState.Normal;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            WindowState state = (WindowState)value;

            switch (state)
            {
                case WindowState.Maximized:
                    return true;
                case WindowState.Minimized:
                    return null;
                case WindowState.Normal:
                    return false;
            }
            return null;
        }
    }
}