using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using OTWB.Spindle;
using Windows.UI.Xaml.Media.Imaging;
using OTWB.Settings;
using OTWB.PathGenerators;
using OTWB.Coordinates;

namespace OTWB
{
    public class BasicLib
    {
        public static List<double> Increments
        {
            get
            {
                return new List<double> { 0.0001,
                                          0.00015,
                                          0.0002,
                                          0.00025,
                                          0.0003,
                                          0.00035,
                                          0.0004,
                                          0.00045,
                                          0.0005, 
                                          0.00055,
                                          0.0006,
                                          0.00065,
                                          0.0007,
                                          0.00075,
                                          0.0008,
                                          0.00085,
                                          0.0009,
                                          0.00095,
                                          0.001,
                                          0.0015,
                                          0.002,
                                          0.0025,
                                          0.003,
                                          0.0035,
                                          0.004,
                                          0.0045,
                                          0.005, 
                                          0.0055,
                                          0.006,
                                          0.0065,
                                          0.007,
                                          0.0075,
                                          0.008,
                                          0.0085,
                                          0.009,
                                          0.0095,
                                          0.01, 
                                          0.015,
                                          0.02,
                                          0.025,
                                          0.03,
                                          0.035,
                                          0.04,
                                          0.045,
                                          0.05,
                                          0.055,
                                          0.06,
                                          0.065,
                                          0.07,
                                          0.075,
                                          0.08,
                                          0.085,
                                          0.09,
                                          0.095,
                                          0.1, 
                                          0.15,
                                          0.2,
                                          0.25,
                                          0.3,
                                          0.35,
                                          0.4,
                                          0.45,
                                          0.5,
                                          0.55,
                                          0.6,
                                          0.65,
                                          0.7,
                                          0.75,
                                          0.8,
                                          0.85,
                                          0.9,
                                          0.95};
            }
        }

        public static int HCF(int[] numbers)
        {
            return numbers.Aggregate(HCF);
        }

        public static int HCF(int a, int b)
        {
            return b == 0 ? a : HCF(b, a % b);
        }

        public static int LCM(int a, int b)
        {
            return (a * b) / HCF(a, b);
        }
       
        public static double Linterp (double v1, double f, double v2)
        {
            return (1 - f) * v1 + f * v2;
        }

        /// <summary>
        /// a x2 + bx + c = 0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Tuple<double,double> Quadratic(double a, double b, double c)
        {
            double d = b * b - 4 * a * c;
            if (d < 0)
                return new Tuple<double, double>(double.NaN, double.NaN);
            else
            {
                double r = Math.Sqrt(d);
                double r1 = (-b + r) / (2 * a);
                double r2 = (-b - r) / (2 * a);
                return new Tuple<double, double>(r1, r2);
            }
            
        }

        public static double ToDegrees 
        {
            get { return 180.0 / Math.PI; }
        }

        public static double ToRadians
        {
            get { return Math.PI / 180.0; }
        }

        public static double EPS
        {
            get { return 0.0000001; }
        }

        public static int Quadrant(double angle)
        {
            if (angle < 0)
                return QuadrantACW(angle);
            else
                return QuadrantCW(angle);
        }

        public static int QuadrantCW(double angle)
        {
            return (int)(Math.Floor(angle / 90) % 4);
        }
        public static int QuadrantACW(double angle)
        {
            return 3 - (int)(Math.Floor(Math.Abs(angle) / 90) % 4);
        }

        public static bool Quadrant3To0(double astart, double aend)
        {
            return ((Quadrant(astart) == 3) && (Quadrant(aend) == 0));
        }

        public static bool Quadrant0To3(double astart, double aend)
        {
            return ((Quadrant(astart) == 0) && (Quadrant(aend) == 3));
        }

        public static int Quadrant(double x, double y)
        {
            return Quadrant(Math.Atan2(y, x) * ToDegrees);
        }

        static bool samesign (double x1, double x2)
        {
            if (x1 == x2)
                return true;
            else if (x1 == 0)
                return x2 > 0;
            else if (x2 == 0)
                return
                    x1 > 0;
            else
                return Math.Sign(x1) == Math.Sign(x2);
        }

        public static bool SameQuadrant(double x1, double y1, double x2, double y2)
        {
            return samesign(x1, x2) && samesign(y1, y2);
        }

        public static bool SameQuadrant(Point p1, Point p2)
        {
            return samesign(p1.X, p2.X) && samesign(p1.Y, p2.Y);
        }

        public static int Quadrant(Point p)
        {
            return Quadrant(p.X, p.Y);
        }

        public static double Sgn(double d)
        {
            if (d < 0)
                return -1;
            else
                return 1;
        }

        public static Tuple<int,Point, Point> CircleLineIntersect(double x1, double y1, double x2, double y2, double r)
        {
            double xr1, xr2, yr1, yr2;
            Point p1, p2;
            double dx = x2 - x1;
            double dy = y2 - y1;
            double drsq = dx * dx + dy * dy;
            double D = x1 * y2 - x2 * y1;
            double disc = r * r * drsq - D * D;
            if (disc < 0)
                return null;
            else if (disc == 0)
            {
                xr1 = D * dy / drsq;
                yr1 = -D * dx / drsq;
                p1 = new Point(xr1,yr1);
                return new Tuple<int,Point, Point>(1,p1,p1);
            }
            else
            {
                double delta = Math.Sqrt(disc);
                xr1 = (D * dy + Sgn(dy) * dx * delta) / drsq;
                xr2 = (D * dy - Sgn(dy) * dx * delta) / drsq;
                yr1 = (-D * dx + Math.Abs(dy) * delta) / drsq;
                yr2 = (-D * dx - Math.Abs(dy) * delta) / drsq;
                p1 = new Point(xr1,yr1);
                p2 = new Point(xr2,yr2);
                return new Tuple<int, Point, Point>(2,p1, p2);
            }
        }
        /*
         circle is (x -a)^2 + (y - b)^2 = r^2
         so in my case a = 0 and b = 0 
         line is y = mx + c
         so
         * 
         m = (y2-y1)/(x2-x1)
        c = (-m * x1 + y1)
         * 
        aprim = (1 + m ^ 2)
        bprim = 2 * m * (c - 0) - 2 * 0 = 2 * m * c
        cprim = a ^ 2 + (c - b) ^ 2 - r ^ 2 = 0^2 + (c - 0) ^2 - r ^2
              = c^2 - r^2
        
        delta = bprim ^ 2 - 4 * aprim * cprim

        x1_e_intersection = (-bprim + Math.Sqrt(delta)) / (2 * aprim)
        y1_e_intersection = m * x1_s_intersection + c

        x2_e_intersection = (-bprim - Math.Sqrt(delta)) / (2 * aprim)
        y2_e_intersection = m * x2_s_intersection + c
        */
        public static Tuple<int, Point, Point> CircleLineIntersect(Point p1, Point p2, double r)
        {
            double m = (p2.Y - p1.Y) / (p2.X - p1.X);
            double c = -m * p1.X + p1.Y;
            double ap = 1 + m * m;
            double bp = 2 * m * c;
            double cp = c * c - r * r;

            double disc = bp * bp - 4 * ap * cp;
            double xr1, yr1, xr2, yr2;
            if (disc < 0)
            {
                Point p = new Point(Double.PositiveInfinity, Double.PositiveInfinity);
                return new Tuple<int, Point, Point>(0, p, p);
            }
            else if (disc == 0)
            {
                xr1 = -bp / (2 * ap);
                yr1 = m * xr1 + c;
                p1 = new Point(xr1, yr1);
                return new Tuple<int, Point, Point>(1, p1, p1);
            }
            else
            {
                double delta = Math.Sqrt(disc);
                xr1 = (-bp + delta) / (2 * ap);
                xr2 = (-bp - delta) / (2 * ap);
                yr1 = m * xr1 + c;
                yr2 = m * xr2 + c;
                p1 = new Point(xr1, yr1);
                p2 = new Point(xr2, yr2);
                return new Tuple<int, Point, Point>(2, p1, p2);
            }

        }

        public static Tuple<double, double> Fraction(Point p1, Point p, Point p2)
        {
            double a1 = (p.X - p1.X) / (p2.X- p1.X);
            double a2 = (p.Y - p1.Y) / (p2.Y - p1.Y);
            return new Tuple<double, double>(a1, a2);
        }

        public static bool Between(Point p1, Point p, Point p2)
        {
            double a1 = (p.X - p1.X) / (p2.X - p1.X);
            double a2 = (p.Y - p1.Y) / (p2.Y - p1.Y);
            if (double.IsNaN(a1))
                return Between(0, a2, 1);
            else
                return Between(0, a1, 1);
        }

        public static bool Between(double s, double f, double e)
        {
            return (f >= s) && (f <= e);
        }

        public static double DistFromPointToLine(Point p0, Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;

            double b = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));     
            double t = Math.Abs(dx * (p1.Y - p0.Y)  - (p1.X - p0.X) * dy );

            return t / b;
        }

        public static bool IsHorizontal(Point p1, Point p2)
        {
            return p1.Y == p2.Y;
        }

        public static bool IsVertical(Point p1, Point p2)
        {
            return p1.X == p2.X;
        }

        public static bool IsDiagonal(Point p1, Point p2)
        {
            return (!IsHorizontal(p1, p2)) && (!IsVertical(p1, p2));
        }

        public static object GetSetting(string name)
        {
            Windows.Storage.ApplicationDataContainer localSettings
                = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values[name] == null)
                localSettings.Values[name] = DefaultSettings.Value(name);
            return
                localSettings.Values[name];
        }

        public static void SetSetting(string name, object value)
        {
             Windows.Storage.ApplicationDataContainer localSettings
                = Windows.Storage.ApplicationData.Current.LocalSettings;
             localSettings.Values[name] = value;
        }

        public static List<PageData> TopMenu 
        { 
            get
            {
                return new List<PageData> 
                {  
                    new PageData("Bazley Patterns","Example patterns from Bazley's book",typeof(MainPage),
                            new BitmapImage(new Uri("ms-appx:///Assets/BazelyIcon.png"))),
                    new PageData("Ross Patterns",null,typeof(RossPage), 
                            new BitmapImage(new Uri("ms-appx:///Assets/RossIcon.png"))),
                    new PageData("Wheels in Wheels",null,typeof(Wheels),
                            new BitmapImage(new Uri("ms-appx:///Assets/WheelsIcon.png"))),
                    new PageData("Rosettes",null,typeof(SpindlePage),
                            new BitmapImage(new Uri("ms-appx:///Assets/RoseIcon.png"))),
                    new PageData("Lattice Rim",null,typeof(LatticeRimPage),
                            new BitmapImage(new Uri("ms-appx:///Assets/LatticeIcon.png"))),
                    new PageData("Lattice Face",null,typeof(LatticeFacePage),
                            new BitmapImage(new Uri("ms-appx:///Assets/LatticeFaceIcon.png"))),
                    new PageData("Braid Rim",null,typeof(BraidPage),
                            new BitmapImage(new Uri("ms-appx:///Assets/BraidIcon.png"))),
                    new PageData("Profile",null,typeof(ProfilePage),
                            new BitmapImage(new Uri("ms-appx:///Assets/convex_profile.png"))),
                    new PageData("Simple Code",null, typeof(GcodePage),
                            new BitmapImage(new Uri("ms-appx:///Assets/code.png")))
                };
            } 
        
        }
    }
}
