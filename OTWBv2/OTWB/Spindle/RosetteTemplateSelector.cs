using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Geometric_Chuck.Spindle
{
    public class RosetteTemplateSelector : DataTemplateSelector
    {

        public DataTemplate TemplateA { get; set; }
        public DataTemplate TemplateB { get; set; }
        public DataTemplate TemplateC { get; set; }
        public DataTemplate TemplateD { get; set; }

        protected override DataTemplate SelectTemplateCore
            (object item, DependencyObject container)
        {
            if (item is Ellipse)
            {
                return TemplateA;
            }
            else if (item is Wave)
            {
                return TemplateB;
            }
            else if (item is Poly)
            {
                return TemplateC;
            }
            else if (item is SpurGear)
            {
                return TemplateD;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
