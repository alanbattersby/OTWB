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
    public sealed partial class CartesianGridControl : UserControl
    {

        public CartesianGridControl()
        {
            this.InitializeComponent();
            this.XRange.RangeStartMin = -300;
            this.YRange.RangeStartMin = -20;

            this.XRange.RangeIncMin = 5;
            this.YRange.RangeIncMin = 5;

            this.XRange.RangeEndMax= 300;
            this.YRange.RangeEndMax = 20;

          
        }

    }
}
