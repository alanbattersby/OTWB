using OTWB.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Xml.Serialization;
using OTWB;

namespace OTWB.PathGenerators
{
    //public class PathFragment2D : List<Point>
    //{
    //    public PathFragment2D() : base() { }

    //    public PathFragment2D(Windows.UI.Xaml.Media.PointCollection pointCollection)
    //        : base()
    //    {
    //        foreach (Point p in pointCollection)
    //            this.Add(p);
    //    }

    //    public static PathFragment3D To3D(PathFragment2D tp)
    //    {
    //        return (PathFragment3D)tp.Select(p => new Cartesian(p.X,p.Y,0) );
    //    }
    //}

    //public class PathFragment3D : List<ICoordinate>
    //{
    //    public static PathFragment2D To2D(PathFragment3D tp)
    //    {
    //        return (PathFragment2D)tp.Select(p => new Point(p.toCartesian3.X, p.toCartesian3.Y));
    //    }

    //    public static PathFragment3D ToCylindrical(PathFragment3D tp)
    //    {
    //        return (PathFragment3D)tp.Select(p => p.toCylindrical);
    //    }
    //}

    public class PathFragment : List<Cartesian> 
    {
        public PathFragment(Windows.UI.Xaml.Media.PointCollection pointCollection)
            : base()
        {
            int winding = 0;
            double lastAngle = 0;
            foreach (Point p in pointCollection)
            {
                Cartesian c = new Cartesian(p.X, p.Y, 0, winding);
                if (BasicLib.Quadrant3To0(lastAngle, c.Angle))
                    winding += 1;
                else if (BasicLib.Quadrant0To3(lastAngle, c.Angle))
                    winding -= 1;
                c.WindingNumber = winding;
                Add(c);
                lastAngle = c.Angle;
            }
        }

        public PathFragment(List<Point> points)
            : base()
        {
            int winding = 0;
            double lastAngle = 0;
            foreach (Point p in points)
            {
                Cartesian c = new Cartesian(p.X, p.Y, 0, winding);
                if (BasicLib.Quadrant3To0(lastAngle, c.Angle))
                    winding += 1;
                else if (BasicLib.Quadrant0To3(lastAngle, c.Angle))
                    winding -= 1;
                c.WindingNumber = winding;
                Add(c);
                lastAngle = c.Angle;
            }
        }

        public PathFragment() : base() { }
      
        public PointCollection ToPointCollection
        {
            get
            {
                return (PointCollection)this.Select(p => p.AsPoint);
            }
        }

        public Extent2D Extent
        {
            get
            {
                var CurrentExtent = new Extent2D(double.MaxValue, double.MinValue, double.MaxValue, double.MinValue);
                foreach (ICoordinate c in this)
                    CurrentExtent.Update(c.toCartesian3);

                return CurrentExtent;
            }
        }
    }

    public class ToolPath : List<PathFragment> 
    {
        public string PatternName { get; set; }
       
        public ToolPath() : base() { }
        public ToolPath(PointCollection pc)
            : base()
        {
            PathFragment f = new PathFragment(pc);
            this.Add(f);
        }

        /// <summary>
        /// If only one fragment then assume it is a polygon
        /// </summary>
        [XmlIgnore]
        public Shape Visual
        {
            get
            {
                return (this.Count == 1) ? GenPoly() : GenPath();
            }

        }

        private Shape GenPath()
        {
            Polygon poly = new Polygon();

            return poly;
        }

        private Shape GenPoly()
        {
            Polygon poly = new Polygon();
            poly.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            poly.RenderTransform = new CompositeTransform();
            poly.Points = this[0].ToPointCollection;
            return poly;
        }

        public void ScaleBy (double s)
        {
            for (int indx = 0; indx < this.Count; indx++)
                this[indx] = (PathFragment)this[indx].Select(p => s * p);
        }
        public void Translate(Cartesian t)
        {
            for (int indx = 0; indx < this.Count; indx++)
                this[indx] = (PathFragment)this[indx].Select(p => t + p);
        }
        public void Translate(Point c)
        {
            for (int indx = 0; indx < this.Count; indx++)
                this[indx] = (PathFragment)this[indx].Select(p => 
                                      new Cartesian(p.X + c.X,
                                                    p.Y + c.Y,
                                                    p.Z));
        }
        public void Rotate(double angle)
        {
            for (int indx = 0; indx < this.Count; indx++)
                this[indx] = (PathFragment)this[indx].Select(p =>
                                  {
                                     Cylindrical c = p.toCylindrical;                                                     
                                     return new Cylindrical(c.Radius,
                                                            c.Angle + angle, 
                                                            c.Depth).toCartesian3;
                                  });
        }

        public Extent2D Extent
        {
            get
            {
                var CurrentExtent = new Extent2D(double.MaxValue, double.MinValue, double.MaxValue, double.MinValue);
                foreach (PathFragment f in this)
                    CurrentExtent.Merge(f.Extent);
                return CurrentExtent;
            }
        }
    }
}
