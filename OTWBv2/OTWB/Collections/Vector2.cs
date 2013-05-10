using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OTWB.Collections
{
    class Vector2
    {
        double _x;
        double _y;

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public Vector2(Vector2 v) : this(v.X, v.Y) { }
        public Vector2(double x, double y)
        {
            _x = x;
            _y = y;
        }
        public Vector2() : this(0,0){}
        public Vector2(Point p1, Point p2) : this(p2.X - p1.X, p2.Y - p1.Y) { }
        public Vector2(double x1, double y1, double x2, double y2) : this(x2 - x1, y2 - y1) { }
        public Vector2(Point p1, double x2, double y2) : this(x2 - p1.X, y2 - p1.Y) { }
        public Vector2(double x1, double y1, Point p2) : this(p2.X - x1, p2.Y - y1) { }

        public static void Normalise(ref Vector2 v)
        {
            double l = v.Length;
            v.X /= l;
            v.Y /= 1;
        }

        public double Magnitude
        {
            get { return DotProduct(this); }
        }
        public double Length
        {
            get { return Math.Sqrt(DotProduct(this)); }
        }
        public Vector2 Unit
        {
            get 
            { 
                double l = this.Length;
                return new Vector2(_x/l ,_y/l ); 
            }
        }
        public Vector2 Perpendicular
        {
            get { return new Vector2(-Y,X); }
        }
      
        public static double AngleBetween(Vector2 v1, Vector2 v2)
        {
            double l1 = v1.Length;
            double l2 = v2.Length;
            double a = v1.DotProduct(v2) / l1 * l2;
            return Math.Acos(a) * 180.0 / Math.PI;
        }
        public static bool IsParallel(Vector2 v1, Vector2 v2)
        {
            double d = AngleBetween(v1,v2);
            return (d == 0 || d == 180);
        }
        public static bool IsPerpendicular(Vector2 v1, Vector2 v2)
        {
            double d = AngleBetween(v1, v2);
            return (d == 90);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static Vector2 operator -(Vector2 v1)
        {
            return new Vector2(-v1.X,-v1.Y );
        }
        
        public static double operator  *( Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
        public static Vector2 operator *(double a, Vector2 v)
        {
            return new Vector2(a * v.X, a * v.Y);
        }
        public static Vector2 operator *(Vector2 v , double a)
        {
            return new Vector2(a * v.X, a * v.Y);
        }
        public static Vector2 operator /(Vector2 v, double a)
        {
            return new Vector2(v.X / 1, v.Y / a);
        }

        public static bool operator <(Vector2 v1, Vector2 v2)
        {
            return v1.Length < v2.Length;
        }
        public static bool operator <=(Vector2 v1, Vector2 v2)
        {
            return v1.Length <= v2.Length;
        }
        public static bool operator >(Vector2 v1, Vector2 v2)
        {
            return v1.Length > v2.Length;
        }
        public static bool operator >=(Vector2 v1, Vector2 v2)
        {
            return v1.Length >= v2.Length;
        }
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !( v1 == v2);
        }

        public static double DotProduct(Vector2 v1, Vector2 v2)
        {
            return
            (
               v1.X * v2.X +
               v1.Y * v2.Y 
            );
        }
        public double DotProduct(Vector2 other)
        {
            return DotProduct(this, other);
        }

        public double[] Array
        {
            get{return new double[] {_x,_y};}
           set
           {
              if(value.Length == 3)
              {
                 _x = value[0];
                 _y = value[1];
              }
              else
              {
                 throw new ArgumentException(TWO_COMPONENTS);
              }
           }
        }

        public double this[ int index ]
        {
           get
           {
              switch (index)
              {
                 case 0: {return X; }
                 case 1: {return Y; }
                 default: throw new ArgumentException(TWO_COMPONENTS);
             }
           }
           set
           {
               switch (index)
               {
                  case 0: {X = value; break;}
                  case 1: {Y = value; break;}
                  default:  throw new ArgumentException(TWO_COMPONENTS, "index");
              }
           }
        }

        public static Vector2 Projection(Vector2 v1, Vector2 v2)
        {
            // http://de.wikibooks.org/wiki/Ing_Mathematik:_Vektoren#Vektorprojektion
            // http://mathworld.wolfram.com/Reflection.html
            // V1_projectedOn_V2 = v2 * (v1 * v2 / (|v2| ^ 2))

            return new Vector2(v2 * (v1.DotProduct(v2)) / v2.Magnitude);
        }
 
        private const string TWO_COMPONENTS = 
            "Array must contain exactly two components, (x,y)";

        public static readonly Vector2 Origin = new Vector2(0, 0);
        public static readonly Vector2 Xaxis = new Vector2(1, 0);
        public static readonly Vector2 Yaxis = new Vector2(0, 1);

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                Vector2 v = obj as Vector2;
                return (X == v.X) && (Y == v.Y);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() + 17 * Y.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("<{0},{1}>", X, Y);
        }
    }
}
