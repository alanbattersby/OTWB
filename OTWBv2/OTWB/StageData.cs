using OTWB.Common;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace OTWB
{
    public enum PatternType
    {
        bazley, ross, wheels, barrel, braid, latticeRim, latticeFace, NONE
    }

    [KnownType(typeof(OTWB.BazelyStageData))]
    public class BazelyStageData : BindableBase
    {
        double _vnum;
        public double Vnum 
        {
            get { return _vnum; }
            set { SetProperty(ref _vnum, value ,"Vnum"); }  
        }

        double _vden;
        public double Vden
        {
            get { return _vden; }
            set { SetProperty(ref _vden, value, "Vden"); }
        }

        bool _samedirn;
        public bool   SameDirection
        {
            get { return _samedirn; }
            set { SetProperty(ref _samedirn, value, "SameDirection"); }
        }

        double _ex;
        public double Ex
        {
            get { return _ex; }
            set { SetProperty(ref _ex, value, "Ex"); }
        }

        double _phi;
        public double PHI
        {
            get { return _phi; }
            set { SetProperty(ref _phi, value, "PHI"); }
        }


        public BazelyStageData (ArraySegment<string> data)
        {
            Vnum = double.Parse(data.ElementAt(0));
            Vden = double.Parse(data.ElementAt(1));
            SameDirection = (double.Parse(data.ElementAt(2)) == 1);
            Ex = double.Parse(data.ElementAt(3));
            PHI = double.Parse(data.ElementAt(4));
        }
        public BazelyStageData()
        {
            Vnum = 1;
            Vden = 1;
            SameDirection = true;
            Ex = 0;
            PHI = 0;
        }
    }

    // A chuck has a pattern Number which is also its name
    // It comprises of several stages
    [KnownType(typeof(OTWB.BazelyChuck))]
    [KnownType(typeof(OTWB.BazelyStageData))]
    public class BazelyChuck : PathGenData
    {

        [XmlArray ("Pattern")]
        public ObservableCollection<BazelyStageData> Stages { get; set; }

        double _sr;
        public double SR 
        { 
            get { return _sr; }
            set { SetProperty(ref _sr,value,"SR"); }    
        }

        string _script;
        public string Script 
        {
            get { return _script; }
            set { SetProperty(ref _script, value, "Script"); } 
        }


        public BazelyChuck() : base(PatternType.bazley,0,1)
        {
            Stages = new ObservableCollection<BazelyStageData>();
            SR = 0;
            var resourceLoader = new ResourceLoader();
            Script = resourceLoader.GetString("RUN");
        }

        public void Add(BazelyStageData sdata)
        {
            sdata.PropertyChanged += sdata_PropertyChanged;
            Stages.Add(sdata);
        }

        public void FixHandler()
        {
            foreach (BazelyStageData sdata in Stages)
            {
                sdata.PropertyChanged += sdata_PropertyChanged;
            }
        }

        public BazelyChuck(int pnum, String[] data)
            : base(PatternType.bazley,0,1)
        {
            PatternIndex = pnum;
            try
            {
                SR = double.Parse(data[5]);
            }
            catch
            {
                SR = 0;
            }
            Stages = new ObservableCollection<BazelyStageData>();
            // now one or more stages 
            ArraySegment<string> stage1 = new ArraySegment<string>(data,0,5);
            ArraySegment<string> stage2 = new ArraySegment<string>(data, 6, 5);
            BazelyStageData sdata = new BazelyStageData(stage1);
            this.Add(sdata);
            string s2 = stage2.ElementAt(0);
            if (double.Parse(s2) != 0)
                this.Add(new BazelyStageData(stage2));
            var resourceLoader = new ResourceLoader();
            Script = resourceLoader.GetString("RUN");
        }

        void sdata_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }
    }

    public class RossStageData : BindableBase
    {
        double _radius;
        public double  Radius 
        {
            get { return _radius; }
            set { SetProperty(ref _radius, value, "Radius"); }
        }        // radius of circle
        
        public Rational N {get; set; }               // this is always 1:1
        public double Phi   { get; set; }         // phase angle

        public RossStageData(Rational n, double r, double phi )
        {
            N = n;
            Radius = r;
            Phi = phi;
        }

        public RossStageData()
        {
            Radius = 0;
            N = new Rational();
            Phi = 0;
        }

        public override string ToString() 
        {
            return String.Format("[R={0}, Phi = {1}, Ratio = {2}]", Radius, Phi, N);
        }

    }

    public class RossPatternData : ObservableCollection<RossStageData>
    {
        public RossPatternData() : base() { PatternNumber = 0; }
        public RossPatternData(int i) : base() { PatternNumber = i; }

        public void Add(Rational n, double r, double phi)
        {
            base.Add(new RossStageData(n, r, phi));
        }

        public int PatternNumber { get; set; }
        [XmlIgnore]
        public string Name { get { return string.Format("#{0}", PatternNumber); } }

    }

   


}
