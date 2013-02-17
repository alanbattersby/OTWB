using Geometric_Chuck.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometric_Chuck.Spindle
{
    class SingleOffset : IOffset
    {
        double _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value is double)
                    _value = (double)value;
            }
        }
    }
    class MultipleOffset : IOffset
    {
        Tuple _value;
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value is Tuple)
                    _value = (value as Tuple);
            }
        }
    }
}
