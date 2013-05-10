using OTWB;
using OTWB.Common;
using OTWB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OTWB.PathGenerators
{
    [XmlInclude(typeof(OTWB.BazelyChuck))]
    [XmlInclude(typeof(OTWB.RossData))]
    [XmlInclude(typeof(OTWB.WheelsData))]
    [XmlInclude(typeof(OTWB.Spindle.Barrel))]
    [XmlInclude(typeof(OTWB.Lattice.LatticeData ))]
    [XmlInclude(typeof(OTWB.Braid.BraidData))]
    public class PathGenData : BindableBase, PathData
    {
      
        [XmlIgnore]
        public string Name
        {
            get { return string.Format("{0}#{1}", PathType, PatternIndex); }
        }

        PatternType _ptype;
        public PatternType PathType
        {
            get { return _ptype; }
            set { SetProperty(ref _ptype, value); }
        }

        int _patternindx;
        public int PatternIndex
        {
            get { return _patternindx; }
            set { SetProperty(ref _patternindx, value); }
        }

        double _maxTurns;
        public double SuggestedMaxTurns
        {
            get { return _maxTurns; }
            set { SetProperty(ref _maxTurns, value); }
        }

        public PathGenData(PatternType typ, int indx, int turns)
        {
            PathType = typ;
            PatternIndex = indx;
            SuggestedMaxTurns = turns;
        }

        public PathGenData() : this(PatternType.NONE, -1,1) { }
    }
}
