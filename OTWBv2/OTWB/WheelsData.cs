using Geometric_Chuck.Common;
using Geometric_Chuck.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Geometric_Chuck
{
    public class WheelStageData : BindableBase
    {
        double _freq;
        public double Frequency
        {
            get { return _freq; }
            set { SetProperty<double>(ref _freq, value, "Frequency"); }
        }

        double _amplitude;
        public double Amplitude
        {
            get { return _amplitude; }
            set { SetProperty<double>(ref _amplitude, value, "Radius"); }
        }

        public WheelStageData(double _f, double _r)
        {
            Frequency = _f;
            Amplitude = _r;
        }
        public WheelStageData() : this(1.0, 1.0) { }
    }

    [XmlRoot("WheelsData")]
    public class WheelsData : IPathData, INotifyPropertyChanged
    {
        public event EventHandler PathDataChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<WheelStageData> _stages;
        public ObservableCollection<WheelStageData> Stages
        {
            get { return _stages; }
            set { _stages = value; }
        }

        [XmlIgnore]
        public string Name { get { return string.Format("wheel#{0}", PatternIndex); } }

        public WheelsData(int indx)
        {
            Stages = new ObservableCollection<WheelStageData>();
            PatternIndex = indx; 
        }
       
        public WheelsData() : this(0) {}

        [XmlElement]
        public PatternType PathType
        {
            get { return PatternType.WHEELS; }
            set { }
        }

        int _index;
        [XmlElement]
        public int PatternIndex
        {
            get {  return _index; }
            set { _index = value; }
        }

        public void FixUp()
        {
            foreach (WheelStageData wd in Stages)
                wd.PropertyChanged += d_PropertyChanged;
        }

        public void Add(double r,double f)
        {
            WheelStageData d = new WheelStageData(f,r);
            d.PropertyChanged += d_PropertyChanged;
            this.Stages.Add(d);
            SuggestedMaxTurns = lcmperiods;
        }

        void d_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Frequency")
                SuggestedMaxTurns = lcmperiods;
            if (PathDataChanged != null)
                PathDataChanged(this, new EventArgs());
        }

        double _period;
        [XmlElement]
        public double Period
        {
            get { return _period; }
            set { _period = value; }
        }

        /// <summary>
        /// only called when more than one waveform
        /// </summary>
        double lcmperiods
        {
            get
            {
                Rational p1 = new Rational( this.Stages[0].Frequency,6);
                int d1 = p1.Denominator;
                for (int i = 1; i < this.Stages.Count; i++)
                {
                    p1 = new Rational(this.Stages[i].Frequency,6);
                    d1 = BasicLib.LCM(d1, p1.Denominator);
                }
                return Math.Abs(d1);
            }
        }

        double _suggestedMaxTurns;

        public double SuggestedMaxTurns
        {
            get { return _suggestedMaxTurns; }
            set { 
                    _suggestedMaxTurns = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("SuggestedMaxTurns"));
                }
        }
    }

   
}
