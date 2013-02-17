using Geometric_Chuck.Common;
using Geometric_Chuck.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Geometric_Chuck.Spindle
{
    /// <summary>
    /// A barrel holds a collection of Rosettes
    /// </summary>
    
   [XmlRoot("Barrel")]
    public class Barrel : BindableBase, IPathData
    {
        public enum CombineMethod
        {
            BLEND,
            MAXR,
            MINR
        }

        const double twopi = 2 * Math.PI;
        protected const double rad = Math.PI / 180;

        double _phase;
        public double Phase
        {
            get { return _phase; }
            set { SetProperty(ref _phase, value, "Phase"); }
        }

        
        double _toolPosn;
        public double ToolPosition
        {
            get { return _toolPosn; }
            set { SetProperty(ref _toolPosn, value, "ToolPosition"); }
        }

        
        CombineMethod _method;
        public CombineMethod CombinationRule
        {
            get { return _method; }
            set { SetProperty(ref _method, value, "CombinationRule"); }
        }

        ObservableCollection<Rosette> _rosettes;
        public ObservableCollection<Rosette> Rosettes
        {
            get { return _rosettes; }
            set { SetProperty(ref _rosettes, value, "Rosettes"); }
        }

        public Barrel(int pi)
        {
            Rosettes = new ObservableCollection<Rosette>();
            CombinationRule = CombineMethod.BLEND;
            PatternIndex = pi;
            ToolPosition = 0;
        }
        public Barrel() : this(0) { }

        public PolygonCollection Path(double inc)
        {
            PolygonCollection pc = new PolygonCollection();
            pc.AddPoly(Outline(inc));
            return pc;
        }

        public Windows.UI.Xaml.Shapes.Polygon Outline(double inc)
        {
            Windows.UI.Xaml.Shapes.Polygon p = new Windows.UI.Xaml.Shapes.Polygon();
            CompositeTransform ct = new CompositeTransform();
            ct.ScaleX = ct.ScaleY = 1;
            ct.Rotation = Phase;
            ScaleTransform sc = new ScaleTransform();
            sc.ScaleY = sc.ScaleX = 1;
          
            p.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            p.RenderTransform = ct;
            double phse = Phase;
            double lastr = 0;
            double r ;
            double tp; 
            double a = 0;
            Point pd;
            int N = (int)Math.Floor(1.0 / inc);
            for (double i = 1; i < N; i++ )
            {
                double f = i * inc;
                r = CalcR(f, 0);
                a = f * twopi;
                tp = ToolPosition + r - lastr;
                pd = new Point(tp * Math.Cos(a), tp * Math.Sin(a));
                p.Points.Add(pd);
                lastr = r;
            }
            r = CalcR(1.0, 0);
            a = twopi;
            tp = ToolPosition + r - lastr;
            pd = new Point(tp * Math.Cos(a), tp * Math.Sin(a));
            p.Points.Add(pd);
            return p;
        }

        public RadialOffsetPath OutlineAsOffsets(int nsteps)
        {
            RadialOffsetPath outline = new RadialOffsetPath();
            outline.NumPoints = nsteps;
            double inc = 1.0 / (nsteps - 1);
            outline.Increment = inc;
            double r = OffsetAt(0);
            double lastR = r;
            outline.Add(r);
            for (int i = 1; i < nsteps; i++)
            {
                r = OffsetAt(i * inc);
                outline.Add(r - lastR);
                lastR = r;
            }
            return outline;
        }

        private double OffsetAt(double angl)
        {
            double r = 0;

            foreach (Rosette ros in Rosettes)
            {
                double rr = ros.OffsetAt(angl);
                switch (CombinationRule)
                {
                    case CombineMethod.BLEND:
                        r += rr;
                        break;
                    case CombineMethod.MAXR:
                        if (Math.Abs(rr) > Math.Abs(r))
                            r = rr;
                        break;
                    case CombineMethod.MINR:
                        if (Math.Abs(rr) < Math.Abs(r))
                            r = rr;
                        break;
                }
            }
            return r;
        }

        private double CalcR(double i, double phase)
        {
            double r = 0;
           
            foreach (Rosette ros in Rosettes)
            {
                double rr = ros.OffsetAt(i + phase);
                switch (CombinationRule)
                {
                    case CombineMethod.BLEND:
                        r += rr;
                        break;
                    case CombineMethod.MAXR:
                        r = Math.Max(r, rr);
                        break;
                    case CombineMethod.MINR:
                        r = Math.Min(r, rr);
                        break;
                }
            }
            return r;
        }

        public string Name
        {
            get { return string.Format("Barrel#{0}", PatternIndex); }
        }

        public PatternType PathType
        {
            get { return PatternType.BARREL; }
        }

        int _patternindx;
        public int PatternIndex
        {
            get { return _patternindx;}
            set { SetProperty(ref _patternindx, value, "PatternIndex"); }
        }

        public double SuggestedMaxTurns
        {
            get { return 1; }
        }

        public void Add(Rosette ros)
        {
            ros.PropertyChanged += ros_PropertyChanged;
            _rosettes.Add(ros);
        }

        void ros_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }
    }
}
