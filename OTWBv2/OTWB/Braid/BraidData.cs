using OTWB.Common;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace OTWB.Braid
{
    public class Map : BindableBase
    {
        int _src;
        public int Source
        {
            get { return _src; }
            set { SetProperty(ref _src, value); }
        }

        int _tgt;
        public int Target
        {
            get { return _tgt; }
            set { SetProperty(ref _tgt, value); }
        }

        public Map(int src, int tgt)
        {
            _src = src;
            _tgt = tgt;
        }

        public Map(int src) : this(src, src) { }
        public Map() : this(0, 0) { }

        public bool IsSame
        {
            get { return (_src == _tgt); }
        }
    }

    public class Permutation : BindableBase
    {
        int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        // index of braid
        List<Map> _perms;
        
        public List<Map> Mappings
        {
            get { return _perms; }
            set { SetProperty(ref _perms, value); }
        }

        public Permutation(List<Map> perms)
        {
            _perms = perms;
            _index = 0;
        }     
      
        public Permutation(int size)
        {
            _index = 0;
            _perms = new List<Map>(size);
        }

        public Permutation() : this(4) { }

        [XmlIgnore]
        public bool IsNoChange
        {
            get
            {
                foreach (Map t in _perms)
                    if (!t.IsSame)
                        return false;
                return true;
            }
        }

        public bool Exists(int from)
        {
            return _perms.Exists(t => t.Source == from);
        }

        public void SetPermOf(int i, int j)
        {
            if (!Exists(i))
                _perms.Add(new Map(i, j));
            else
            {
                Map t = _perms.Find(tt => tt.Source == i);
                t.Target = j;
               
            }
            OnPropertyChanged();
        }

        public int PermOf(int i)
        {
            if (!Exists(i))
                return i;
            else
            {
                Map t = _perms.Find(tt => tt.Source == i);
                return t.Target;
            }
        }

        [XmlIgnore]
        public List<int> Keys
        {
            get
            {
                return _perms.Select(t => t.Source).ToList();
            }
        }

        public static Permutation CreateID(int nstrands)
        {
            Permutation p = new Permutation(nstrands);
            for (int i = 0; i < nstrands; i++)
            {
                p.Mappings.Add(new Map(i));
            }
            return p;
        }
    }

    public class BraidData : PathGenData
    {
        int _nstrands;
        public int NumStrands
        {
            get { return _nstrands; }
            set { SetProperty(ref _nstrands, value); }
        }

        int _repeats;
        public int Repeats
        {
            get { return _repeats; }
            set { SetProperty(ref _repeats, value); }
        }

        double _toolposition;
        public double ToolPosition
        {
            get { return _toolposition; }
            set { SetProperty(ref _toolposition, value); }
        }

        double _length;
        public double Length
        {
            get { return _length; }
            set { SetProperty(ref _length, value); }
        }

        double _width;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        double _margin;
        public double Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        Random namegen;
        public static string StrandNameFormat = "STRAND{0}";

        ObservableCollection<Permutation> _perms;
        public ObservableCollection<Permutation> Perms
        {
            get { return _perms; }
            set { SetProperty(ref _perms, value); }
        }

            
        public BraidData(int pi) : base(PatternType.braid, pi, 1)
        {
            _nstrands = 4;
            _repeats = 1;
            _toolposition = 100;
            _length = 100;
            _width = 50;
            namegen = new Random(DateTime.UtcNow.Millisecond);
            _perms = new ObservableCollection<Permutation>();
        }

        public BraidData() : this(0) { }

        public void Add(Permutation p)
        {
            _perms.Add(p);
            p.Index = _perms.IndexOf(p);
            p.PropertyChanged += p_PropertyChanged;
        }

        public void RemovePerm(int indx)
        {
            _perms.RemoveAt(indx);
        }

        public void RemoveLastPerm()
        {
            Permutation p = _perms.Last();
            p.PropertyChanged -= p_PropertyChanged;
            _perms.Remove(p);
        }

        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           this.OnPropertyChanged(e.PropertyName);
        }
    
    }
}
