using System;
using Windows.UI.Xaml.Data;

namespace OTWB.Common
{
    /// <summary>
    /// Value converter that translates true to false and vice versa.
    /// </summary>
    public sealed class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }

    public sealed class Minus1Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int)
                return (int)value - 1;
            if (value is double)
                return (double)value - 1.0;
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int)
                return (int)value + 1;
            if (value is double)
                return (double)value + 1.0;
            else
                return value;
        }
    }
}
