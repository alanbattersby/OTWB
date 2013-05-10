using OTWB.Common;
using OTWB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using System.Xml.Serialization;

namespace OTWB.Profiles
{
    [XmlInclude(typeof(ConcaveArcProfile))]
    [XmlInclude(typeof(ConvexArcProfile))]
    [XmlInclude(typeof(PointProfile))]
    public class Profile : BindableBase
    {
        protected double _offsetL;
        public double OffsetL
        {
            get { return _offsetL; }
            set { SetProperty(ref _offsetL, value); }
        }

        protected double _offsetR;
        public double OffsetR
        {
            get { return _offsetR; }
            set { SetProperty(ref _offsetR, value); }
        }

        protected Range _range;
        public Range Range
        {
            get { return _range; }
            set { SetProperty(ref _range, value); }

        }

        public virtual double Height(Coordinates.Cartesian c)
        {
            return Height(c.Length);
        }

        public virtual double Height(double x)
        {
            double f = (x - _range.Start) / _range.Length;
            return BasicLib.Linterp(_offsetL, f, _offsetR);
        }

        [XmlIgnore]
        public virtual Windows.UI.Xaml.Shapes.Shape Visual
        {
            get 
            {
                Line l = new Line();
                l.X1 = _range.Start;
                l.Y1 = _offsetL;
                l.X2 = Range.End;
                l.Y2 = _offsetR;
                return l;
            }
        }

        public Profile() : this (new Range(-100, 5, 100), 0) { }
        public Profile(double offset) : this(new Range(-100, 5, 100),offset) { }
        public Profile(Range r, double offset)
        {
            _offsetL = _offsetR = offset;
            _range = r;
        }
        public Profile(double offsetl, double offsetr)
            : this (new Range(-100, 5, 100),offsetl,offsetr) { }
        public Profile(Range r, double offsetl, double offsetr)
        {
            _offsetL = offsetl;
            _offsetR = offsetr;
            _range = r;
        }
    }
}
