using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OTWB.Spindle
{
    /// <summary>
    /// path composed of relative offsets defined by
    /// N equal steps around a single rotation
    /// So increrement is constant between steps
    /// </summary>
    public class RadialOffsetPath : OffsetPath<double>
    {   
        public RadialOffsetPath() : base() { }
    }

    public class PointOffsetPath : OffsetPath<Point>
    {
        public PointOffsetPath() : base() { }
    }
}
