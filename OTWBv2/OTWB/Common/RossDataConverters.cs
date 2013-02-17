using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Geometric_Chuck.Common
{
   class RossEx1Converter : IValueConverter
   {
       object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
       {
           if (value is RossData)
               return (value as RossData).Ex1;
           else
               return string.Empty;
       }

       object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
       {
           throw new NotImplementedException();
       }
   }
   class RossEx2Converter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).Ex2;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossPHi1Converter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).Phi1;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossPHi2Converter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).Phi2;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossPHi3Converter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).Phi3;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossFlConverter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).Fl;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossFrConverter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).Fr;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossKConverter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).K;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossLConverter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).L;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossMConverter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).M;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossNConverter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).N;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
   class RossV4Converter : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
            {
                if (value == null)
                    return null;
                if (value is RossData)
                    return (value as RossData).V4;
                else
                    return string.Empty;
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }

   class RossStage2VisibilityConverter : IValueConverter
   {
       object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
       {
           if (value == null)
               return Visibility.Collapsed;
           else if (value is RossData)
               return ((value as RossData).Ex2 > 0) ? Visibility.Visible : Visibility.Collapsed;
           else
               return Visibility.Collapsed;
           
       }

       object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
       {
           throw new NotImplementedException();
       }
   }
}
