using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Geometric_Chuck.Spindle
{
    public class Poly : Rosette
    {
        
        #region Properties
     
        int _n;
        public int N
        {
            get { return _n; }
            set { 
                SetProperty(ref _n, value, "N");
                Name = SuggestedName;
            }
        }
     
        #endregion

         public Poly(int n, double phase, double weight)
        {
            N = n;
            Phase = phase;
            Weight = weight;
            SuggestedNameTemplate = "Polygon-N{0}-W{1}-P{2}";
        }
        public Poly(int n, double ph) : this(n, ph, 1) { }
        public Poly(int n ) : this(n, 0, 1) { }
        public Poly() : this(3, 0, 1) { }

        // ref see http://tpfto.wordpress.com/2011/09/15/parametric-equations-for-regular-and-reuleaux-polygons/

        public override double OffsetAt(double param)
        {
            double angl = param * twopi + Phase_In_Radians;
            double f = 2 * Math.Floor(N * angl / twopi) + 1;
            double sec = 1 / Math.Cos(angl - Math.PI/N * f);
            return Weight * Math.Cos(Math.PI / N) * sec;
        }

        protected override string SuggestedName
        {
            get { return string.Format(SuggestedNameTemplate, 
                                       N, 
                                       Math.Round(Weight,ApproxToNDP), 
                                       Phase); }
        }
    }
}
