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
    public class SquareWave : Wave
    {

        public SquareWave(double f, double weight, double phase )
            : base(f,weight,phase)
        {
            SuggestedNameTemplate = "SquareWave-F{0}-W{1}-P{2}";
        }
        public SquareWave(double f, double w) : this(f, w, 0) { }
        public SquareWave(double f) : this(f, 1, 0) { }
        public SquareWave() : this(10, 1, 0) { }

        public override double OffsetAt(double param)
        {
            double a = param + Phase_In_Radians;
            double v = a - Math.Floor(a / _Period) * _Period;
            return wv(v/_Period);
        }

        double wv(double p)
        {
            if (p <= 0.5)
                return Weight;
            else
                return 0;
        }
    }
}
