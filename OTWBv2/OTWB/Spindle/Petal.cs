using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTWB.Spindle
{
    public class Petal : Wave
    {
         public Petal(double f, double weight, double phase )
            : base(f, weight,phase)
        {
            SuggestedNameTemplate = "Petal-F{0}-W{1}-P{2}";
        }
        public Petal(double f, double w) : this(f, w, 0) { }
        public Petal(double f) : this(f, 1, 0) { }
        public Petal() : this(1, 1, 0) { }

        public override double OffsetAt(double param)
        {
            return Math.Abs(base.OffsetAt(param));
        }
    }
}
