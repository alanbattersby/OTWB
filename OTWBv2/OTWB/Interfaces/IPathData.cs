using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Geometric_Chuck.Interfaces
{
    public interface IPathData : INotifyPropertyChanged
    {
        string Name { get; }
        PatternType PathType { get; }
        int PatternIndex { get; set; }
        double SuggestedMaxTurns { get; }
    }
}
