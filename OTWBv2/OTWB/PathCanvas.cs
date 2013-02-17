using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Geometric_Chuck
{
    class PathCanvas : Windows.UI.Xaml.Controls.Canvas
    {
        private const string XAXIS = "XAXIS";
        private const string YAXIS = "YAXIS";
        PolygonCollection Paths { get; set; }

        public Extent CurrentExtent { get; set; }      

        public PathCanvas() : base() 
        {
            Paths = new PolygonCollection();
            //AddAxes();
            //CurrentExtent = new Extent(-1, 1, -1, 1);
            //AddHilightPoint(new Point(0, 0));
            this.RenderTransformOrigin = new Point(0.5, 0.5);
           
        }

        public void Clear()
        {
            //ResourceLoader loader = new ResourceLoader();
            //string name = loader.GetString("HILITE_POINT");
            foreach (Shape s in Children)
            {

                if ((s.Name != XAXIS) && (s.Name != YAXIS))
                    Children.Remove(s);
            }
        }

        private void Replace(Shape s)
        {
            UIElement ui = null;

            foreach (UIElement uie in Children)
            {
                if ((uie is Shape) && (uie as Shape).Name == s.Name)
                {
                    ui = uie;
                    break;
                }
            }
            if (ui != null)
            {
                Children.Remove(ui);
                Children.Add(s);
            }
            else
                Children.Add(s);
        }

        //private void AddAxes()
        //{
        //    Line Xaxis = new Line();
        //    Xaxis.Name = XAXIS;
        //    Xaxis.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
        //    Xaxis.X1 = -100; Xaxis.Y1 = 0;
        //    Xaxis.X2 = 100; Xaxis.Y2 = 0;
        //    Line Yaxis = new Line();
        //    Yaxis.Name = YAXIS;
        //    Yaxis.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
        //    Yaxis.X1 = 0; Yaxis.Y1 = -100;
        //    Yaxis.X2 = 0; Yaxis.Y2 = 100;
        //    Children.Add(Xaxis);
        //    Children.Add(Yaxis);
        //}

        //public void AddHilightPoint(Point p)
        //{
        //    if (ScaleFactor > 0)
        //    {
        //        ResourceLoader loader = new ResourceLoader();
        //        Windows.UI.Xaml.Shapes.Path path = new Windows.UI.Xaml.Shapes.Path();
        //        path.RenderTransform = new ScaleTransform();
        //        path.Name = loader.GetString("HILITE_POINT");
        //        path.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 255));
        //        path.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 255)); ;
        //        GeometryGroup g = new GeometryGroup();
        //        EllipseGeometry eg = new EllipseGeometry();

        //        eg.Center = p;
        //        eg.RadiusX = eg.RadiusY = 1;
        //        g.Children.Add(eg);
        //        path.Data = g;

        //        eg.RadiusX = eg.RadiusY = 2 / ScaleFactor;
        //        (path.RenderTransform as ScaleTransform).ScaleX = ScaleFactor;
        //        (path.RenderTransform as ScaleTransform).ScaleY = ScaleFactor;
        //        Replace(path);
        //    }
        //}

        //private double ScaleFactor
        //{
        //    get
        //    {
        //        double width = (ActualWidth <= 0)
        //                        ? MinWidth : ActualWidth;
        //        double height = (ActualHeight <= 0)
        //                      ? MinHeight : ActualHeight;
               
        //        double xsf = width / (2 * CurrentExtent.MaxX);
        //        double ysf = height / (2 * CurrentExtent.MaxY);
        //        return Math.Min(xsf, ysf);
        //    }
        //}

        private void CalcExtent()
        {
            CurrentExtent = new Extent(double.MaxValue, double.MinValue, double.MaxValue, double.MinValue);
            foreach (Polygon poly in Paths.Polygons)
               foreach (Point p in poly.Points)
                 CurrentExtent.Update(p);
        }

        //public void AddPath(Polygon path)
        //{
        //    Paths.AddPoly(path);
        //    CalcExtent();
        //    double sf = ScaleFactor;
        //    path.StrokeThickness = (Math.Abs(sf) > 0) ? 1 / sf : 1;
        //    (path.RenderTransform as ScaleTransform).ScaleX = sf;
        //    (path.RenderTransform as ScaleTransform).ScaleY = sf;
        //    Children.Add(path);
        //}
    }
}
