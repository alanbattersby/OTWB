using OTWB.Common;
using OTWB.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OTWB.Profiles
{
    public sealed partial class ProfileDisplay : UserControl
    {
        private const string XAXIS = "XAXIS";
        private const string YAXIS = "YAXIS";

        Profile _profile;
        public Profile Profile 
        {
            get { return _profile; }
            set
            {
                _profile = value;
                ShowPaths();
            }
        
        }

        double _pathwidth = 1;

        public CartesianGrid Grid { get; set; }

        public ProfileDisplay()
        {
            this.InitializeComponent();
            Grid = new CartesianGrid(new Range(-100, 10, 100), new Range(-20,5,20));
            Grid.Foreground = Colors.Blue;
            Grid.PropertyChanged += Grid_PropertyChanged;
            double s = ScaleFactor;
            (ProfileDisplayCanvas.RenderTransform as CompositeTransform).ScaleX = s;
            (ProfileDisplayCanvas.RenderTransform as CompositeTransform).ScaleY = -s;
            Grid.Outline.StrokeThickness = (Math.Abs(s) > 0) ? _pathwidth / Math.Abs(s) : _pathwidth;
            ProfileDisplayCanvas.Children.Add(Grid.Outline);
            AddAxes();
            
        }

        void Grid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // remove old grid and replace
            Grid.Update();
            Replace(Grid.Outline);
            ChangeScaleFactorTo(ScaleFactor);
        }

        private double ScaleFactor
        {
            get
            {
                double width = (PathBorder.ActualWidth <= 0)
                                   ? PathBorder.MinWidth : PathBorder.ActualWidth;
                width -= PathBorder.BorderThickness.Left + PathBorder.BorderThickness.Right
                           + PathBorder.Margin.Left + PathBorder.Margin.Right;


                double height = (PathBorder.ActualHeight <= 0)
                              ? PathBorder.MinHeight : PathBorder.ActualHeight;
                height -= PathBorder.BorderThickness.Top + PathBorder.BorderThickness.Bottom
                        + PathBorder.Margin.Top + PathBorder.Margin.Bottom;

                return Math.Abs(Math.Min(width / (2 * Grid.Dim1.End), height / (2 * Grid.Dim1.End)));

            }
        }

        private void AddAxes()
        {
            double m = ScaleFactor;
            Line Xaxis = new Line();
            Xaxis.Name = XAXIS;
            Xaxis.Stroke = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
            Xaxis.X1 = Grid.Dim1.Start - Grid.Dim1.Inc;
            Xaxis.Y1 = 0;
            Xaxis.X2 = Grid.Dim1.End + Grid.Dim1.Inc; 
            Xaxis.Y2 = 0;
            Xaxis.StrokeThickness = 1 ;

            Line Yaxis = new Line();
            Yaxis.Name = YAXIS;
            Yaxis.Stroke = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
            Yaxis.X1 = 0;
            Yaxis.Y1 = Grid.Dim2.Start - Grid.Dim2.Inc;
            Yaxis.X2 = 0;
            Yaxis.Y2 = Grid.Dim2.End + Grid.Dim2.Inc;
            Yaxis.StrokeThickness = 1 ;

            ProfileDisplayCanvas.Children.Add(Xaxis);
            ProfileDisplayCanvas.Children.Add(Yaxis);
        }

        private void Replace(Shape s)
        {
            UIElement ui = null;

            foreach (UIElement uie in ProfileDisplayCanvas.Children)
            {
                if ((uie is Shape) && (uie as Shape).Name == s.Name)
                {
                    ui = uie;
                    break;
                }
            }
            if (ui != null)
            {
                ProfileDisplayCanvas.Children.Remove(ui);
                ProfileDisplayCanvas.Children.Add(s);
            }
            else
                ProfileDisplayCanvas.Children.Add(s);
        }

        public void ChangeScaleFactorTo(double inc)
        {
            if (inc > 0)
            {
                (ProfileDisplayCanvas.RenderTransform as CompositeTransform).ScaleX = inc;
                (ProfileDisplayCanvas.RenderTransform as CompositeTransform).ScaleY = -inc;
                foreach (Shape uie in ProfileDisplayCanvas.Children)
                {
                    if (!(uie is Polygon))
                        uie.StrokeThickness = 1 / inc;
                }
            }
        }

        public void ShowPaths()
        {
            ProfileDisplayCanvas.Children.Clear();
            ProfileDisplayCanvas.Children.Add(Grid.Outline);
            AddAxes();
            if (Profile != null)
            {
                Shape s = Profile.Visual;
                s.Stroke = new SolidColorBrush(Colors.Yellow);
                s.StrokeThickness = 1;
                ProfileDisplayCanvas.Children.Add(s);
            }
        }

        public SolidColorBrush CanvasBackgroundBrush 
        { 
            get
            {
                return (SolidColorBrush)ProfileDisplayCanvas.Background;
            }
        }
    }
}
