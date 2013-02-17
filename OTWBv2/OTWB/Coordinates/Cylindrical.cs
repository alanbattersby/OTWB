using Geometric_Chuck;
// Cylindrical.cs
using Geometric_Chuck.Interfaces;
using System;
using System.Xml.Serialization;
using TCD.Mathematics;
using Windows.Foundation;

namespace  OTWB.Coordinates
{
	
	/// <summary>
	/// The Cylindrical coordinate represents points in the
	/// direction plane XZ by a radius R and an angle A. The Y coordinate
	/// is represented by the Depth Property
	/// </summary>
	[XmlRoot("CYLINDRICAL")]
	public class Cylindrical : ICoordinate
	{
		double _r;			// radius
		double _a;			// angle in degrees
		double _d;         	// depth

        static int DP 
        {
            get
            {
               var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
               return (localSettings.Values["DP"] != null) ? (int)(localSettings.Values["DP"]) : 3;
            }
        }             // local dp settings

		[XmlIgnore]
		public double AngleInRadians
		{
            get { return Math.Round(Geometric_Chuck.BasicLib.ToRadians * _a, DP); }
			set { _a = Math.Round(Geometric_Chuck.BasicLib.ToDegrees * value,DP); }
		}
			
		public Cylindrical(double r, double a, double d)
		{
           
			Depth = Math.Round(d,DP); 
			Radius = Math.Round(r,DP);
			Angle = Math.Round(a,DP);
		}

        public Cylindrical(Point p)
        {
            _r = Math.Round(Math.Sqrt(p.X * p.X + p.Y * p.Y), DP);
            _a = Pol(p.X, p.Y);
            _d = 0;
        }

	    public Cylindrical() : this (0,0,0){}
						
		public Cylindrical(Cartesian v)
		{
            _r = Math.Round(Math.Sqrt(v.X * v.X + v.Y * v.Y), DP);
            _a = Pol(v.X, v.Y);
            _d = Math.Round(v.Z, DP);
			Angle += v.WindingNumber * 360.0f;
		}
		
		public Cylindrical(Cylindrical cc)
			: this (cc.Radius,cc.Angle,cc.Depth){}
				
		
		public Cylindrical toCylindrical
		{
			get { return this; }
		}
		
		public ICoordinate Copy
		{
			get {return new Cylindrical(this); }
		}
		
		public bool PointAtInfinity
		{
			get {
				return  double.IsPositiveInfinity(_r);
			}
		}

	
		public Cartesian toCartesian3
		{
			get 
            {
                double X = _r * Math.Cos(BasicLib.ToRadians * _a);
                double Y = _r * Math.Sin(BasicLib.ToRadians * _a);
                return new Cartesian(X,Y,Depth); 
            }
		}
		
		public Spherical toSpherical
		{
			get { return new Spherical(this.toCartesian3); }	
		}
	
		[XmlIgnore]
		public Point asPoint
		{
			
			get 
			{
				Point p = new Point();
				p.X = _r * Math.Cos(BasicLib.ToRadians * _a);
				p.Y = _r * Math.Sin(BasicLib.ToRadians *_a );
				return p;
			}
			set
			{
				double x = value.X;
				double y = value.Y;
				_r = Math.Sqrt(x * x + y * y);
				_a = BasicLib.ToRadians * Pol(x,y);
				_d = 0;
			}
		}

		public double Depth {
			get {
				return _d;
			}
			set {
				_d = Math.Round(value,DP);
			}
		}
		
		[XmlElement]
		public double Radius {
			get {
				return _r;
			}
			set {
				_r = Math.Round(value,DP);
			}
		}
		
		[XmlElement]
		public double Angle {
			get {
				return _a;
			}
			set {
				_a = Math.Round(value,DP);
			}
		}
		
		[XmlIgnore]
		public int WindingNumber
		{
			get { return (int)Math.Floor(Angle / 360.0); }	
			set { 
					double a = Angle % 360.0;
					Angle = a + value * 360.0;
			}
		}
		
			
		public bool Equal(Cylindrical p)
		{
			return ((p.Radius == Radius) && (p.Angle  == Angle) && (p.Depth == Depth));
		}
		
		public override string ToString()
		{
			return string.Format("r={0:f} a={1:f} d={2:f}",Radius,Angle,Depth);
		}

		
	
		public bool Equals (ICoordinate c)
		{
			Cylindrical cc = c.toCylindrical;
			return (cc.Radius == Radius && cc.Angle == Angle && cc.Depth == Depth);
		}
		
		public void Add(ICoordinate c1)
		{
			Cartesian v = this.toCartesian3 + c1.toCartesian3;
			_r = Math.Round(Math.Sqrt(v.X * v.X + v.Y * v.Y),DP);
			_a = Pol(v.X,v.Y);  
			_d = Math.Round(v.Z,DP);
		}
		
		public static double Pol(double x, double y)
		{
			if (y >= 0)
			{
				double r = Math.Sqrt(x*x + y*y);
				if (Math.Abs(r) < BasicLib.EPS)
					return 0;
				else
					return Math.Round(BasicLib.ToDegrees * Math.Acos(x/r),DP);
			}
			else 
				return (double)(180 + Pol(-x, -y));
		}
		
        //public void Transform (Transformation t)
        //{
        //    Cylindrical c = t.Transform(this.toCartesian3).toCylindrical;
        //    Radius = c.Radius;
        //    Angle = c.Angle;
        //    Depth = c.Depth;
        //}

	}
}
