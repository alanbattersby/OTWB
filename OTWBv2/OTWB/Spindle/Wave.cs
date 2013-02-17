using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;

namespace Geometric_Chuck.Spindle
{
    public class Wave : Rosette
    {
        #region Properties
        double _f;
        [XmlElement]
        public double Frequency
        {
            get { return _f; }
            set { 
                    SetProperty(ref _f, value, "Frequency");
                    Name = SuggestedName;
                    _Period = 1 / Frequency;
            }
        }

        [XmlIgnore]
        public double _Period { get; set; }
    
        #endregion

        public Wave(double f, double weight, double phase )
            : base(weight,phase)
        {
            Frequency = f;
            SuggestedNameTemplate = "Wave-F{0}-W{1}-P{2}";
        }
        public Wave(double f, double w) : this(f, w, 0) { }
        public Wave(double f) : this(f, 1, 0) { }
        public Wave() : this(6, 1, 0) { }

        [XmlIgnore]
        protected override string SuggestedName
        {
            get { return string.Format(SuggestedNameTemplate, 
                    Math.Round(Frequency, ApproxToNDP), Weight, Phase); }
        }

        public override double OffsetAt(double param)
        {
            double angl = Frequency * (param * twopi + Phase_In_Radians);
            return Weight * Math.Sin(angl);
        }

    }
}
