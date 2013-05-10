using OTWB.Collections;
using OTWB.Interfaces;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace OTWB.PathGenerators
{
    class BazelyEngine : IPathGenerator
    {
        private const double ToRadians = Math.PI / 180;
        private const double Alpha = ToRadians * 360;
        private const double ToDegrees = 1 / ToRadians;

        private double FNR(double p)
        {
            return p * Math.Atan(1.0) / 45.0;
        }

        public ToolPath CreateToolPath(PathData pd, double inc)
        {
            BazelyChuck chuck = (pd as BazelyChuck);
            if (chuck.Stages.Count == 1)
                return  new ToolPath(OneStagePath(chuck, inc));
            else
                return new ToolPath(TwoStagePath(chuck, inc));
        }

        public ShapeCollection CreatePaths (PathData pathdata, double inc)
        {
            //Debug.WriteLine("BazelyEngine CreatePaths");
            ShapeCollection pc = new ShapeCollection();
            pc.PatternName = pathdata.Name;
            Polygon poly = Path(pathdata, inc);
            try
            {
                pc.AddShape(poly);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            //Debug.WriteLine("End BazelyEngine CreatePaths");
            return pc;
        }

        Windows.UI.Xaml.Shapes.Polygon Path(PathData pd, double inc)
        {
            Windows.UI.Xaml.Shapes.Polygon p = new Windows.UI.Xaml.Shapes.Polygon();
            p.Tag = "Bazley";
            //Debug.WriteLine("BazelyEngine Polygon Created");
            p.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            p.RenderTransform = new CompositeTransform();
            if (!(pd is BazelyChuck))
                return p;
            BazelyChuck chuck = (pd as BazelyChuck);
            //Debug.WriteLine("BazelyEngine Calc Points");
            if (chuck.Stages.Count == 1)
                p.Points = OneStagePath(chuck, inc);
            else
                p.Points = TwoStagePath(chuck, inc);
            //Debug.WriteLine("BazelyEngine Points created");
            CentrePolygon(ref p);
            return p;
        }

        private void CentrePolygon(ref Windows.UI.Xaml.Shapes.Polygon poly)
        {
            var CurrentExtent = new Extent2D(double.MaxValue, double.MinValue, double.MaxValue, double.MinValue);
            foreach (Point p in poly.Points)
                CurrentExtent.Update(p);
            Point cntr = CurrentExtent.Centre;
            for (var i = 0; i < poly.Points.Count(); i++)
            {
                poly.Points[i] = new Point(poly.Points[i].X - cntr.X,
                                            poly.Points[i].Y - cntr.Y);
            }
        }

        private PointCollection OneStagePath(BazelyChuck chuck, double inc)
        {
            PointCollection pnts = new PointCollection();
            BazelyStageData stage1 = chuck.Stages[0];
            var stage1Vnum = stage1.Vnum;
            var stage1Vden = stage1.Vden;
            var stage1PHI = FNR(stage1.PHI);

            var V = stage1Vnum / stage1Vden;

            if (!stage1.SameDirection)
                V *= -1;
            if (stage1Vden % stage1Vnum == 0)
                stage1Vden /= stage1Vnum;

            //pnts.Add(new Point(stage1.Ex * Math.Cos(0) + chuck.SR * Math.Cos(stage1PHI),
            //                   stage1.Ex * Math.Sin(0) + chuck.SR * Math.Sin(stage1PHI)));

            double maxangle = stage1.Vden * Alpha;
           
            for (double Theta = 0; Theta <= maxangle; Theta += inc)
            {
                pnts.Add(
                    new Point(stage1.Ex * Math.Cos(Theta) + chuck.SR * Math.Cos((1 + V) * Theta + stage1PHI),
                              stage1.Ex * Math.Sin(Theta) + chuck.SR * Math.Sin((1 + V) * Theta + stage1PHI))
                              );
            }
            return pnts;
        }
        private PointCollection TwoStagePath(BazelyChuck chuck, double inc)
        {
            PointCollection pnts = new PointCollection();
            BazelyStageData stage1 = chuck.Stages[0];
            BazelyStageData stage2 = chuck.Stages[1];

            var stage1Vnum = stage1.Vnum;
            var stage1Vden = stage1.Vden;

            var stage2Vnum = stage2.Vnum;
            var stage2Vden = stage2.Vden;

            var V1 = stage1Vnum / stage1Vden;
            var V2 = stage2Vnum / stage2Vden;

            double stage1PHI = FNR(ToRadians * stage1.PHI);
            double stage2PHI = FNR(ToRadians * stage2.PHI);

            if (stage2Vnum % stage1Vden == 0)
                stage2Vden /= stage1Vden;
            else if (stage2Vnum % stage1Vden == 11)
                stage2Vden = stage2Vden / stage1Vden * 3;

            double maxangle = stage1Vden * stage2Vden * Alpha;
            maxangle = Math.Max(maxangle, Alpha);
           
            for (double Theta = 0; Theta <= maxangle; Theta += inc)
            {
                //X = .Ex2 * Math.Cos(Theta) + .Ex1 * Math.Cos((1 + V2) * Theta + .PHI2) + .SR * Math.Cos((1 + V2 - (V1 * V2)) * Theta + .PHI1)
                //Y = .Ex2 * Math.Sin(Theta) + .Ex1 * Math.Sin((1 + V2) * Theta + .PHI2) + .SR * Math.Sin((1 + V2 - (V2 * V1)) * Theta + .PHI1)

                double x = stage2.Ex * Math.Cos(Theta) + stage1.Ex * Math.Cos((1 + V2) * Theta + stage2PHI) + chuck.SR * Math.Cos((1 + V2 - (V1 * V2)) * Theta + stage1PHI);
                double y = stage2.Ex * Math.Sin(Theta) + stage1.Ex * Math.Sin((1 + V2) * Theta + stage2PHI) + chuck.SR * Math.Sin((1 + V2 - (V2 * V1)) * Theta + stage1PHI);
                pnts.Add(new Point(x, y));
            }
            return pnts;
        }
    }
}
