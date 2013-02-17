using Geometric_Chuck.Spindle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Geometric_Chuck.Interfaces
{
    interface IRosette
    {
        double Phase { get; set; }
        double Weight { get; set; }
        string Name { get; }
        double OffsetAt(double param);
        Point PointAt(double param);
        Windows.UI.Xaml.Shapes.Polygon Path(double inc);
        RadialOffsetPath OutlineAsOffsets(int nsteps);
    }
}
