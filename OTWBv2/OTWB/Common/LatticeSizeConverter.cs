using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace OTWB.Common
{
    class LatticeSizeConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return 0;
            else
            {
                int p = int.Parse(parameter as string);
                double v = ((int)value * 1.0) / 2;
                double d = Math.Floor(v);
                return (int)d * p;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

   
}
