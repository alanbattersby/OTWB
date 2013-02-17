using Geometric_Chuck.Common;
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

namespace Geometric_Chuck
{
    
    public class PolygonCollection :  BindableBase
    {
        string _patternName;            // Name of the Pattern
        public string PatternName 
        {
            get { return _patternName; }
            set { SetProperty( ref _patternName, value,"PatternName"); }   
        }

        PatternType _patternType;       // Type of the pattern
        public Geometric_Chuck.PatternType PatternType 
        {
            get { return _patternType; }
            set { SetProperty(ref _patternType, value, "PatternType"); }
        }

        ObservableCollection<Polygon> _items;   // Items in Collection
        public ObservableCollection<Polygon> Polygons
        {
            get { return _items; }
            set { SetProperty(ref _items, value, "Items"); }
        }

        Extent _extent;         // Extent of the collection
        public Extent PathExtent 
        {
            get { return _extent; }
            set { SetProperty(ref _extent, value, "PathExtent"); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public PolygonCollection()
            : base()
        {
            _patternName = string.Empty;
            _patternType = PatternType.NONE;
            _items = new ObservableCollection<Polygon>();
            _items.CollectionChanged += _items_CollectionChanged;
            _extent = new Extent();
        }

        void _items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Count = _items.Count;
            if ((CollectionChanged != null) && _items.Count > 0)
                CollectionChanged(this, e);
        }

        public Polygon this[int index] 
        {
            get { return Polygons[index]; }
            set { Polygons[index] = (Polygon)value; }
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
                if (this.Polygons.Count == 0)
                    names.Add("Empty");
                else
                {
                   
                    foreach (Polygon p in this.Polygons)
                    names.Add(p.Name);
                }
                return names;
            }
        }

        public void AddPoly(Polygon p)
        {
            if (p.Name == string.Empty)
                p.Name = nextPathName;
            Polygons.Add(p);
            UpdateExtent(p);
        }
        public int IndexOf(Polygon p)
        {
            return Polygons.IndexOf(p); 
        }

        public string nextPathName
        {
            get
            {
                var resourceLoader = new ResourceLoader();
                string fmt = resourceLoader.GetString("PATH_FMT");
                
                return string.Format(fmt,PatternName,this.Polygons.Count);
            }
        }

        public bool Exists(string pathname)
        {
            return !(PathOf(pathname) == null);
        }

        public Polygon PathOf(string pathname)
        {
            return this.Polygons.First(p => p.Name == pathname);
        }
        public Polygon PathAt(int indx)
        {
            return this.Polygons[indx];
        }

        public int NumPoints {
            get
            {
                int tot = 0;
                foreach (Polygon p in this.Polygons)
                    tot += p.Points.Count;
                return tot;
            }
        }

        public void UpdateExtent(Polygon poly)
        {
            foreach (Point pt in poly.Points)
                PathExtent.Update(pt);
        }

        public void CalculateExtent()
        {
            PathExtent.MakeEmptyExtent();
            if (this.Polygons.Count > 0)
                foreach (Polygon p in this.Polygons)
                    UpdateExtent(p);
        }

        public void Clear()
        {
            this.Polygons.Clear();
            PathExtent.MakeEmptyExtent();
        }

        public List<List<Point>> AllPoints
        {
            get
            {
                List<List<Point>> all = new List<List<Point>>();
                foreach (Polygon poly in this.Polygons)
                {
                    all.Add(poly.Points.ToList());
                }
                return all;
            }
        }
    }

}
