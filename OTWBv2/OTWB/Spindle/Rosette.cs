using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using System.Xml.Serialization;
using Geometric_Chuck.Interfaces;

namespace Geometric_Chuck.Spindle
{
    [XmlInclude(typeof(SpurGear))]
    [XmlInclude(typeof(Poly))]
    [XmlInclude(typeof(Wave))]
    [XmlInclude(typeof(SquareWave))]
    [XmlInclude(typeof(Ellipse))]
    [XmlInclude(typeof(Petal))]
    public class Rosette  : BindableBase, IRosette
    {
        protected const double rad = Math.PI / 180;
        protected const double twopi = 2 * Math.PI;
        protected const double deg = 180 / Math.PI;

        #region Properties
        int _ndp;
        public int ApproxToNDP
        {
            get { return _ndp; }
            set { SetProperty(ref _ndp, value,"NDP"); }
        }
        
        double _phase;
        [XmlElement]
        public double Phase
        {
            get { return _phase; }
            set 
            { 
                SetProperty(ref _phase, value, "Phase");
                Name = SuggestedName;
            }
        }

        [XmlIgnore]
        protected double Phase_In_Radians
        {
            get { return Phase * rad; }
        }

        double _weight;
        [XmlElement]
        public double Weight
        {
            get { return _weight; }
            set 
            { 
               SetProperty(ref _weight, value, "Weight");
               Name = SuggestedName;
            }
        }
        string _nametmpl;
        [XmlElement]
        protected string SuggestedNameTemplate
        {
            get { return _nametmpl; }
            set { SetProperty(ref _nametmpl, value, ""); }
        }

        string _name;
        [XmlIgnore]
        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        #endregion

       
        protected virtual string SuggestedName
        {
            get { return string.Format(SuggestedNameTemplate, Weight, Phase); }
        }

        public Rosette(double weight, double phase)
        {
            SuggestedNameTemplate = "-W{0}-P{1}";
            Weight = weight;
            Phase = phase;
            ApproxToNDP = 3;
        }

        public Rosette(double weight) : this(weight, 0) { }
        public Rosette() : this(1, 0) { }

        public virtual double OffsetAt(double param)
        {
           return Weight;
        }

        public Windows.Foundation.Point PointAt(double param)
        {
            double angl = param * twopi;
            double r = OffsetAt(param);
            return new Point(r * Math.Cos(angl), r * Math.Sin(angl));
        }

        public Windows.UI.Xaml.Shapes.Polygon Path(double inc)
        {
            Windows.UI.Xaml.Shapes.Polygon p = new Windows.UI.Xaml.Shapes.Polygon();
            ScaleTransform sc = new ScaleTransform();
            sc.ScaleY = sc.ScaleX = 1;
            p.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            p.RenderTransform = sc;
            for (double i = 0; i <= 1.0; i += inc)
            {
                p.Points.Add(PointAt(i));
            }
            return p;
        }

        public RadialOffsetPath OutlineAsOffsets(int nsteps)
        {
            RadialOffsetPath outline = new RadialOffsetPath();
            outline.NumPoints = nsteps;
            double inc = 1 / (nsteps - 1);
            outline.Increment = inc;
            double lastR = 0;
            outline.Add(0);
            for (int i = 1; i < nsteps; i++)
            {
                double r = OffsetAt(i * inc);
                outline.Add( r - lastR);
                lastR = r;
            }
            return outline;
        }

    }
}
