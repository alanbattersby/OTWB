//  
//  Copyright (C) 2009 alan
// 

using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;

namespace OTWB.Common
{	
	public class Range : BindableBase
	{
		[XmlIgnore]
		public double _start;
		[XmlIgnore]
		public double _end;
		[XmlIgnore]
		public double _inc;
		
		[XmlElement("Start")]
		public double Start
		{
			get { return _start; }
			set { SetProperty(ref _start,value,"Start"); }
		}
		
		[XmlElement("End")]
		public double End
		{
			get { return _end; }
			set { SetProperty(ref _end,value,"End"); }
		}
		
		[XmlElement("Inc")]
		public double Inc
		{
			get { return _inc; }
			set { SetProperty(ref _inc, value, "Inc"); }
		}
		
		public Range(double s, double i, double e)
		{
			Start = s;
			Inc = i;
			End = e;
		}
		
		public Range() : this(0,0,0){}
		
		public Range(double s, double e) : this(s,0,e){}
		
		public void CalcInc(int nsteps)
		{
			if (nsteps < 2)
				Inc = 0;
			else
				Inc = (End - Start) / (nsteps - 1);	
		}
		
		[XmlIgnore]
		public bool Increasing
		{
			get { return (Start < End); }
		}

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Start, Inc, End);
        }

        public override bool Equals(object obj)
        {
            if (obj is Range)
            {
                Range r = obj as Range;
                return (r.Start == Start) && (r.End == End);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + Math.Round(Start, 5).GetHashCode();
            hash = hash * 23 + Math.Round(Inc, 5).GetHashCode();
            hash = hash * 23 + Math.Round(End, 5).GetHashCode();
            return hash;
        }
        public double Length
        {
            get
            {
                return Math.Abs(End - Start);
            }
        }

        public bool Includes(double v)
        {
            return (v >= Start) && (v <= End);
        }

        public static bool operator< (double v, Range r)
        {
            return v < r.Start;
        }

        public static bool operator> (double v, Range r)
        {
            return v > r.End;
        }

     
	}

    
    
}