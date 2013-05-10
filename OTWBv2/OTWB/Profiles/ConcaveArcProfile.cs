using OTWB.Common;
using OTWB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using System.Xml.Serialization;

namespace OTWB.Profiles
{
    public class ConcaveArcProfile : Profile
    {
        double _h;
        public double MaxHeight
        {
            get { return _h; }
            set { SetProperty(ref _h, value); }
        }

        public double Chord
        {
            get { return _range.Length / 2; }
        }

        public ConcaveArcProfile() : this(200, 20) { }
        public ConcaveArcProfile(double l, double h)
            : base(new Range(-l/2,0.1,l/2),0)
        {
            _h = h;
        }

        public override double Height(Coordinates.Cartesian c)
        {
            return Height(c.Length);
        }

        [XmlIgnore]
        public override Windows.UI.Xaml.Shapes.Shape Visual
        {
            get
            {
                Polyline p = new Polyline();
                double x = _range.Start;
                do
                {
                    p.Points.Add(new Point(x, Height(x)));
                    x += _range.Inc;

                } while (x <= _range.End);

                return p;
            }
        }


        public override double Height(double x)
        {
            double H = _offsetL;
            double ch = Chord;

            if (Math.Abs(x) == ch)
                return H;
            else if (Math.Abs(x) > ch)
                return H;
            else
            {
                double R = (_h * _h + ch * ch) / (2 * _h);
                double b = 2 * (R - _h);
                double c = x * x - 2 * R * _h + _h * _h;
                Tuple<double, double> r = BasicLib.Quadratic(1, b, c);
                if (r.Item1 > 0)
                    return H + r.Item1;
                else
                    return H + r.Item2;
            }
        }

       
    }
}
