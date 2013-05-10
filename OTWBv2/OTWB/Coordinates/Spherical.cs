
using System;
using System.Xml.Serialization;

namespace  OTWB.Coordinates
{
	
	[XmlRoot("SPHERICAL")]
	public class Spherical : ICoordinate
	{
		double _radius;		// radius
		double _theta;		// longitude in degrees
		double _thi;			// co-lattitude in degrees

        static int DP
        {
            get
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                return (localSettings.Values["DP"] != null) ? (int)(localSettings.Values["DP"]) : 3;
            }
        }     

		[XmlElement]
		public double Radius {
			get {
				return _radius;
			}
			set {
				_radius = value;
			}
		}

		[XmlElement]
		public double Longitude {
			get {
				return _theta;
			}
			set {
				_theta = value;
			}
		}

		[XmlElement]
		public double CoLattitude {
			get {
				return _thi;
			}
			set {
				_thi = value;
			}
		}		
		
		[XmlIgnore]
		public double Lattitude {
			get {
				return 90.0f - _thi;
			}
			set {
				_thi = 90.0f - value;
			}
		}
		
		public Spherical(double r, double l, double cl)
		{
			_radius = Math.Round(r,DP);
			_theta = Math.Round(l,DP);
			_thi = Math.Round(cl,DP);
		}
		
		public Spherical (Cartesian c)
        {
            _radius = c.Length;
            _theta = OTWB.BasicLib.ToRadians * Math.Atan(c.Y / c.X);
            _thi = OTWB.BasicLib.ToRadians * Math.Acos(c.Z / _radius);
        }
				
		public Spherical(Cylindrical c)
			: this (c.toCartesian3){}
		
		public Spherical() : this(1.0f,0,0){}
		
		public Spherical(Spherical s)
			: this (s.Radius,s.CoLattitude,s.Longitude){}
		
		public ICoordinate Copy
		{
			get {return new Spherical(this); }
		}
		
		public bool PointAtInfinity
		{
			get {
				return  double.IsPositiveInfinity(_radius);
			}
		}
		
		public Cartesian toCartesian3 {
			get {
				double thetarad = OTWB.BasicLib.ToRadians * _theta;
				double thirad = OTWB.BasicLib.ToRadians * _thi;
				Cartesian c = new Cartesian();
				c.X = (double)(_radius * Math.Cos(thetarad) * Math.Sin(thirad));
				c.Y = (double)(_radius * Math.Sin(thetarad) * Math.Sin(thirad));
				c.Z = (double)(_radius * Math.Cos(thirad));
				return c;
			}
		}
		
		public Cylindrical toCylindrical {
			get {
				return toCartesian3.toCylindrical;
			}
		}
		
		public Spherical toSpherical
		{
			get { return this; }
		}
		
		public bool Equals (ICoordinate c)
		{
			Spherical cc = c.toSpherical;
			return (cc.Radius == Radius && 
			        cc.Longitude == Longitude && 
			        cc.CoLattitude == CoLattitude);
		}
		
		public void Add(ICoordinate c1)
		{
			Cartesian v = this.toCartesian3  + c1.toCartesian3;
            _radius = v.Length;
            _theta = OTWB.BasicLib.ToDegrees * Math.Atan(v.Y / v.X);
            _thi = OTWB.BasicLib.ToDegrees * Math.Acos(v.Z / _radius);
		}
		
		public double Depth
		{
			get { return this.toCylindrical.Depth; }	
			set {
				Cartesian c = this.toCartesian3;
				c.Depth = value;
				Spherical s = new Spherical(c);
				this.CoLattitude = s.CoLattitude;
				this.Radius = s.Radius;
				this.Longitude = s.Longitude;
			}
		}
		
        //public void Transform (Transformation t)
        //{
        //    Spherical s = t.Transform(this.toCartesian3).toSpherical;
        //    Radius = s.Radius;
        //    CoLattitude = s.CoLattitude;
        //    Longitude = s.Longitude;
        //}

	}
}
