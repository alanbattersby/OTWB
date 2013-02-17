using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Geometric_Chuck.Spindle;
using Windows.UI.Xaml.Media.Imaging;
using OTWB.Settings;

namespace Geometric_Chuck
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
                    new PageData("Bazely","Example patterns from Bazely's book",typeof(MainPage),
                        new BitmapImage(new Uri("ms-appx:///Assets/BazelyIcon.png"))),
                    new PageData("Ross",null,typeof(RossPage), 
                        new BitmapImage(new Uri("ms-appx:///Assets/RossIcon.png"))),
                    new PageData("Wheels",null,typeof(Wheels),new BitmapImage(new Uri("ms-appx:///Assets/WheelsIcon.png"))),
                    new PageData("Rose",null,typeof(SpindlePage),
                        new BitmapImage(new Uri("ms-appx:///Assets/RoseIcon.png"))),
                    new PageData("Simple Code",null, typeof(GcodePage),null,true)
                };
            } 
        
        }
    }
}
