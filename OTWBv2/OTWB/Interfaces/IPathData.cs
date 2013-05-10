using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OTWB.Interfaces
{
    public interface PathData : INotifyPropertyChanged
    {
        string Name { get; }
        PatternType PathType { get; set; }
        int PatternIndex { get; set; }
        double SuggestedMaxTurns { get; set; }
    }
}
