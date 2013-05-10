using OTWB.Spindle;
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
    public sealed partial class NewRosetteControl : UserControl
    {
        public Rosette NewRosette { get; set; }
        
        public event EventHandler<Rosette> RosetteChosen;

        public NewRosetteControl()
        {
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewRosette = new Ellipse();
            if (RosetteChosen != null)
                RosetteChosen(this, NewRosette);
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NewRosette = new Wave();
            if (RosetteChosen != null)
                RosetteChosen(this, NewRosette);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NewRosette = new Poly();
            if (RosetteChosen != null)
                RosetteChosen(this, NewRosette);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NewRosette = new SpurGear();
            if (RosetteChosen != null)
                RosetteChosen(this, NewRosette);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            NewRosette = new SquareWave();
            if (RosetteChosen != null)
                RosetteChosen(this, NewRosette);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            NewRosette = new Petal();
            if (RosetteChosen != null)
                RosetteChosen(this, NewRosette);
        }

       
    }
}
