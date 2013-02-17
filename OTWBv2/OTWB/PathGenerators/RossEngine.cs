using Geometric_Chuck.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Geometric_Chuck.PathGenerators
{
    class RossEngine : IPathGenerator
    {
        private double FNR(double p)
        {
            return p * Math.Atan(1.0) / 45.0;
        }

        public PolygonCollection CreatePaths(IPathData pathdata, double inc)
        {
            PolygonCollection pc = new PolygonCollection();
            pc.PatternName = pathdata.Name;
            pc.AddPoly(Path(pathdata, inc));
            return pc;
        }

        Windows.UI.Xaml.Shapes.Polygon Path(IPathData pd, double inc)
        {
            CompositeTransform ct = new CompositeTransform();
            ct.ScaleX = ct.ScaleY = 1;
            Windows.UI.Xaml.Shapes.Polygon p = new Windows.UI.Xaml.Shapes.Polygon();
            p.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            p.RenderTransform = ct;
            if (!(pd is RossData))
                return p;
            RossData rd = (RossData)pd;
            if (rd.Ex2 == 0)
                p.Points = RossCalculate1(rd,  inc);
            else
                p.Points = RossCalculate2(rd, inc);

            // now centre polygon 
            CentrePolygon(ref p);
            return p;
        }
        private void CentrePolygon(ref Windows.UI.Xaml.Shapes.Polygon poly)
        {
            var CurrentExtent = new Extent(double.MaxValue, double.MinValue, double.MaxValue, double.MinValue);
            foreach (Point p in poly.Points)
                CurrentExtent.Update(p);
            Point cntr = CurrentExtent.Centre;
            for (var i = 0; i < poly.Points.Count(); i++)
            {
                poly.Points[i] = new Point(poly.Points[i].X - cntr.X,
                                            poly.Points[i].Y - cntr.Y);
            }
        }
        private PointCollection RossCalculate1(RossData CurrentData, double inc)
        {
            
            PointCollection points = new PointCollection();
            var maxAngle = CurrentData.SuggestedMaxTurns * 2 * Math.PI;
            var X = 0.0;
            var Y = 0.0;
            var N = (CurrentData.N == -CurrentData.N)
                ? CurrentData.N - 1.0
                : CurrentData.N + 1.0;
                        
            var Phi1 = FNR(CurrentData.Phi1);
            var Phi2 = FNR(CurrentData.Phi2);
            var Phi3 = FNR(CurrentData.Phi3);

            X = CurrentData.Ex1 * Math.Cos(0) + CurrentData.SR * Math.Cos(Phi1) + CurrentData.Fl * Math.Cos(Phi2) + CurrentData.Fr * Math.Cos(Phi3);
            Y = CurrentData.Ex1 * Math.Sin(0) + CurrentData.SR * Math.Sin(Phi1) + CurrentData.Fl * Math.Sin(Phi2) + CurrentData.Fr * Math.Sin(Phi3);
            points.Add(new Point(X, Y));

            for (var Theta = inc; Theta <= maxAngle; Theta += inc)
            {
                X = CurrentData.Ex1 * Math.Cos(Theta) + CurrentData.SR * Math.Cos(N * Theta + Phi1) + CurrentData.Fl * Math.Cos(CurrentData.M * Theta + Phi2) + CurrentData.Fr * Math.Cos(CurrentData.L * Theta + Phi3);
                Y = CurrentData.Ex1 * Math.Sin(Theta) + CurrentData.SR * Math.Sin(N * Theta + Phi1) + CurrentData.Fl * Math.Sin(CurrentData.M * Theta + Phi2) + CurrentData.Fr * Math.Sin(CurrentData.L * Theta + Phi3);
                points.Add(new Point(X, Y));
            }
            return points;
        }
        private PointCollection RossCalculate2(RossData CurrentData,  double inc)
        {
           
            PointCollection PointList = new PointCollection();
            var X = 0.0;
            var Y = 0.0;
            var maxAngle = CurrentData.SuggestedMaxTurns * 2 * Math.PI;
            X = CurrentData.Ex2 * Math.Cos(0) + CurrentData.Ex1 * Math.Cos(0) + CurrentData.SR * Math.Cos(0) + CurrentData.Fl * Math.Cos(0) + CurrentData.Fr * Math.Cos(0);
            Y = CurrentData.Ex2 * Math.Sin(0) + CurrentData.Ex1 * Math.Sin(0) + CurrentData.SR * Math.Sin(0) + CurrentData.Fl * Math.Sin(0) + CurrentData.Fr * Math.Sin(0);
            PointList.Add(new Point(X, Y));

            for (var Theta = inc; Theta <= maxAngle; Theta += inc)
            {
                X = CurrentData.Ex2 * Math.Cos(Theta) + CurrentData.Ex1 * Math.Cos((1 + CurrentData.V4) * Theta) + CurrentData.SR * Math.Cos(CurrentData.M * Theta) + CurrentData.Fl * Math.Cos(CurrentData.L * Theta) + CurrentData.Fr * Math.Cos(CurrentData.K * Theta);
                Y = CurrentData.Ex2 * Math.Sin(Theta) + CurrentData.Ex1 * Math.Sin((1 + CurrentData.V4) * Theta) + CurrentData.SR * Math.Sin(CurrentData.M * Theta) + CurrentData.Fl * Math.Sin(CurrentData.L * Theta) + CurrentData.Fr * Math.Sin(CurrentData.K * Theta);
                PointList.Add(new Point(X, Y));
            }
            return PointList;
        }
    }
}
