using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Geometric_Chuck.Common;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI;

namespace Geometric_Chuck
{
   
    public class Grid : BindableBase
    {
        public enum GRIDTYPE {CARTESIAN,RADIAL};

        protected Range _dim1;
        public Range Dim1
        {
            get { return _dim1; }
            set { 
                    SetProperty(ref _dim1, value);
                    _dim1.PropertyChanged += _dim_PropertyChanged;
                    CreateGrid();
            }
        }

        protected Range _dim2;
        public Range Dim2
        {
            get { return _dim2; }
            set { 
                    SetProperty(ref _dim2, value);
                    _dim2.PropertyChanged += _dim_PropertyChanged;
                    CreateGrid();
            }
        }

        public virtual Path CreateGrid()
        {
            Path _outline = new Path();
            _outline.Name = "GRID";
            _outline.RenderTransform = new CompositeTransform();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(100, 0, 0, 255);
            _outline.Stroke = mySolidColorBrush;
            _outline.StrokeThickness = 0.5;
            return _outline;
        }

        protected Path _outline;
        GRIDTYPE _gtype;

        public Grid(GRIDTYPE _typ, Range dim1, Range dim2)
        {
            _gtype = _typ;
            Dim1 = dim1;
            Dim2 = dim2;
            CreateGrid();
        }

        void _dim_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Update();
        }

        public Grid() : this(GRIDTYPE.RADIAL,
                             new Range(0,0.1,1),
                             new Range(0,20,360)) { }

        public Path Outline
        {
            get { return _outline; }
            set { SetProperty(ref _outline, value, "Outline"); }
        }

        public void Update()
        {
            Outline = CreateGrid();
        }
    }

    public class RadialGrid : Grid
    {

        public RadialGrid(Range r)
            : base(GRIDTYPE.RADIAL,r,new Range(0,20,360)) 
        {
            Outline = CreateGrid();
        }

        public RadialGrid()
            : base(GRIDTYPE.RADIAL, new Range(0, 0.1, 1), new Range(0, 20, 360)) 
        { 
            Outline = CreateGrid();
        }

        public override Path CreateGrid()
        {
            Path _outline = base.CreateGrid();
            if (Dim1 == null || Dim2 == null) return _outline;
            GeometryGroup gg = new GeometryGroup();
            Point origin = new Point(0, 0);
            for (double r = _dim1.Start + _dim1.Inc; r <= _dim1.End; r += _dim1.Inc)
            {
                EllipseGeometry eg = new EllipseGeometry();
                eg.Center = origin;
                eg.RadiusX = eg.RadiusY = r;
                gg.Children.Add(eg);
            }

            for (double a = _dim2.Start + _dim2.Inc; a <= _dim2.End; a += _dim2.Inc)
            {
                double aa = a * Math.PI / 180;
                double xs = (_dim1.Start + _dim1.Inc) * Math.Cos(aa);
                double ys = (_dim1.Start + _dim1.Inc) * Math.Sin(aa);
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
            Outline = CreateGrid();
            Outline.Name = "GRID";
        }

        public override Path CreateGrid()
        {
            Path _outline = base.CreateGrid();
            PathGeometry pg = new PathGeometry();
            double start1 = Math.Floor(_dim1.Start / _dim1.Inc) * _dim1.Inc;
            double start2 = Math.Floor(_dim2.Start / _dim2.Inc) * _dim2.Inc;
            double end1 = Math.Round(_dim1.End / _dim1.Inc, 0) * _dim1.Inc;
            double end2 = Math.Ceiling(_dim2.End / _dim2.Inc) * _dim2.Inc;

            pg.Figures = new PathFigureCollection();
            if (_dim1.Start >= 0)
            {  // both are positive so start at 0

                for (double x = 0; x <= end1; x += _dim1.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(x, start2);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(x, end2);
                    pg.Figures.Add(pf);
                }
            }
            else  // starting value is negative
            {   // here go from 0 down to negative starting value

                for (double x = 0; x >= start1; x -= _dim1.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(x, start2);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(x, end2);
                    pf.Segments.Add(l);
                    pg.Figures.Add(pf);
                }

                if (_dim1.End > 0)
                {
                    // go from 0 to End
                    for (double x = 0; x <= end1; x += _dim1.Inc)
                    {
                        PathFigure pf = new PathFigure();
                        pf.StartPoint = new Point(x, start2);
                        pf.Segments = new PathSegmentCollection();
                        LineSegment l = new LineSegment();
                        l.Point = new Point(x, end2);
                        pf.Segments.Add(l);
                        pg.Figures.Add(pf);
                    }
                }
            }

            if (_dim2.Start >= 0)
            {
                for (double y = 0; y <= end2; y += _dim2.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(start1, y);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(end1, y);
                    pf.Segments.Add(l);
                    pg.Figures.Add(pf);
                }
            }
            else
            {
                for (double y = 0; y >= start2; y -= _dim2.Inc)
                {
                    PathFigure pf = new PathFigure();
                    pf.StartPoint = new Point(start1, y);
                    pf.Segments = new PathSegmentCollection();
                    LineSegment l = new LineSegment();
                    l.Point = new Point(end1, y);
                    pf.Segments.Add(l);
                    pg.Figures.Add(pf);
                }

                if (_dim2.End > 0)
                {
                    for (double y = 0; y <= end2; y += _dim2.Inc)
                    {
                        PathFigure pf = new PathFigure();
                        pf.StartPoint = new Point(start1, y);
                        pf.Segments = new PathSegmentCollection();
                        LineSegment l = new LineSegment();
                        l.Point = new Point(end1, y);
                        pf.Segments.Add(l);
                        pg.Figures.Add(pf);
                    }
                }
            }
            _outline.Data = pg;
            return _outline;
        }
    }
}
