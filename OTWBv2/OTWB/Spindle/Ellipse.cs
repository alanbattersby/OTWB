using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Xml.Serialization;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI;

namespace Geometric_Chuck.Spindle
{
    public class Ellipse : Rosette
    {
        #region Properties
        double _e;
        [XmlElement]
        public double Eccentricity
        {
            get { return _e; }
            set { 
                    SetProperty(ref _e, value, "Eccentricity");
                    Name = SuggestedName;
            }
        }

        #endregion

        protected override string SuggestedName
        {
            get { return string.Format(SuggestedNameTemplate, 
                   Math.Round(Eccentricity, ApproxToNDP), 
                   Math.Round(Weight,ApproxToNDP), Phase); }
        }

        public Ellipse(double e, double weight,double phase )
            : base(weight, phase)
        {
            SuggestedNameTemplate = "Ellipse-E{0}-W{1}-P{2}";
            Eccentricity = e;       
        }
        public Ellipse(double e, double wt) : this(e, wt, 0) { }
        public Ellipse(double e) : this(e, 1, 0) { }
        public Ellipse() : this(1, 1, 0) { }

        /// <summary>
        /// param is a value between 0 and 1 rotation
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override double OffsetAt(double param)
        {
            double angl = param * twopi + Phase_In_Radians;
            double bcos = Eccentricity * Math.Cos(angl);
            double asin = Math.Sin(angl);
            return Weight * Eccentricity  / Math.Sqrt(bcos * bcos + asin * asin);
        }

    }
}
