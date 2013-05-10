using OTWB.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Xml.Serialization;
using OTWB.PathGenerators;
using OTWB.Coordinates;

namespace OTWB.Collections
{
    
    public class ShapeCollection : BindableBase
    {
        string _patternName;            // Name of the Pattern
        public string PatternName
        {
            get { return _patternName; }
            set { SetProperty(ref _patternName, value, "PatternName"); }
        }

        PatternType _patternType;       // Type of the pattern
        public OTWB.PatternType PatternType
        {
            get { return _patternType; }
            set { SetProperty(ref _patternType, value, "PatternType"); }
        }

        ObservableCollection<Shape> _items;   // Items in Collection
        public ObservableCollection<Shape> Shapes
        {
            get { return _items; }
            set { SetProperty(ref _items, value, "Items"); }
        }

        Extent2D _extent;         // Extent of the collection
        public Extent2D PathExtent
        {
            get { return _extent; }
            set { SetProperty(ref _extent, value, "PathExtent"); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

          public ShapeCollection()
            : base()
        {
            _patternName = string.Empty;
            _patternType = PatternType.NONE;
            _items = new ObservableCollection<Shape>();
            _items.CollectionChanged += _items_CollectionChanged;
            _extent = new Extent2D();
        }

        void _items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Count = _items.Count;
            if ((CollectionChanged != null) && _items.Count > 0)
                CollectionChanged(this, e);
        }

        public Shape this[int index]
        {
            get { return Shapes[index]; }
            set { Shapes[index] = (Polygon)value; }
        }

        int _count;
        public int Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value, "Count"); }
        }

        public List<string> PathNames
        {
            get
            {
                List<string> names = new List<string>();
                if (this.Shapes.Count == 0)
                    names.Add("Empty");
                else
                {

                    foreach (Polygon p in this.Shapes)
                        names.Add(p.Name);
                }
                return names;
            }
        }

        public void AddShape(Shape p)
        {
            if (p.Name == string.Empty)
                p.Name = nextPathName;
            Shapes.Add(p);
            p.Tag = IndexOf(p); // tag is position in collection
            UpdateExtent(p);
        }

        public int IndexOf(Shape p)
        {
            return Shapes.IndexOf(p);
        }

        public string nextPathName
        {
            get
            {
                var resourceLoader = new ResourceLoader();
                string fmt = resourceLoader.GetString("PATH_FMT");

                return string.Format(fmt, PatternName, this.Shapes.Count);
            }
        }

        public bool Exists(string pathname)
        {
            return !(PathOf(pathname) == null);
        }

        public Shape PathOf(string pathname)
        {
            return this.Shapes.First(p => p.Name == pathname);
        }
        public Shape PathAt(int indx)
        {
            return this.Shapes[indx];
        }

        public int NumPoints
        {
            get
            {
                int tot = 0;
                foreach (Shape s in this.Shapes)
                    if (s is Polygon)
                        tot += (s as Polygon).Points.Count;
                return tot;
            }
        }

        public void UpdateExtent(Shape s)
        {
            if (s is Polygon)
            {
                Polygon poly = s as Polygon;
                foreach (Point pt in poly.Points)
                    PathExtent.Update(pt);
            }
        }

        public void CalculateExtent()
        {
            PathExtent.MakeEmptyExtent();
            if (this.Shapes.Count > 0)
                foreach (Shape s in this.Shapes)
                {
                    if (s is Polygon)
                        UpdateExtent((s as Polygon));
                }
        }

        public void Clear()
        {
            this.Shapes.Clear();
            PathExtent.MakeEmptyExtent();
        }

        public List<List<Point>> AllPoints
        {
            get
            {
                List<List<Point>> all = new List<List<Point>>();
                foreach (Shape s in this.Shapes)
                {
                    if (s is Polygon)
                    {
                        all.Add((s as Polygon).Points.ToList());
                    }
                }
                return all;
            }
        }

        public List<PathFragment> AllPaths
        {
            get
            {
                List<PathFragment> p = new List<PathFragment>();
                foreach (Shape s in this.Shapes)
                {
                    if (s is Polygon)
                    {
                        p.Add(new PathFragment((s as Polygon).Points));
                    }
                    else if (s is Path)
                    {

                    }
                }
                return p;
            }
        }
    }

}
