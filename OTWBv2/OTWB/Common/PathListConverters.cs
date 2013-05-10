using OTWB.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace OTWB.Common
{
    public class PatternPathListNameConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {

            Debug.WriteLine("PatternPathListNameConverter Converting {0}", value.GetType());
            if (value == null)
                return null;
            if (value is ShapeCollection)
            {
                //Debug.WriteLine("Converting list length {0} Type {1}", 
                //    (value as PolygonCollection).Count, (value as PolygonCollection).PatternType);
                return (value as ShapeCollection).PathNames;
            }
            return "oops";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class PointIndexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "Error";
            if (value is Polygon)
            {
                Polygon p = value as Polygon;
                List<int> indxs = new List<int>(p.Points.Count);
                foreach (Point pt in p.Points)
                    indxs.Add(p.Points.IndexOf(pt));
                return indxs;
            }
            return "oops";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class PathIndexConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {

            List<string> indxs = new List<string>();
            if ((value == null) || (!(value is ShapeCollection)))
            {
                indxs.Add("Error");
            }
            else
            {
                ShapeCollection pc = value as ShapeCollection;
                if (pc.Count == 0)
                    indxs.Add("Empty Path");
                else
                    foreach (Shape p in pc.Shapes)
                        indxs.Add(p.Name);
            }
            return indxs;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class PolySizeConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {

            if ((value == null) || (!(value is Polygon)))
            {
                return 0;
            }
            else
            {
                return (value as Polygon).Points.Count;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ShapePointsConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {

            if (value is ShapeCollection)
            {
                ShapeCollection pc = value as ShapeCollection;
                return GetPoints(pc[0]);
            }
            return new PointCollection();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        //TODO add code for other shapes
        PointCollection GetPoints(Shape s)
        {
            if (s is Polygon)
                return (s as Polygon).Points;
            else
                return new PointCollection();
        }
    }
}
