using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OTWB.Coordinates
{
    public class CoordTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PointTemplate { get; set; }
        public DataTemplate CylindricalTemplate { get; set; }
        public DataTemplate SphericalTemplate { get; set; }
        public DataTemplate CartesianTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore
            (object item, DependencyObject container)
        {
            if (item is Point)
            {
                return PointTemplate;
            }
            else if (item is Cylindrical)
            {
                return CylindricalTemplate;
            }
            else if (item is Spherical)
            {
                return SphericalTemplate;
            }
            else if (item is Cartesian)
            {
                return CartesianTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
