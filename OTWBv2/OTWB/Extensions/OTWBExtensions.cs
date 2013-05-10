using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OTWB.Extensions
{
    public static class PointExtensions
    {
        public static double Radius(this Point p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

    }
}
