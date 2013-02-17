using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;

namespace Geometric_Chuck.Interfaces
{
    interface IPathGenerator
    {
        PolygonCollection CreatePaths(IPathData p, double increment);
    }
}
