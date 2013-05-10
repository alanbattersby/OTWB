using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows;
using Windows.Foundation;
using OTWB.Common;
using Windows.UI.Xaml.Shapes;
using OTWB.Collections;
using Windows.UI.Xaml.Media;
using OTWB.Coordinates;


namespace OTWB.Profiles
{
    public class PointXAscending : IComparer<Point>
    {
        public int Compare(Point p1, Point p2)
        {
            return p1.X.CompareTo(p2.X);
        }

    }

    public class PointXDescending : IComparer<Point>
    {
        public int Compare(Point p1, Point p2)
        {
            return -p1.X.CompareTo(p2.X);
        }

    }

    public class PointYComparer : IComparer<Point>
    {
        public int Compare(Point p1, Point p2)
        {
            return p1.Y.CompareTo(p2.Y);
        }

    }
    /// <summary>
    /// A profile has a descriptive key value
    /// plus a list of pairs of values
    /// </summary>
    public class PointProfile : Profile
    {
        SortableObservableCollection<Point> _profile;		// list of x / r , height values
        double _key;
        public double Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }
        Point _origin;

        Nullable<Point> _currentItem;
        Nullable<Point> _newItem;

        protected Path _outline;     // outline of this rosette
        PointXDescending _xdescending;
        PointXAscending _xascending;

        [XmlIgnore]
        public override Shape Visual
        {
            get
            {
                if (_outline.Data == null)
                    CreateOutline();
                return _outline;
            }
        }

        public PointProfile()
        {

            _profile = new SortableObservableCollection<Point>();
            Key = -1;
            _origin = new Point(0, 0);
            _outline = new Path();
            _outline.StrokeThickness = 1;

            _currentItem = new Nullable<Point>();
            _newItem = new Nullable<Point>();
            _xdescending = new PointXDescending();
            _xascending = new PointXAscending();
        }

        public SortableObservableCollection<Point> Data
        {
            get { return _profile; }
            set
            {
                _profile = value;
                _profile.Sort<Point>((Point pt) => pt, _xascending);
            }
        }



        public void Add(double r, double h)
        {
            Point p = new Point(r, h);
            if (_profile.Contains(p))
            {
                return;
            }
            else
                _profile.Add(p);

            if (_profile.Count > 1)
                _profile.Sort<Point>((Point pt) => pt, _xascending);
        }

        public void RemoveR(double r)
        {
            int i = 0;
            bool valid = false;
            for (i = 0; i < _profile.Count; i++)
            {
                if (r == _profile[i].X)
                {
                    valid = true;
                    break;
                }
            }
            if (valid)
                _profile.RemoveAt(i);
        }

        // change the current r value
        public void UpdateR(double oldr, double newr, double h)
        {
            Point v = _profile.First((Point v2) => (v2.X == oldr && v2.Y == h));
            if (v.X == oldr)
            {
                _profile.Remove(v);
                _profile.Add(new Point(newr, h));
            }
            if (_profile.Count > 1)
                _profile.Sort<Point>((Point pt) => pt, _xascending);
        }

        // change the height of the current r value
        public void UpdateH(double r, double oldh, double newh)
        {
            Point v = _profile.First((Point v2) => (v2.X == r && v2.Y == oldh));

            if (v.X == r)
            {
                _profile.Remove(v);
                //				RemoveR(r);
                _profile.Add(new Point(r, newh));
            }
            else
            {
                _profile.Add(new Point(r, newh));
            }
            _profile.Sort<Point>((Point pt) => pt, _xascending);
        }

        public override double Height(double r)
        {
            if (_profile.Count == 0)
                return 0;
            Nullable<Point> glb = null;
            Nullable<Point> lub = null;
            try
            {
                glb = _profile.Last(v2 => v2.X <= r);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                if (!glb.HasValue)
                    glb = _profile[0];
            }
            if (glb.HasValue && (glb.Value.X == r)) return glb.Value.Y;
            try
            {
                lub = _profile.First((Point v2) => (v2.X > r));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                if (!lub.HasValue)
                    lub = _profile[_profile.Count - 1];
            }

            return interp(lub.Value, r, glb.Value);
        }

        public override double Height(Cartesian c)
        {
            if (_profile == null || _profile.Count == 0) return 0;
            int indx = findLargerIndex(c.Length);
            if (indx == 0)
            {
                return interp(_origin, c.Length, _profile[0]);
            }
            else
                return interp(_profile[indx - 1], c.Length, _profile[indx]);
        }

        // assume that v1.r < v2.r
        // x is radius r , y is height h
        // so  h = (h2 - h1)(r - r1) / (r2 - r1) + h1
        double interp(Point v1, double r, Point v2)
        {
            return (v2.Y - v1.Y) * (r - v1.X) / (v2.X - v1.X) + v1.Y;
        }

        int findLargerIndex(double r)
        {
            for (int i = 0; i < _profile.Count; i++)
            {
                if (r <= _profile[i].X)
                    return i;
            }
            // here r is larger than largest value
            return _profile.Count - 1;
        }

        public void CreateOutline()
        {
            GeometryGroup gg = new GeometryGroup();
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            PolyLineSegment myPolyLineSegment = new PolyLineSegment();
            PointCollection pnts = new PointCollection();
            foreach (Point p in _profile)
                pnts.Add(p);
            myPolyLineSegment.Points = pnts;
            
            pathFigure.Segments.Add(myPolyLineSegment);
            pathFigure.StartPoint = _profile[0];
            pathGeometry.Figures.Add(pathFigure);
            gg.Children.Add(pathGeometry);

            for (int indx = 0; indx < _profile.Count; indx++)
            {
                EllipseGeometry eg = new EllipseGeometry();
                eg.Center = _profile[indx];
                eg.RadiusX = eg.RadiusY =  1;
                gg.Children.Add(eg);
            }
            _outline.Data = gg;
        }


        public void MaxValues(out double r, out double d)
        {
            if (_profile.Count == 0)
            {
                r = 0;
                d = 0;
                return;
            }

            // search for largest values in the profile
            r = d = double.MinValue;
            foreach (Point v in _profile)
            {
                if (v.X > r) r = v.X;
                if (v.Y > d) d = v.Y;
            }
            return;
        }

        public double MaxAbsoluteHeight()
        {
            if (_profile.Count == 0)
                return 0;
            double m = 0;
            foreach (Point v in _profile)
            {
                double a = (double)Math.Abs(v.Y);
                if (a > m)
                    m = a;
            }
            return m;
        }

        public double MaxAbsoluteKey()
        {
            if (_profile.Count == 0)
                return 0;
            double m = 0;
            foreach (Point v in _profile)
            {
                double a = (double)Math.Abs(v.X);
                if (a > m)
                    m = a;
            }
            return m;
        }

        public string KeyString
        {
            get { return string.Format("Key {0}", Key); }
        }

        #region IEditableCollection

        public bool CanAddNew { get { return true; } }
        public bool CanRemove { get { return _profile.Count > 0; } }
        public bool CanCancelEdit { get { return true; } }
        public bool IsAddingNew { get { return _newItem.HasValue; } }
        public bool IsEditingItem { get { return _currentItem.HasValue; } }

        public Object CurrentEditItem { get { return _currentItem.Value; } }
        public Object CurrentAddItem { get { return _newItem.Value; } }

        public Object AddNew()
        {
            _currentItem = new Point();
            if (_currentItem.HasValue)
                _profile.Add(_currentItem.Value);
            _currentItem = null;
            return _profile[_profile.Count - 1];
        }

        public void Remove(Object item)
        {
            if (item is Point)
                _profile.Remove((Point)item);
        }

        public void RemoveAt(int indx)
        {
            _profile.RemoveAt(indx);
        }

        public void EditItem(Object item)
        {
            if (item is Point)
            {
                _currentItem = (Point)item;
            }
        }

        public void CancelEdit()
        {
            _currentItem = null;
        }

        public void CancelNew()
        {
            _currentItem = null;
        }

        public void CommitEdit()
        {

        }

        public void CommitNew()
        {
            _profile.Add(_currentItem.Value);
            _currentItem = null;

        }
        #endregion


        public void Update(int row, int col, double newval)
        {
            //System.Diagnostics.Debug.Print("Update {0}, {1}, {2}", row, col, newval);
            if (_profile.Count > row)
            {
                Point p = _profile.ElementAt(row);
                if (col == 0)
                    p.X = newval;
                else if (col == 1)
                    p.Y = newval;
                _profile.RemoveAt(row);
                _profile.Insert(row, p);
                CreateOutline();
            }
        }

        public static PointProfile CreateConvex(int numpoints, double radius, double height, bool isneg)
        {
            PointProfile pf = new PointProfile();
            pf.fillProfile(numpoints, radius, height, false, isneg);
            return pf;
        }

        public static PointProfile CreateConcave(int numpoints, double radius, double height, bool isneg)
        {
            PointProfile pf = new PointProfile();
            pf.fillProfile(numpoints, radius, height, true, isneg);
            return pf;
        }

        void fillProfile(int n, double r, double h, bool concave, bool isneg)
        {
            int dp = (int)OTWB.BasicLib.GetSetting("DP");
            double R = (r * r + h * h) / (2 * h);
            double d = R - h;
            double theta = Math.Atan(d / r);
            double dtheta = (Math.PI / 2 - theta) / n;
            double rr = 0, rh = 0;
            _profile.Clear();
            for (int i = 0; i <= n; i++)
            {
                double a = theta + i * dtheta;
                rr = Math.Round(R * Math.Cos(a), dp);
                if (concave)
                    rh = (float)Math.Round(R * Math.Sin(a) - d, dp);
                else
                    rh = (float)Math.Round(R * (1 - Math.Sin(a)), dp);
                Point np = new Point(rr, (isneg) ? -rh : rh);
                _profile.Add(np);
            }

            int maxindx = _profile.Count - 1;
            for (int indx = maxindx; indx >= 0; indx--)
            {
                Point p = _profile[indx];
                if (Math.Abs(p.X) > 0)
                    _profile.Add(new Point(-p.X, p.Y));
            }

        }

        public Range Range1
        {
            get
            {
                Range range = new Range(_profile[0].X, 0, _profile[0].X);
                foreach (Point p in _profile)
                {
                    if (p.X < range.Start)
                        range.Start = p.X;
                    if (p.X > range.End)
                        range.End = p.X;
                }
                range.CalcInc(_profile.Count);
                return range;
            }
        }

        public Range Range2
        {
            get
            {
                Range range = new Range(_profile[0].Y, 0, _profile[0].Y);
                foreach (Point p in _profile)
                {
                    if (p.Y < range.Start)
                        range.Start = p.Y;
                    if (p.Y > range.End)
                        range.End = p.Y;
                }
                range.CalcInc(_profile.Count);
                return range;
            }
        }
    }
}