using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OTWB.MyControls
{
    public sealed partial class GridControl : UserControl
    {

        public GridControl()
        {
            this.InitializeComponent();
            this.RadialRange.RangeStartMin = 0;
            this.RadialRange.RangeStartMax = 299;

            this.RadialRange.RangeEndMax= 300;

            this.AngleRange.RangeStartMin = 0;
            this.AngleRange.RangeStartMax = 359;
            this.AngleRange.RangeEndMax = 360;
        }

    }
}
