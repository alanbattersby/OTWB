using System;
using System.Xml.Serialization;

namespace OTWB.Coordinates
{
	/// <summary>
	/// The standard cartesian coordinate system X,Y,Z
	/// </summary>
	[XmlRoot("CARTESIAN")]
	public class Cartesian : ICoordinate
	{
		double _x,_y,_z;		// 	
		int _wn;			// winding number

        static int DP
        {
            get
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                return (localSettings.Values["DP"] != null) ? (int)(localSettings.Values["DP"]) : 3;
            }
        }     

		[XmlElement]
		public double X
		{
			get { return _x ; }
			set { _x = Math.Round(value,DP); }
		}
		
		[XmlElement]
		public double Y
		{
			get { return _y ; }
			set { _y = Math.Round(value,DP); }
		}
		
		[XmlElement]
		public double Z
		{
			get { return _z ; }
			set { _z = Math.Round(value,DP); }
		}
		
		[XmlElement]
		public int WindingNumber
		{
			get { return _wn; }
			set { _wn = value; }
		}
		
		[XmlIgnore]
		public double Depth
		{
			get { return _z; }	
			set { _z = Math.Round(value,DP); }
		}
		
		public bool PointAtInfinity
		{
			get {
				return  double.IsPositiveInfinity(_x) ||
					     double.IsPositiveInfinity(_y) ||
						 double.IsPositiveInfinity(_z);
			}
		}
		
		public ICoordinate Copy
		{
			get {return new Cartesian(this); }
		}
		
		[XmlIgnore]
		public double Length 
		{
			get { return (double)Math.Sqrt(_x * _x + _y * _y + _z * _z); }
		}
		
	
		
		public Cartesian (Cartesian c)
            : this(c.X,c.Y,c.Z){}
		

		public Cartesian(double x, double y, double z, int winding)
		{
            
			X = Math.Round(x,DP);
			Y = Math.Round(y,DP);
			Z = Math.Round(z,DP);
			WindingNumber = winding;
		}
		
        public Cartesian(double x, double y, double z) : this(x,y,z,0) {}
			
		public Cartesian() : this(0,0,0) {}
		
		public Cylindrical toCylindrical
		{
			get {
				return new Cylindrical(this);
			}
		}
		
		public Cartesian toCartesian3
		{
			get { return this; }
		}
		
		public Spherical toSpherical
		{
			get { return new Spherical(this); }	
		}
		
		public bool Equals( ICoordinate c)
		{
			return this.Equals(c.toCartesian3);
		}
		
		public void Add(ICoordinate c1)
		{
			Cartesian v1 = c1.toCartesian3;
			X += Math.Round(v1.X,DP);
			Y += Math.Round(v1.Y,DP);
			Z += Math.Round(v1.Z,DP);
		}
		
		public void Subtract(ICoordinate c)
		{
			Cartesian v1 = c.toCartesian3;
			X -= Math.Round(v1.X,DP);
			Y -= Math.Round(v1.Y,DP);
			Z -= Math.Round(v1.Z,DP);
		}
		
		public override string ToString ()
		{
			return string.Format("X:{0:f} Y:{1:f} Z:{2:f}",this.X,this.Y,this.Z);
		}
		
		public static Cartesian operator - (Cartesian c1, Cartesian c2)
		{
			return new Cartesian(c1.X - c2.X,
			                       c1.Y - c2.Y,
			                       c1.Z - c2.Z);
		}

		public static Cartesian operator + (Cartesian c1, Cartesian c2)
		{
			return new Cartesian(c1.X + c2.X,
			                       c1.Y + c2.Y,
			                       c1.Z + c2.Z);
		}
		
		public static Cartesian operator * (double v, Cartesian c1) {
			return new Cartesian(v * c1.X, v * c1.Y, v * c1.Z);	
		}
				
		public static Cartesian Linterp(Cartesian c1, double b, Cartesian c2)
		{
			Cartesian v = new Cartesian();
			v.X = b * c1.X + (1 - b) * c2.X;
			v.Y = b * c1.Y + (1 - b) * c2.Y;
			v.Z = b * c1.Z + (1 - b) * c2.Z;
			return v;
		}
		
		public bool Equals (Cartesian v)
		{
			
			double e =  1.0 / Math.Pow(10,DP);
			double vx = Math.Round(Math.Abs(_x - v._x),DP);
			double vy = Math.Round(Math.Abs(_y - v._y),DP);
			double vz = Math.Round(Math.Abs(_z - v._z),DP);
					
			bool same = (vx < e) && (vy < e) && (vz < e);
			return same;
		}

        //public void Transform(Transformation t)
        //{
        //    Cartesian c = t.Transform(this).toCartesian3;
        //    Coord = c.Coord;
        //}
	}
}
