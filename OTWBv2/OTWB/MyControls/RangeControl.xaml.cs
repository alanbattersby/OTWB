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
    public sealed partial class RangeControl : UserControl
    {
        public RangeControl()
        {
            this.InitializeComponent();
        }

        public double RangeStartMax
        {
            get { return this.Start.Maximum; }
            set { this.Start.Maximum = value; }
        }

        public double RangeStartMin
        {
            get { return this.Start.Minimum; }
            set { this.Start.Minimum = value; }
        }

        public double RangeEndMax
        {
            get { return this.End.Maximum; }
            set { this.End.Maximum = value; }
        }

        public double RangeEndMin
        {
            get { return this.End.Minimum; }
            set { this.End.Minimum = value; }
        }

        public double RangeIncMax
        {
            get { return this.Inc.Maximum; }
            set { this.Inc.Maximum = value; }
        }

        public double RangeIncMin
        {
            get { return this.End.Minimum; }
            set { this.End.Minimum = value; }
        }
    }
}
