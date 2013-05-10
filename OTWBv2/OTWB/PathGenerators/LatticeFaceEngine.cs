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
using OTWB.Extensions;


namespace OTWB.PathGenerators
{
    class LatticeFaceEngine : IPathGenerator
    {
        LatticeData ld;
        double rowHeight;
        double colWidth;

        public ShapeCollection CreatePaths(PathData p, double increment)
        {
           
            ShapeCollection pc = new ShapeCollection();
            if (p == null || !(p is LatticeData)) return pc;
            ld = p as LatticeData;
            
            rowHeight = ld.Layout.Height / (ld.Rows - 1);
            colWidth = ld.Layout.Width / (ld.Columns - 1);
            //Shape s = MkPath(0, 0, increment);
            //if (s != null)
            //    pc.AddShape(s);
            if ((ld.Layout.RepeatX == 0) && (ld.Layout.RepeatY == 0))
            {
                pc.AddShape(MkPath(0, 0, increment));
            }
            else if ((ld.Layout.RepeatX == 0) && (ld.Layout.RepeatY > 0))
            {
                for (int indy = -ld.Layout.RepeatY; indy < ld.Layout.RepeatY; indy++)
                {
                    Shape s = MkPath(0, indy, increment);
                    if (s != null)
                        pc.AddShape(s);
                }
            }
            else if ((ld.Layout.RepeatX > 0) && (ld.Layout.RepeatY == 0))
            {
                for (int indx = -ld.Layout.RepeatX; indx < ld.Layout.RepeatX; indx++)
                {
                    Shape s = MkPath(indx, 0, increment);
                    if (s != null)
                        pc.AddShape(s);
                }
            }
            else
            {
                for (int indy = -ld.Layout.RepeatY; indy < ld.Layout.RepeatY; indy++)
                {
                    for (int indx = -ld.Layout.RepeatX; indx < ld.Layout.RepeatX; indx++)
                    {
                        Shape s = MkPath(indx, indy, increment);
                        if (s != null)
                            pc.AddShape(s);
                    }
                }

            }
            return pc;
        }

        private Point CircleLineIntersect(Point Inside, Point Outside, double r)
        {
            Tuple<int, Point, Point> intrsct = BasicLib.CircleLineIntersect(Inside, Outside,r);
            if ((intrsct.Item1 == 0) || (intrsct.Item1 == 1))
            {
                return intrsct.Item2;
            }
            else 
            {
                if (BasicLib.Between(Inside, intrsct.Item2, Outside))
                    return intrsct.Item2;
                else
                    return intrsct.Item3;


                
            }
        }
        
        private Windows.UI.Xaml.Shapes.Shape MkPath(int indx, int indy, double inc)
        {
            double innerR = Math.Abs(ld.Layout.ClipRange.Start);
            double outerR = Math.Abs(ld.Layout.ClipRange.End);
            
            double startX = indx * ld.Layout.Width + ld.Layout.OffsetX * colWidth;
            double startY = indy * ld.Layout.Height + ld.Layout.OffsetY * rowHeight;

            Path path = new Path();
            path.Stroke = new SolidColorBrush(Colors.Red);
            path.Name = string.Format("Lattice{0}_{1}", indx,indy);
            GeometryGroup g = new GeometryGroup();
            path.Data = g;
            PathGeometry pg = new PathGeometry();
            g.Children.Add(pg);
            PathFigure pf;
            Point origin = new Point(0, 0);

            foreach (Line2D ll in ld.Lines)
            {
                double x1 = startX + ll.X1 * colWidth;
                double x2 = startX + ll.X2 * colWidth;
                double y1 = startY + ll.Y1 * rowHeight;
                double y2 = startY + ll.Y2 * rowHeight;

               
                Point p1 = new Point(x1, y1);
                Point p2 = new Point(x2, y2);
                if (ld.Layout.Hyper)
                {
                    Hypo(ref p1);
                    Hypo(ref p2);
                }

                Point intrsct;

                if (ld.Layout.Clip)
                {
                    double d1 = p1.Radius();
                    double d2 = p2.Radius();
                    Point pstart = (d1 < d2) ? p1 : p2;
                    Point pend = (d1 < d2) ? p2 : p1;
                    pf = null;
                    if (innerR > 0)
                    {
                        d1 = pstart.Radius();
                        d2 = pend.Radius();
                        // must clip against both inner and outer radii
                        // first clip against inner 
                        if ((d1 > outerR) && (d2 > outerR))
                        {
                            Tuple<int, Point, Point> intrscts = BasicLib.CircleLineIntersect(pstart, pend, outerR);
                            if (intrscts.Item1 == 1)
                            {
                               
                            }
                            else if (intrscts.Item1 == 2)
                            {
                                if (BasicLib.Between(pstart, intrscts.Item2, pend) &&
                                    BasicLib.Between(pstart, intrscts.Item3, pend))
                                {
                                    pf = GeneratePathFigure(intrscts.Item2, intrscts.Item3);
                                    pg.Figures.Add(pf);
                                }
                            }
                        }
                        else if ((d1 < innerR) && (d2 < innerR))
                        {
                           // do nothing
                        }
                       
                        else if ((d1 < innerR) && (d2 >= innerR) && (d2 <= outerR))
                        {
                            if (BasicLib.IsVertical(pstart, pend))
                            {
                                double y = BasicLib.Sgn(pstart.Y) * Math.Sqrt(innerR * innerR - pstart.X * pstart.X);
                                Point p = new Point(pstart.X, y);
                                pf = GeneratePathFigure(p, pend);
                            }
                            else if (BasicLib.IsHorizontal(pstart, pend))
                            {
                                double x = BasicLib.Sgn(pstart.X) * Math.Sqrt(innerR * innerR - pstart.Y * pstart.Y);
                                Point p =  new Point(x, pstart.Y);
                                pf = GeneratePathFigure(p,pend);
                            }
                            else
                            {
                                intrsct = CircleLineIntersect(pstart, pend, innerR);
                                pf = GeneratePathFigure(intrsct, pend);
                            }
                        }
                        else if ((d1 >= innerR) && (d1 <= outerR) && (d2 >= innerR))
                        {
                            if (d2 <= outerR)
                            {
                                if (BasicLib.IsDiagonal(pstart, pend))
                                { // need to check if there is an intersection
                                    Tuple<int, Point, Point> intrscts = BasicLib.CircleLineIntersect(pstart, pend, innerR);
                                    if ((intrscts.Item1 == 0) ||(intrscts.Item1 == 1))
                                    { // tangent so plot
                                         pf = GeneratePathFigure(pstart, pend);
                                    }
                                    else if (intrscts.Item1 == 2)
                                    {
                                        
                                        if (BasicLib.Between(pstart, intrscts.Item2, pend) &&
                                            BasicLib.Between(pstart, intrscts.Item3, pend))
                                        {
                                            Vector2 v1 = new Vector2(pstart, intrscts.Item2);
                                            Vector2 v2 = new Vector2(pstart, intrscts.Item3);
                                            if (v1.Length < v2.Length)
                                            {
                                                pf = GeneratePathFigure(pstart, intrscts.Item2);
                                                pg.Figures.Add(pf);
                                                pf = GeneratePathFigure(intrscts.Item3, pend);
                                            }
                                            else
                                            {
                                                pf = GeneratePathFigure(pstart, intrscts.Item3);
                                                pg.Figures.Add(pf);
                                                pf = GeneratePathFigure(intrscts.Item2, pend);
                                            }
                                        }
                                        else pf = GeneratePathFigure(pstart, pend);
                                    }
                                }
                                else
                                    pf = GeneratePathFigure(pstart, pend);
                            }
                            else  // here d2 lies outside outerR
                            {
                                if (BasicLib.IsVertical(pstart, pend))
                                {
                                    double y = BasicLib.Sgn(pstart.Y) * Math.Sqrt(outerR * outerR - pstart.X * pstart.X);
                                    pf = GeneratePathFigure(pstart, new Point(pstart.X, y));
                                }
                                else if (BasicLib.IsHorizontal(pstart, pend))
                                {
                                    double x = BasicLib.Sgn(pstart.X) * Math.Sqrt(outerR * outerR - pstart.Y * pstart.Y);
                                    pf = GeneratePathFigure(pstart, new Point(x, pstart.Y));
                                }
                                else
                                {
                                    intrsct = CircleLineIntersect(pstart, pend, outerR);
                                    pf = GeneratePathFigure(pstart, intrsct);
                                }
                            }
                        }
                        if (pf != null)
                            try
                            {
                                if (pg.Figures.Contains(pf))
                                {
                                    pg.Figures.Remove(pf);
                                }
                                pg.Figures.Add(pf);
                            }
                            catch
                            {

                            }
                    }
                    else  // ignore innerR
                    #region Ignore innerR
                    {
                        if ((d1 > outerR) && (d2 > outerR))
                        {
                            pf = null;
                        }
                        else if ((d1 <= outerR) && (d2 <= outerR))
                        {
                            pf = GeneratePathFigure(pstart, pend);
                        }
                        else // one point lies either side
                        { // outerR must lie between d2 - d1
                            // need to start from inner not nearest
                            pstart = (d1 < outerR) ? p1 : p2;
                            pend = (d1 < outerR) ? p2 : p1;

                            if (BasicLib.IsVertical(p1, p2))
                            {
                                double y = Math.Sign(pstart.Y) * Math.Sqrt(outerR * outerR - pstart.X * pstart.X);
                                pf = GeneratePathFigure(pstart, new Point(pstart.X, y));
                            }
                            else if (BasicLib.IsHorizontal(p1, p2))
                            {
                                double x = Math.Sign(pstart.X) * Math.Sqrt(outerR * outerR - pstart.Y * pstart.Y);
                                pf = GeneratePathFigure(pstart, new Point(x, pstart.Y));
                            }
                            else
                            {
                                intrsct = CircleLineIntersect(pstart, pend, outerR);
                                pf = GeneratePathFigure(pstart, intrsct);
                            }
                        }

                        if (pf != null)
                            pg.Figures.Add(pf);
                    } 
                    #endregion

                   
                }
                else
                {
                    pf = GeneratePathFigure(p1,p2);
                    pg.Figures.Add(pf);
                }
            }
            return path;
        }

        PathFigure GeneratePathFigure(Point p1, Point p2)
        {
            double incr = ld.Layout.ClipRange.Inc;
            PathFigure pf = new PathFigure();
            Point po = new Point(p1.X, p1.Y);
                
            //if (ld.Layout.Hypo) 
            //   Hypo(ref po);
            pf.StartPoint = po;
               
            double i = 0;
            Point p;
            PolyLineSegment pls = new PolyLineSegment();
            for (i = incr; i <= 1.0; i += incr)
            {
                p = Interp(p1, i, p2);
                //if (ld.Layout.Hypo)
                //    Hypo(ref p);
                pls.Points.Add(p);
            }
            pf.Segments.Add(pls);
            return pf; 
        }

   
        Point Interp(Point p1, double indx, Point p2)
        {
            double x = BasicLib.Linterp(p1.X, indx, p2.X);
            double y = BasicLib.Linterp(p1.Y, indx, p2.Y);
            return new Point(x,y);
        }
      

        public ToolPath CreateToolPath(PathData pd, double inc)
        {
            ToolPath tp;
            LatticeData l = pd as LatticeData;
            tp = new ToolPath(); //.BarrelOutline((int)(1 / inc)));
            return tp;
        }

        private double Hypo(double d)
        {
            double ksqd = ld.Layout.K * ld.Layout.K;
            return ld.Layout.WorkPieceRadius * d / Math.Sqrt(d * d + ksqd);
        }

        private void Hypo(ref Point p)
        {
            double mz = p.Radius();
            p.X = Hypo(p.X);
            p.Y = Hypo(p.Y);
        }
    }
}
