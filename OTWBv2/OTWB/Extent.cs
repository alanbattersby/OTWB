using OTWB.Common;
using OTWB.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OTWB
{
    public class Extent2D : BindableBase
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }

        double _maxradius = 0.0;
        public double MaxRadius
        {
            get 
            { 
                MaxRadius = Radius;
                return _maxradius;
            }
            set { SetProperty(ref _maxradius, value, "MaxRadius"); }
        }

        double Radius
        {
            get {
                var xm = Math.Max(Math.Abs(MinX), Math.Abs(MaxX));
                var ym = Math.Max(Math.Abs(MinY), Math.Abs(MaxY));
                return Math.Sqrt(xm * xm + ym * ym); 
            }
        }


        public double MinRadius
        {
            get
            {
                var xm = Math.Min(Math.Abs(MinX), Math.Abs(MaxX));
                var ym = Math.Min(Math.Abs(MinY), Math.Abs(MaxY));
                return Math.Sqrt(xm * xm + ym * ym);
            }
        }

        public double Width
        {
            get { return MaxX - MinX; }
        }
        public double Height
        {
            get { return MaxY - MinY; }
        }

        public Extent2D(double minX = double.MaxValue, 
                      double maxX = double.MinValue, 
                      double minY = double.MaxValue, 
                      double maxY = double.MinValue)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public Extent2D() : this (double.MaxValue, double.MinValue, double.MaxValue, double.MinValue)
        {}

        public static Extent2D EmptyExtent
        {
            get { return new Extent2D(); }
        }

        public void MakeEmptyExtent()
        {
            MinX = MinY = double.MaxValue;
            MaxX = MaxY = double.MinValue;
        }

        public bool IsMaxExtent
        {
            get
            {
                return (this.MinX == double.MaxValue) &&
                       (this.MaxX == double.MinValue) &&
                       (this.MinY == double.MaxValue) &&
                       (this.MaxY == double.MinValue);
            }
        }

        public void Update(Point p)
        {
           if (p.X > MaxX )
                MaxX = p.X;
           else if (p.X < MinX )
                MinX = p.X;
           if (p.Y > MaxY)
                MaxY = p.Y;
           else if (p.Y < MinY)
               MinY = p.Y;
        }
        public void Update(Cartesian p)
        {
            if (p.X > MaxX)
                MaxX = p.X;
            else if (p.X < MinX)
                MinX = p.X;
            if (p.Y > MaxY)
                MaxY = p.Y;
            else if (p.Y < MinY)
                MinY = p.Y;
        }
 
        public Point Centre
        {
            get
            {
                return new Point((MaxX + MinX) / 2, (MaxY + MinY) / 2);
            }
        }

        public void Clear()
        {
            MinX = double.MaxValue;
            MaxX = double.MinValue; 
            MinY = double.MaxValue;
            MaxY = double.MinValue;
        }

        public override string ToString()
        {
            return string.Format("minX = {0} , maxX={1} , minY = {2} , maxY = {3}", MinX, MaxX, MinY, MaxY);
        }

        public void Merge(Extent2D ext)
        {
            if (ext.MaxX > MaxX)
                MaxX = ext.MaxX;
            if (ext.MinX < MinX)
                MinX = ext.MinX;
            if (ext.MaxY > MaxY)
                MaxY = ext.MaxY;
            if (ext.MinY < MinY)
                MinY = ext.MinY;
        }
    }
}
