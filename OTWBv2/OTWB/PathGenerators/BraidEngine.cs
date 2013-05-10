using OTWB.Braid;
using OTWB.Collections;
using OTWB.Interfaces;
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
    class BraidEngine :  IPathGenerator
    {
        BraidData bd;
        double rowHeight;
        double colWidth;
        Point origin;
        double repeatAngle;
        double colAngle;

        double rad = BasicLib.ToRadians;
        double deg = BasicLib.ToDegrees;

        public ShapeCollection CreatePaths(PathData pd, double increment)
        {
            bd = pd as BraidData;
            ShapeCollection pc = new ShapeCollection();
            pc.PatternName = pd.Name;
            repeatAngle = 2 * Math.PI / bd.Repeats;
            colAngle = (repeatAngle - rad * bd.Margin) / bd.Perms.Count ;

            rowHeight = bd.Width / (bd.NumStrands - 1);
            colWidth = bd.Length / bd.Perms.Count;
            origin = new Point(bd.ToolPosition, 0);

            for (int indx = 0; indx < bd.Repeats; indx++)
            {
                pc.AddShape(MkPath(indx, increment));
            }
            return pc;
           
        }


        public ToolPath CreateToolPath(PathData pd, double inc)
        {
            ToolPath tp;
            BraidData b = pd as BraidData;
            tp = new ToolPath(); //.BarrelOutline((int)(1 / inc)));
            return tp;
        }

        private Path MkPath(double indx, double increment)
        {
            Path path = new Path();
            path.Stroke = new SolidColorBrush(Colors.Red);
            path.Name = string.Format("Braid{0}", indx);
            path.Tag = indx;

            GeometryGroup g = new GeometryGroup();
            path.Data = g;
            PathGeometry pg = new PathGeometry();
            g.Children.Add(pg);

            //pg.Figures.Add(BuildStrand(indx * repeatAngle, 0, increment));
            for (int i = 0; i < bd.NumStrands; i++)
            {
                pg.Figures.Add(BuildStrand(indx * repeatAngle, i, increment));
            } 
            return path;
        }

        private PathFigure BuildStrand(double startAngle, int indx, double inc)
        {
            // now build strand start with first point
            PathFigure pf = new PathFigure();

            int currentStrand = indx;

            // now loop through perms
            foreach (Permutation p in bd.Perms)
            {
                // point for start of perm
                int pindx = bd.Perms.IndexOf(p);
                double x1 = pindx;
                double x2 = x1 + 1;
                double sangle = startAngle + colAngle * x1; 
                double y1 = bd.ToolPosition - currentStrand * rowHeight;
                if (pindx == 0)
                {
                    pf.StartPoint = new Point(y1 * Math.Cos(sangle), y1 * Math.Sin(sangle));
                }
                int nxtStrand = p.PermOf(currentStrand);
                double eangle = sangle + colAngle;
                double y2 = bd.ToolPosition - nxtStrand * rowHeight;

                PolyLineSegment pls = new PolyLineSegment();
                double i = 0;
                for (i = 0; i < 1.0; i += inc)
                {
                    if (p.IsNoChange)
                    {
                        double a = sangle + i * colAngle;
                        Point pnt = new Point(y1 * Math.Cos(a), y1 * Math.Sin(a));
                        pls.Points.Add(pnt);
                    }
                    else
                        pls.Points.Add(Interp(x1, y1, i, x2, y2, sangle));
                }
                if (p.IsNoChange)
                {
                    double a = sangle + 1.0 * colAngle;
                    Point pnt = new Point(y1 * Math.Cos(a), y1 * Math.Sin(a));
                    pls.Points.Add(pnt);
                }
                else
                    pls.Points.Add(Interp(x1, y1, 1.0, x2, y2, sangle));

                pf.Segments.Add(pls);
                currentStrand = nxtStrand;
            }

            return pf;
        }

        Point Interp(double x1, double y1, double indx, double x2, double y2, double sa)
        {
            double x = BasicLib.Linterp(x1, indx, x2);
            double y = BasicLib.Linterp(y1, indx, y2);
            double r = Math.Sqrt(x * x + y * y);
            double a = sa + (x - x1) * colAngle;
            return new Point(r * Math.Cos(a),
                             r * Math.Sin(a));
        }
    }
}
