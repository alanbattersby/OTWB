using OTWB.Braid;
using OTWB.Collections;
using OTWB.Coordinates;
using OTWB.Interfaces;
using OTWB.Lattice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace OTWB.PathGenerators
{
    class LatticeRimEngine : IPathGenerator
    {
        LatticeData ld;
        double rowHeight;
        double colWidth;
        Point origin;
        double repeatAngle;
        double colAngle;
        double rowStart;

        public ShapeCollection CreatePaths(PathData p, double increment)
        {
           
            ShapeCollection pc = new ShapeCollection();
            if (p == null || !(p is LatticeData)) return pc;
            ld = p as LatticeData;
            repeatAngle = 360.0 / ld.Layout.RepeatX;
            colAngle = (repeatAngle - ld.Layout.Margin) / (ld.Columns - 1);

            rowHeight = ld.Layout.Height / (ld.Rows - 1);
            colWidth = ld.Layout.Width / (ld.Columns - 1);
            origin = new Point(ld.Layout.ToolPosition, 0);

            if ((ld.Layout.RepeatX == 0) && (ld.Layout.RepeatY == 0))
            {
                pc.AddShape(MkPath(0, 0, increment));
            }
            else if ((ld.Layout.RepeatX == 0) && (ld.Layout.RepeatY > 0))
            {
                for (int indy = -ld.Layout.RepeatY; indy < ld.Layout.RepeatY; indy++)
                {
                    pc.AddShape(MkPath(0, indy, increment));
                }
            }
            else if ((ld.Layout.RepeatX > 0) && (ld.Layout.RepeatY == 0))
            {
                for (int indx = -ld.Layout.RepeatX; indx < ld.Layout.RepeatX; indx++)
                {
                    pc.AddShape(MkPath(indx, 0, increment));
                }
            }
            else
            {
                for (int indy = -ld.Layout.RepeatY; indy < ld.Layout.RepeatY; indy++)
                {
                    for (int indx = -ld.Layout.RepeatX; indx < ld.Layout.RepeatX; indx++)
                    {
                        pc.AddShape(MkPath(indx, indy, increment));
                    }
                }
            }

            //for (int indx = 0; indx < ld.Layout.RepeatX; indx++)
            //{
            //    pc.AddShape(MkPath(indx,increment));
            //}
            return pc;
        }

        private Windows.UI.Xaml.Shapes.Shape MkPath(int indx, int indy, double inc)
        {
            double rad = BasicLib.ToRadians;
            double deg = BasicLib.ToDegrees;

            Path path = new Path();
            path.Stroke = new SolidColorBrush(Colors.Red);
            path.Name = string.Format("Lattice{0}_{1}", indx, indy);
            GeometryGroup g = new GeometryGroup();
            path.Data = g;
            PathGeometry pg = new PathGeometry();
            g.Children.Add(pg);
            double startAngle = indx * repeatAngle;

            foreach (Line2D ll in ld.Lines)
            { 
                PathFigure pf = new PathFigure();
                double angle1 = startAngle + ll.X1 * colAngle;
                double angl1 = angle1 * rad;
                double angle2 = startAngle + ll.X2 * colAngle;
                double angl2 = angle2 * rad;
                double rowstart = indy * ld.Layout.Height;
                double y1 = ld.Layout.ToolPosition - rowstart  - ll.Y1 * rowHeight;
                double y2 = ld.Layout.ToolPosition - rowstart - ll.Y2 * rowHeight;
                
                pf.StartPoint = new Point(y1 * Math.Cos(angl1),
                                          y1 * Math.Sin(angl1));
                if (ll.IsVertical)
                {      
                    LineSegment ls = new LineSegment();
                    ls.Point = new Point(y2 * Math.Cos(angl1),
                                          y2 * Math.Sin(angl1));
                    pf.Segments.Add(ls);
                   
                }
                else if (ll.IsHorizontal)
                {
                    PolyLineSegment pls = new PolyLineSegment();
                    double a = angl1;
                    Point pnt;
                    do
                    {
                        pnt = new Point(y2 * Math.Cos(a), y2 * Math.Sin(a));
                        pls.Points.Add(pnt);
                        a += (angl1 < angl2) ? inc : -inc;
                    }
                    while ((angl1 < angl2) ? (a < angl2) : ( a > angl2));
                    if ((angl1 < angl2) ? (a != angl2) : (a != angl1))
                    {
                        a = (angl1 < angl2) ? angl2 : angl1;
                        pnt = new Point(y2 * Math.Cos(a), y2 * Math.Sin(a));
                        pls.Points.Add(pnt);
                    }
                    pf.Segments.Add(pls);
                }
                else // is diagonal
                {
                    double i = 0;
                    PolyLineSegment pls = new PolyLineSegment();
                    for (i = 0; i <= 1.0; i += inc)
                    {                  
                        pls.Points.Add(Interp(ll.X1,y1,i,ll.X2,y2,startAngle));
                    }        
                    pls.Points.Add(Interp(ll.X1, y1, 1.0, ll.X2, y2,startAngle));
                  
                    pf.Segments.Add(pls);
                } 
                pg.Figures.Add(pf);
            }
            return path;
        }

        Point Interp(double x1, double y1, double indx, double x2, double y2, double sa)
        {
            double x = BasicLib.Linterp(x1, indx, x2);
            double y = BasicLib.Linterp(y1, indx, y2);
            double r = Math.Sqrt(x * x + y * y);
            double a = sa + x * colAngle;
            return new Point(r * Math.Cos(BasicLib.ToRadians * a),
                             r * Math.Sin(BasicLib.ToRadians * a));
        }
      

        public ToolPath CreateToolPath(PathData pd, double inc)
        {
            ToolPath tp;
            LatticeData l = pd as LatticeData;
            tp = new ToolPath(); //.BarrelOutline((int)(1 / inc)));
            return tp;
        }
    }
}
