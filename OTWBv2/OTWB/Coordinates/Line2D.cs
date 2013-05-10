using OTWB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using System.Xml.Serialization;

namespace OTWB.Coordinates
{
    public class Line2D : BindableBase
    {
        double _x1;
        public double X1
        {
            get { return _x1; }
            set { SetProperty(ref _x1, value); }
        }

        double _y1;
        public double Y1
        {
            get { return _y1; }
            set { SetProperty(ref _y1, value); }
        }

        double _x2;
        public double X2
        {
            get { return _x2; }
            set { SetProperty(ref _x2, value); }
        }

        double _y2;
        public double Y2
        {
            get { return _y2; }
            set { SetProperty(ref _y2, value); }
        }

        public Line2D(double xs, double ys, double xe, double ye)
        {
            _x1 = xs;
            _y1 = ys;
            _x2 = xe;
            _y2 = ye;
        }
        public Line2D() : this(0, 0, 0, 0) { }
        public Line2D(Point from, Point to)
        {
            _x1 = from.X;
            _y1 = from.Y;
            _x2 = to.X;
            _y2 = to.Y;

        }

        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlIgnore]
        public Line Visual
        {
            get
            {
                Line l = new Line();
                l.Name = Name;
                l.X1 = _x1;
                l.Y1 = _y1;
                l.X2 = _x2;
                l.Y2 = _y2;
                return l;
            }
        }

        [XmlIgnore]
        public bool IsHorizontal
        {
            get { return _y1 == _y2; }
        }

        [XmlIgnore]
        public bool IsVertical
        {
            get { return _x1 == _x2; }
        }
    }
}
