using Geometric_Chuck.Common;
using OTWB.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTWB.PathGenerators
{
    public class PathTransform : BindableBase
    {
        double _sX;
        public double ScaleX
        {
            get { return _sX; }
            set { SetProperty(ref _sX, value); }
        }

        double _sY;
        public double ScaleY
        {
            get { return _sY; }
            set { SetProperty(ref _sY, value); }
        }

        double _transX;
        public double TranslateX
        {
            get { return _transX; }
            set { SetProperty(ref _transX, value); }
        }

        double _transY;
        public double TranslateY
        {
            get { return _transY; }
            set { SetProperty(ref _transY, value); }
        }

        double _transZ;
        public double TranslateZ
        {
            get { return _transZ; }
            set { SetProperty(ref _transZ, value); }
        }
 
        double _rot;
        public double Rotation
        {
            get { return _rot; }
            set { SetProperty(ref _rot, value); }
        }

        public PathTransform(double sx, double sy, double r, double tx, double ty, double tz)
        {
            ScaleX = sx;
            ScaleY = sy;
            Rotation = r;
            TranslateX = tx;
            TranslateY = ty;
            TranslateZ = tz;
        }

        public PathTransform() : this(1.0, 1.0, 0, 0,0,0) { }
    }
}
