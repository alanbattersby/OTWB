using Geometric_Chuck.Interfaces;
using Geometric_Chuck.Spindle;
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
    class OffsetPathEngine : IPathGenerator
    {
        /// <summary>
        /// In this class the second parameter is the number of paths
        /// </summary>
        /// <param name="pathdata"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public PolygonCollection CreatePaths(IPathData pathdata, double increment)
        {
            PolygonCollection pc = new PolygonCollection();
            pc.PatternName = pathdata.Name;
            if (pathdata.PathType == PatternType.BARREL)
            {
                Barrel b = pathdata as Barrel;
                RadialOffsetPath path = b.OutlineAsOffsets((int)(1/increment));
                Polygon poly = CreatePolygonFrom(path, b.ToolPosition);
                (poly.RenderTransform as CompositeTransform).Rotation = b.Phase;
                pc.AddPoly(poly);
            }
            return pc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param> 
        /// <param name="x"></param>  X coordinate of starting point
        /// <param name="y"></param>  Y coordinate of starting point
        /// <returns></returns>
        public Polygon CreatePolygonFrom(RadialOffsetPath path, double rstart = 0)
        {
            Polygon p = new Polygon();
            CompositeTransform ct = new CompositeTransform();
            p.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            p.RenderTransform = ct;
            double lastR = rstart;
            for (int i = 0; i < path.NumPoints; i++)
            {
                double angle = i * path.Increment * 2 * Math.PI;
                double r = lastR + path[i];
                double x = Math.Round(r * Math.Cos(angle),3);
                double y = Math.Round(r * Math.Sin(angle), 3);
                Point nxtPoint = new Point(x,y );
                p.Points.Add(nxtPoint);
                lastR = r;
            }
            return p;
        }
    }
}
