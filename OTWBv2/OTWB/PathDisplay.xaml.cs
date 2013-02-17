using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Geometric_Chuck
{
    public sealed partial class PathDisplay : UserControl
    {
        private const string XAXIS = "XAXIS";
        private const string YAXIS = "YAXIS";
        private PolygonCollection  _currentPath;
        public RadialGrid Grid { get; set; }

        double _pathwidth;
        public double PathWidth
        {
            get { return _pathwidth; }
            set
            {
                _pathwidth = value;
                ShowPaths();
            }
        }

        public PolygonCollection CurrentPath 
        {
            set
            {
                if (_currentPath != null)
                    _currentPath.CollectionChanged -= CurrentPath_CollectionChanged;
                _currentPath = value;
                if (_currentPath != null)
                    _currentPath.CalculateExtent();
            }

            get
            {
                return _currentPath;
            }
        }

        public void CurrentPath_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //Debug.WriteLine("Current Path Changed  action {0}", e.Action);
            //if (e.NewItems != null)
            //    foreach (object itm in e.NewItems)
            //        if (itm != null)
            //            Debug.WriteLine("Added {0}", itm);
            if ((e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) ||
                (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) ||
                (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace) ||
                (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset))
            {
                ///TODO  make this more sophisticated
                ShowPaths();
            }

        } 
               
        public PathDisplay()
        {
            this.InitializeComponent();
            _pathwidth = 1.0;
            Grid = new RadialGrid(new Range(0,10,100));
            Grid.PropertyChanged += Grid_PropertyChanged;
            double s = ScaleFactor;
            (PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleX = s;
            (PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleY = s;
            Grid.Outline.StrokeThickness = (Math.Abs(s) > 0) ? _pathwidth / Math.Abs(s) : _pathwidth; 
            PathDisplayCanvas.Children.Add(Grid.Outline);
            AddAxes();
            
            //AddHilightPoint(new Point(0, 0));
            this.RenderTransformOrigin = new Point(0.5, 0.5);
            this.PointerWheelChanged += PathDisplay_PointerWheelChanged;
        }

        public void UpdateGrid()
        {
            Grid.Update();
            Replace(Grid.Outline);
            ChangeScaleFactorTo(ScaleFactor);
        }

        void Grid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // remove old grid and replace
            Replace(Grid.Outline);
            ChangeScaleFactorTo(ScaleFactor);
        }

        public void ShowPaths()
        {
            //Debug.WriteLine("Starting Adding paths to Canvas");
            if ((_currentPath != null) && (_currentPath.Count > 0))
            {
                //Debug.WriteLine("Current path viable {0}", _currentPath.PatternName);
                Clear();
                if (_currentPath.PathExtent.IsMaxExtent)
                    _currentPath.CalculateExtent();
                //Debug.WriteLine("Extent is {0} ",CurrentPath.Extent.ToString());
                double m = ScaleFactor;
               // (PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleX = m;
                foreach (Polygon path in _currentPath.Polygons)
                {
                    path.StrokeThickness = _pathwidth;
                    //if (!double.IsInfinity(m))
                    //{
                    //    path.StrokeThickness = (Math.Abs(m) > 0) ? _pathwidth / m : _pathwidth;
                    //}
                    if (!this.PathDisplayCanvas.Children.Contains(path))
                        this.PathDisplayCanvas.Children.Add(path);

                    
                }
            }
        }

        public void Clear()
        {
            //ResourceLoader loader = new ResourceLoader();
            //string name = loader.GetString("HILITE_POINT");
            foreach (Shape s in PathDisplayCanvas.Children)
            {

                if (s is Polygon)
                    PathDisplayCanvas.Children.Remove(s);
            }
        }

        private void Replace(Shape s)
        {
            UIElement ui = null;

            foreach (UIElement uie in PathDisplayCanvas.Children)
            {
                if ((uie is Shape) && (uie as Shape).Name == s.Name)
                {
                    ui = uie;
                    break;
                }
            }
            if (ui != null)
            {
                PathDisplayCanvas.Children.Remove(ui);
                PathDisplayCanvas.Children.Add(s);
            }
            else
                PathDisplayCanvas.Children.Add(s);
        }

        private void AddAxes()
        {
            double m = ScaleFactor;
            Line Xaxis = new Line();
            Xaxis.Name = XAXIS;
            Xaxis.Stroke = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
            Xaxis.X1 = -10; Xaxis.Y1 = 0;
            Xaxis.X2 = 10; Xaxis.Y2 = 0;
            Xaxis.StrokeThickness = 1 / m;
       
            Line Yaxis = new Line();
            Yaxis.Name = YAXIS;
            Yaxis.Stroke = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255));
            Yaxis.X1 = 0; Yaxis.Y1 = -10;
            Yaxis.X2 = 0; Yaxis.Y2 = 10;
            Yaxis.StrokeThickness = 1 / m;
            
            PathDisplayCanvas.Children.Add(Xaxis);
            PathDisplayCanvas.Children.Add(Yaxis);
        }

        public void AddGrid()
        {
            //Grid grid = new Grid();
            //PathCanvas.Children.Add(Grid.Outline);
        }

        public void AddHilightPoint(Point p)
        {
            double m = ScaleFactor;
            if (m > 0)
            {
                ResourceLoader loader = new ResourceLoader();
                Windows.UI.Xaml.Shapes.Path path = new Windows.UI.Xaml.Shapes.Path();
                path.RenderTransform = CreateTransforms(0,0,1,1);
                path.Name = loader.GetString("HILITE_POINT");
                path.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 255));
                path.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 255)); ;
                GeometryGroup g = new GeometryGroup();
                EllipseGeometry eg = new EllipseGeometry();

                eg.Center = p;
                eg.RadiusX = eg.RadiusY = 1;
                g.Children.Add(eg);
                path.Data = g;

                eg.RadiusX = 2/m;
                eg.RadiusY = 2 / m;
                (path.RenderTransform as CompositeTransform).ScaleX = m;
                (path.RenderTransform as CompositeTransform).ScaleY = m;
                Replace(path);
            }
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

                return Math.Abs(Math.Min(width / (2 * Grid.Dim1.End),height / (2 * Grid.Dim1.End)));

            }
        }

        CompositeTransform CreateTransforms(double x, double y, double sfx, double sfy)
        {
            CompositeTransform ct = new CompositeTransform();
            ct.ScaleX = sfx;
            ct.ScaleY = sfy;
            ct.TranslateX = x;
            ct.TranslateY = y;
            return ct;
        }

        private void PathDisplay_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            
            int delta = e.GetCurrentPoint(null).Properties.MouseWheelDelta;
            //Debug.WriteLine("moved");
            double inc =  (delta > 0) ? 0.2 : -0.2;
            ChangeScaleFactorBy(inc);
        }

        void ChangeScaleFactorBy(double inc)
        {
            try
            {
                double s = Math.Abs((PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleX);
                if (s + inc > 0)
                {
                    (PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleX += inc;
                    (PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleY += inc;


                    foreach (Shape uie in PathDisplayCanvas.Children)
                    {
                        if (!(uie is Polygon))
                            uie.StrokeThickness = 1 / (s + inc);
                    }
                }
            }
            catch (System.ArgumentException ae)
            {
                Debug.WriteLine(ae.Message);
            }
        }

        public void ChangeScaleFactorTo(double inc)
        {
            if (inc > 0)
            {
                (PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleX = inc;
                (PathDisplayCanvas.RenderTransform as CompositeTransform).ScaleY = inc;
                foreach (Shape uie in PathDisplayCanvas.Children)
                {
                    if (!(uie is Polygon))
                        uie.StrokeThickness = 1 / inc;
                }
            }
        }

        public void Home()
        {
            //CalcExtent();
            ChangeScaleFactorTo(ScaleFactor);
            ChangeTranslationTo(0, 0);
        }

        public void ChangeTranslationTo(double x, double y)
        {
            (PathDisplayCanvas.RenderTransform as CompositeTransform).TranslateX = x;
            (PathDisplayCanvas.RenderTransform as CompositeTransform).TranslateY=  y;
        }

        public void Centre()
        {
            //CalcExtent();
            Point cntr = CurrentPath.PathExtent.Centre;
            foreach (UIElement uie in PathDisplayCanvas.Children)
            {
                if (uie is Polygon)
                {
                    CompositeTransform ct = (CompositeTransform)uie.RenderTransform;
                    ct.TranslateX= -cntr.X;
                    ct.TranslateY = -cntr.Y;
                }
            }

        }

        public void CleanUp()
        {
            if (_currentPath != null)
                _currentPath.CollectionChanged -= CurrentPath_CollectionChanged;
            _currentPath = null;
            PathDisplayCanvas.Children.Clear();
        }

    }
}
