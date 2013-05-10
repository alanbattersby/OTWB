using OTWB.Collections;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;

namespace OTWB.Interfaces
{
    interface IPathGenerator
    {
        ShapeCollection CreatePaths(PathData p, double increment);
        ToolPath CreateToolPath(PathData pd, double inc);
    }
}
