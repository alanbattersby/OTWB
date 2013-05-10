using OTWB.Collections;
using OTWB.Common;
using OTWB.Lattice;
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

namespace OTWB
{
    public sealed partial class PathDisplay : UserControl
    {
        private const string XAXIS = "XAXIS";
        private const string YAXIS = "YAXIS";
        private ShapeCollection  _currentPath;
        public RadialGrid Grid { get; set; }
        public Windows.UI.Xaml.Shapes.Path WorkOutline;

        ViewModel _viewmodel;
        public ViewModel Viewmodel
        {
            get { return _viewmodel; }
            set 
            { 
                _viewmodel = value;
                if (_viewmodel.CurrentPathData is LatticeData)
                {
                    CreateWorkOutline();
                }
            }
        }

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

        bool _showgrid;
        public bool Showgrid
        {
            get { return _showgrid; }
            set
            {
                _showgrid = value;
                if (_showgrid)
                    ShowGrid();
                else
                    HideGrid();
            }
        }

        bool _showWorkOutline;
        public bool ShowWorkOutline
        {
            get { return _showWorkOutline; }
            set
            {
                _showWorkOutline = value;
                if (_showWorkOutline)
                    ShowWorkPieceOutline();
                else
                    HideWorkPieceOutline();
            }
        }
       
        public void CreateWorkOutline()
        {
            WorkOutline = new Windows.UI.Xaml.Shapes.Path();
            GeometryGroup gg = new GeometryGroup();
            Point origin = new Point(0, 0);
            EllipseGeometry eg = new EllipseGeometry();
            eg.Center = origin;
            eg.RadiusX = eg.RadiusY = (_viewmodel.CurrentPathData as LatticeData).Layout.ClipRange.End;
            gg.Children.Add(eg);
            if ((_viewmodel.CurrentPathData as LatticeData).Layout.ClipRange.Start > 0)
            {
                eg = new EllipseGeometry();
                eg.Center = origin;
                eg.RadiusX = eg.RadiusY = (_viewmodel.CurrentPathData as LatticeData).Layout.ClipRange.Start;
                gg.Children.Add(eg);
            }
            WorkOutline.Data = gg;
            WorkOutline.StrokeThickness = 1 / ScaleFactor;
            WorkOutline.Stroke = new SolidColorBrush(Colors.Wheat);
            WorkOutline.StrokeDashArray = new DoubleCollection() { 5, 5 };
            WorkOutline.Name = "WorkOutline";
        }

        public ShapeCollection CurrentPath 
        {
            set
            {
                //if (_currentPath != null)
                //    _currentPath.CollectionChanged -= CurrentPath_CollectionChanged;
                _currentPath = value;
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

        public SolidColorBrush CanvasBackgroundBrush
        {
            get { return (SolidColorBrush)PathDisplayCanvas.Background; }
        }

        public PathDisplay()
        {
            this.InitializeComponent();
            _pathwidth = 1.0;
            Grid = new RadialGrid(new Range(0,10,100));
            Grid.Foreground = Colors.Blue;
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

        void Grid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // remove old grid and replace
            Grid.Update();
            Replace(Grid.Outline);
            ChangeScaleFactorTo(ScaleFactor);
        }

        public void ShowPaths()
        {
            //Debug.WriteLine("Starting Adding paths to Canvas");
            if ((_currentPath != null) && (_currentPath.Count > 0))
            {
                //Debug.WriteLine("Current path viable {0}", _currentPath.PatternName);
                PathDisplayCanvas.Children.Clear();
                if (_showgrid)
                    PathDisplayCanvas.Children.Add(Grid.Outline);
                if (_showWorkOutline)
                    PathDisplayCanvas.Children.Add(WorkOutline);
                try
                {
                    foreach (Shape path in _currentPath.Shapes)
                    {
                        path.StrokeThickness = _pathwidth;
                        path.Stroke = new SolidColorBrush(Colors.Red);
                        path.StrokeEndLineCap = PenLineCap.Round;
                        this.PathDisplayCanvas.Children.Add(path);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.InnerException);
                }
            }
        }

        //private void Clip(Shape s)
        //{
        //    double r = _viewmodel.WorkDiameter / 2;
        //    List<PathFigure> outside = new List<PathFigure>();

        //    if (s is Windows.UI.Xaml.Shapes.Path)
        //    {
        //        Geometry g = (s as Windows.UI.Xaml.Shapes.Path).Data;
        //        if (g is GeometryGroup)
        //        {
        //            foreach ( Geometry gc in (g as GeometryGroup).Children)
        //            {
        //                if (gc is PathGeometry)
        //                {
        //                    foreach (PathFigure pf in (gc as PathGeometry).Figures)
        //                    {
        //                        if (distance(pf.StartPoint) < r)
        //                        {
        //                            foreach (PathSegment ps in pf.Segments)
        //                            {
        //                                if (ps is PolyLineSegment)
        //                                {
        //                                    PolyLineSegment pls = ps as PolyLineSegment;
        //                                    PointCollection pc = new PointCollection();
        //                                    List<Point> lp = pls.Points.Where(p => distance(p) < r).ToList();
        //                                    pls.Points.Clear();
        //                                    foreach (Point p in lp)
        //                                        pls.Points.Add(p);

        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            outside.Add(pf);
        //                        }

        //                    }
        //                    foreach (PathFigure pf in outside)
        //                    {
        //                        (gc as PathGeometry).Figures.Remove(pf);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private double distance(Point p)
        //{
        //    return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        //}

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
                        if (!(uie is Polygon) || uie.Name == "Workoutline")
                            uie.StrokeThickness = _pathwidth / (s + inc);
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

        internal void ShowWorkPieceOutline()
        {
            foreach (UIElement uie in PathDisplayCanvas.Children)
            {
                if (uie is Windows.UI.Xaml.Shapes.Path)
                {
                    if ((uie as Windows.UI.Xaml.Shapes.Path) == WorkOutline)
                    {
                        return;
                    }
                }
            }
            if (_viewmodel != null)
                PathDisplayCanvas.Children.Add(WorkOutline);
        }

        internal void HideWorkPieceOutline()
        {
            foreach (UIElement uie in PathDisplayCanvas.Children)
            {
                if (uie is Windows.UI.Xaml.Shapes.Path)
                {
                    if (uie == WorkOutline)
                    {
                        PathDisplayCanvas.Children.Remove(uie);
                    }
                }
            }
        }

        internal void ShowGrid()
        {        
            foreach (UIElement uie in PathDisplayCanvas.Children)
            {
                if (uie is Windows.UI.Xaml.Shapes.Path)
                {
                    if ((uie as Windows.UI.Xaml.Shapes.Path) == Grid.Outline)
                    {
                        return;
                    }
                }
            }
            PathDisplayCanvas.Children.Add(Grid.Outline);
        }
        internal void HideGrid()
        {
            foreach (UIElement uie in PathDisplayCanvas.Children)
            {
                if (uie is Windows.UI.Xaml.Shapes.Path)
                {
                    if (uie == Grid.Outline)
                    {
                        PathDisplayCanvas.Children.Remove(uie);
                    }
                }
            }
        }
    }
}
