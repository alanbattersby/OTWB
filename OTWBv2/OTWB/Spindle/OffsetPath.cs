using Geometric_Chuck.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometric_Chuck.Spindle
{
    public class OffsetPath<T>
    {
        List<T> Offsets { get; set; }    // the offset objects
        public double Increment { get; set; }
        public int NumPoints { get; set; }
       
        public OffsetPath()
        {
            Increment = 0;
            NumPoints = 0;
            Offsets = new List<T>();
        }

        public T this[int i]
        {
            get
            {
                return Offsets[i];
            }
            set
            {
                Offsets[i] = value;
            }
        }

        public void Add(T item)
        {
            Offsets.Add(item);
        }
    }

}
