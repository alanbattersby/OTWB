using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using OTWB.Common;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI;

namespace OTWB
{
   
    public class Grid : BindableBase
    {
        public enum GRIDTYPE {CARTESIAN,RADIAL};
        public string Name { get; set; }

        protected Range _dim1;
        public Range Dim1
        {
            get { return _dim1; }
            set {
                //if (_dim1 != null)
                //    _dim1.PropertyChanged -= _dim_PropertyChanged;
                SetProperty(ref _dim1, value);
                //_dim1.PropertyChanged += _dim_PropertyChanged;
                CreateGrid();
            }
        }

        protected Range _dim2;
        public Range Dim2
        {
            get { return _dim2; }
            set {
                //if (_dim2 != null)
                //    _dim2.PropertyChanged -= _dim_PropertyChanged;
                SetProperty(ref _dim2, value);
                //_dim2.PropertyChanged += _dim_PropertyChanged;
                CreateGrid();
            }
        }

        Color _fgd;
        public Color Foreground
        {
            get { return _fgd; }
            set { SetProperty(ref _fgd, value); }
        }
        public virtual Path CreateGrid()
        {
            Path _outline = new Path();
            _outline.Name = ((Name == null || Name == string.Empty)) ? "GRID" : Name;
            _outline.RenderTransform = new CompositeTransform();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Foreground;
            _outline.Stroke = mySolidColorBrush;
            _outline.StrokeThickness = 0.5;
            return _outline;
        }

        protected Path _outline;
        GRIDTYPE _gtype;

        public Grid(GRIDTYPE _typ, Range dim1, Range dim2)
        {
            _gtype = _typ;
            _dim1 = dim1;       
            _dim2 = dim2;        
            Foreground = Colors.Blue;
            CreateGrid();
            _dim1.PropertyChanged += _dim_PropertyChanged;
            _dim2.PropertyChanged += _dim_PropertyChanged;
        }

        void _dim_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }

        public Grid() : this(GRIDTYPE.RADIAL,
                             new Range(0,0.1,1),
                             new Range(0,20,360)) { }

        public Path Outline
        {
            get 
            {
                if (_outline == null)
                    _outline = CreateGrid();
                return _outline; 
            }
            set { SetProperty(ref _outline, value, "Outline"); }
        }

        public void Update()
        {
            _outline = CreateGrid();
        }
    }

    public class RadialGrid : Grid
    {

        public RadialGrid(Range r)
            : base(GRIDTYPE.RADIAL,r,new Range(0,20,360)) 
        {
            //Outline = CreateGrid();
            Outline.Name = "RADIAL_GRID";
        }

        public RadialGrid()
            : base(GRIDTYPE.RADIAL, new Range(0, 0.1, 1), new Range(0, 20, 360)) 
        { 
            //Outline = CreateGrid();
            Outline.Name = "RADIAL_GRID";
        }

        public override Path CreateGrid()
        {
            Path _outline = base.CreateGrid();
            _outline.Name = "RADIAL_GRID";
            if (Dim1 == null || Dim2 == null) return _outline;
            GeometryGroup gg = new GeometryGroup();
            Point origin = new Point(0, 0);
            double inc = (_dim1.Inc <= 0) ? 5 : _dim1.Inc;
            for (double r = _dim1.Start; r <= _dim1.End; r += inc)
            {
                EllipseGeometry eg = new EllipseGeometry();
                eg.Center = origin;
                eg.RadiusX = eg.RadiusY = r;
                gg.Children.Add(eg);
            }

            inc = (_dim2.Inc <= 0) ? 5 : _dim2.Inc;

            for (double a = _dim2.Start; a <= _dim2.End; a += _dim2.Inc)
            {
                double aa = a * Math.PI / 180;
                double xs = _dim1.Start * Math.Cos(aa);
                double ys = _dim1.Start * Math.Sin(aa);
                Point start = new Point(xs, ys);
                double xe = _dim1.End * Math.Cos(aa);
                double ye = _dim1.End * Math.Sin(aa);
                Point end = new Point(xe, ye);
                LineGeometry lg = new LineGeometry();
                lg.StartPoint = start;
                lg.EndPoint = end;
                gg.Children.Add(lg);
            }
            _outline.Data = gg;
            return _outline;
        }
    }

    public class CartesianGrid : Grid
    {
        public CartesianGrid()
            : base(GRIDTYPE.CARTESIAN, new Range(0, 0.1, 1), new Range(0, 0.1, 1)) 
        {
            //Outline = CreateGrid();
            Outline.Name = "CARTESIAN_GRID";
        }

        public CartesianGrid(Range d1, Range d2)
            : base(GRIDTYPE.CARTESIAN,d1,d2)
        {
            //Outline = CreateGrid();
            Outline.Name = "CARTESIAN_GRID";
        }

        public override Path CreateGrid()
        {
            Path _outline = base.CreateGrid();
            _outline.Name = "CARTESIAN_GRID";
            PathGeometry pg = new PathGeometry();
            double startx = Math.Floor(_dim1.Start / _dim1.Inc) * _dim1.Inc;
            double starty = Math.Floor(_dim2.Start / _dim2.Inc) * _dim2.Inc;
            double endx = Math.Ceiling(_dim1.End / _dim1.Inc) * _dim1.Inc;
            double endy = Math.Ceiling(_dim2.End / _dim2.Inc) * _dim2.Inc;

            pg.Figures = new PathFigureCollection();
            // Here we start to create the rows which are horizontal
            // and range over Y which is dim2 
            // from (dim1.Start,y) -> (dim1.end,y)
            // where y ranges from dim2.start to dim2.end
            #region Create rows
            if (_dim2.Start >= 0)
            {
                for (double y = starty; y <= endy; y += _dim2.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(startx, y);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(endx, y);
                    pf.Segments.Add(l);
                    pg.Figures.Add(pf);
                }
            }
            else
            {
                for (double y = 0; y >= starty; y -= _dim2.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(startx, y);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(endx, y);
                    pf.Segments.Add(l);
                    pg.Figures.Add(pf);
                }

                if (_dim2.End > 0)
                {
                    for (double y = 0; y <= endy; y += _dim2.Inc)
                    {
                        PathFigure pf = new PathFigure();
                        pf.StartPoint = new Point(startx, y);
                        pf.Segments = new PathSegmentCollection();
                        LineSegment l = new LineSegment();
                        l.Point = new Point(endx, y);
                        pf.Segments.Add(l);
                        pg.Figures.Add(pf);
                    }
                }
            } 
            #endregion

            // Here we create the columns which are vertical
            // and range over X which is dim1
            // from (x,dim2.start) -> (x,dim2.End)
            // where x ranges from dim1.start to dim1.end
            #region Create Columns
            if (_dim1.Start >= 0)
            {  // both are positive so start at 0

                for (double x = startx; x <= endx; x += _dim1.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(x, starty);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(x, endy);
                    pf.Segments.Add(l);
                    pg.Figures.Add(pf);
                }
            }
            else  // starting value is negative
            {   // here go from 0 down to negative starting value

                for (double x = 0; x >= startx; x -= _dim1.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(x, starty);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(x, endy);
                    pf.Segments.Add(l);
                    pg.Figures.Add(pf);
                }

                if (_dim1.End > 0)
                {
                    //go from 0 to End
                    for (double x = 0; x <= endx; x += _dim1.Inc)
                    {
                        PathFigure pf = new PathFigure();
                        pf.StartPoint = new Point(x, starty);
                        pf.Segments = new PathSegmentCollection();
                        LineSegment l = new LineSegment();
                        l.Point = new Point(x, endy);
                        pf.Segments.Add(l);
                        pg.Figures.Add(pf);
                    }
                }
            } 
            #endregion

            _outline.Data = pg;
            return _outline;
        }
    }
}
