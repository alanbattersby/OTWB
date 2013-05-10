using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace OTWB.Common
{
    class PathListIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<int> indxs = new List<int>();
            if (value is IList)
            {
                int l = (value as IList).Count;
                for (int i = 0; i < l; i++)
                    indxs.Add(i);
            } 
            return indxs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
