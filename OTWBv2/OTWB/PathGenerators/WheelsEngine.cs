using Geometric_Chuck.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Geometric_Chuck.PathGenerators
{
    class WheelsEngine : IPathGenerator
    {
        public PolygonCollection CreatePaths(IPathData pathdata, double inc)
        {
            PolygonCollection pc = new PolygonCollection();
            pc.PatternName = pathdata.Name;
            pc.AddPoly(Path(pathdata, inc));
            return pc;
        }

        Windows.UI.Xaml.Shapes.Polygon Path(IPathData pd, 
                                            double inc)
        {
            CompositeTransform ct = new CompositeTransform();
            ct.ScaleX = ct.ScaleY = 1;
            Windows.UI.Xaml.Shapes.Polygon p = new Windows.UI.Xaml.Shapes.Polygon();
            p.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            p.RenderTransform = ct;
            if (!(pd is WheelsData))
                return p;
            WheelsData rd = (WheelsData)pd;
            p.Points = CalculateWheels(rd, inc);
            // now centre polygon 
            //CentrePolygon(ref p);
            return p;
        }

        private PointCollection CalculateWheels(WheelsData CurrentData, 
                                                double inc)
        {
            PointCollection pts = new PointCollection();
            var maxAngle = CurrentData.SuggestedMaxTurns * 2 * Math.PI; 
            for (double t = 0; t < maxAngle; t += inc)
            {
                Complex c = new Complex();
                foreach (WheelStageData sd in CurrentData.Stages)
                {
                    c += sd.Amplitude * Complex.Exp(new Complex(0,sd.Frequency * t ));
                }
                pts.Add(new Point(c.Real, c.Imaginary));
            }
            return pts;
        }

        private void CentrePolygon(ref Polygon poly)
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
    }
}
