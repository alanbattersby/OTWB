using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTWB
{
    public class Ratio
    {
        int _n = 1;         // Numerator
        int _m = 1;         // Denominator
        bool _neg = false;  // sign

        public bool Negative
        {
            get { return _neg; }
            set { _neg = value; }
        }
      
        public int First
        {
            get { return _n; }
            set { _n = (int)Math.Floor(1.0 * value); }
        }
        
        public int Last
        {
            get { return (_neg ) ? - _m : _m ; }
            set { _neg = value < 0;
                 _m = (int)Math.Floor (Math.Abs(1.0 * value)); 
            }
        }

        public Ratio()
        {
            _n = 1;
            _m = 1;
            _neg = false;
        }

        public Ratio (int n, int m)
        {
            First = n;
            Last = m;
        }
   
  
        int  GreatestDivisor(int numberA , int numberB)
        {
            if (numberB == 0) 
                return numberA;
            else
                return GreatestDivisor(numberB, numberA % numberB);
        }

        public void Simplify()
        {
            var gcd = GreatestDivisor(_n, _m);
            if (gcd > 1)
            {
                _n = _n / gcd;
                _m = _m / gcd;
            }
        }

        public Ratio SimplestTerms()
        {
            Ratio  _copy = new Ratio(this.First, this.Last);
            _copy.Simplify();
            return _copy;
        }

        public override string ToString()
        {
            if (_neg)
                return String.Format("{0}:-{1}", _n, _m);
            else
                return String.Format("{0}:{1}", _n, _m);
        }

        public Ratio (double v, int dp)
        {
            // Set value from double to correct number of decimal places
            //  given by dp
            _neg = v < 0;
            _m = (int)Math.Pow(10.0, (double)dp);
            _n = (int)Math.Floor(Math.Abs(v) * _m);
            Simplify();
        }

        public double Value
        {
            get
            {
                if (_neg)
                    return -1.0 * _n / _m;
                else
                    return (1.0 * _n) / _m;
            }
        }

        public double NtoOne
        {
            get {
                return Value;
            }

        }
       
        public double OnetoN
        {
            get {
                return 1 / Value;
            }
        }
    }
}
