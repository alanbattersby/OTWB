using OTWB.Collections;
using OTWB.Interfaces;
using OTWB.PathGenerators;
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

namespace OTWB.PathGenerators
{
    class WheelsEngine : IPathGenerator
    {
        public ToolPath CreateToolPath(PathData pd, double inc)
        {
            ToolPath tp;
            WheelsData rd = (WheelsData)pd;
            tp = new ToolPath(CalculateWheels(rd, inc));
            return tp;
        }

        public ShapeCollection CreatePaths(PathData pathdata, double inc)
        {
            ShapeCollection pc = new ShapeCollection();
            pc.PatternName = pathdata.Name;
            pc.AddShape(Path(pathdata, inc));
            return pc;
        }

        Windows.UI.Xaml.Shapes.Polygon Path(PathData pd, 
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

    }
}
