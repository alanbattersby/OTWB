using Geometric_Chuck.Spindle;
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

namespace Geometric_Chuck.MyControls
{
    public sealed partial class NewRosetteFlyoutControl : UserControl
    {
        public Rosette NewRosette { get; set; }

        public NewRosetteFlyoutControl()
        {
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewRosette = new Ellipse();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NewRosette = new Wave();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NewRosette = new Poly();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NewRosette = new SpurGear();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NewRosette = new SquareWave();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            NewRosette = new Petal();
        }
    }
}
