using OTWB.Common;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.Foundation;
using System.Xml.Serialization;
using OTWB.Coordinates;

namespace OTWB.Lattice
{
     [XmlRoot("LatticeData")]
    public class LatticeData : PathGenData
    {
        public event EventHandler<Line2D> LineChanged;
        public event EventHandler<Line2D> LineRemoved;

        Random namegen;
        public static string LineNameFormat = "LINE{0}";

        LayoutData _layout;
        public LayoutData Layout
        {
            get { return _layout; }
            set 
            { 
                SetProperty(ref _layout, value);
                _layout.PropertyChanged += _layout_PropertyChanged;
                _layout.ClipRange.PropertyChanged += _layout_PropertyChanged;
            }
        }

        void _layout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        int _dim1;
        public int Rows
        {
            get { return _dim1; }
            set { SetProperty(ref _dim1, value); }
        }

        int _dim2;
        public int Columns
        {
            get { return _dim2; }
            set { SetProperty(ref _dim2, value); }
        }

        ObservableCollection<Line2D> _items;
        public ObservableCollection<Line2D> Lines
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

       

        public LatticeData(int nxtfree) :
             base(PatternType.latticeRim,nxtfree,1)
        {
            Rows = 6;
            Columns = 6;
            Layout = new LayoutData();
          
            Lines = new ObservableCollection<Line2D>();
            namegen = new Random(DateTime.UtcNow.Millisecond);
           
        }

        void ClipRange_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public LatticeData() : this(0) { }

        public void Add(Line2D l)
        {
            _items.Add(l);
            l.Name = string.Format(LineNameFormat, namegen.Next());
            if (LineChanged != null)
                LineChanged(this, l);
            l.PropertyChanged += l_PropertyChanged;
        }

        public void Remove(Line2D l)
        {
            if (l == null) return;
            l.PropertyChanged -= l_PropertyChanged;
            _items.Remove(l);
            if (LineRemoved != null)
                LineRemoved(this, l);
            
        }

        void l_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Line2D)
                if (LineChanged != null)
                    LineChanged(this, (sender as Line2D));
        }

        internal void Clear()
        {
            Lines.Clear();
        }
    }

   
}
