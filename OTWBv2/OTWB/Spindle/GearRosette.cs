using System;
using System.Xml.Serialization;
using System.ComponentModel;
using OTWB.Spindle;
using Windows.Foundation;


namespace OTWB.Spindle
{
   
    public class SpurGear : Rosette
    {
        double _thetaOD = 0;  // angle of side of tooth

      

        double _module;
        public double Module
        {
            get { return _module; }
            set { 
                SetProperty(ref _module, value, "Module");
                Name = SuggestedName;
                CalcDiameters();
            }
        }

        double _dp;
        public double DiametralPitch
        {
            get { return _dp; }
            set {
                SetProperty(ref _dp, value, "DP");
                Name = SuggestedName;
                CalcDiameters();
            }
        }

        double _pressureAngle;
        public double PressureAngle
        {
            get { return _pressureAngle; }
            set { 
                SetProperty(ref _pressureAngle, value, "PressureAngle");
                Name = SuggestedName;
                CalcDiameters();
            }
        }

        int _numTeeth;
        public int NumTeeth
        {
            get { return _numTeeth; }
            set { 
                SetProperty(ref _numTeeth, value, "NumTeeth");
                Name = SuggestedName;
                CalcDiameters();
            }
        }

        int _actTeeth;
        public int ActualTeeth
        {
            get { return _actTeeth; }
            set { SetProperty(ref _actTeeth, value, "ActualTeeth"); }
        }

        double _undercut;
        public double Undercut
        {
            get { return _undercut; }
            set { SetProperty(ref _undercut, value, "UnderCut");  }
        }
    
        protected override string SuggestedName
        {
            get
            {
                return string.Format(SuggestedNameTemplate, 
                    Math.Round(Module, ApproxToNDP), NumTeeth, PressureAngle); 
            }
        }

        [XmlIgnore]
        bool Valid
        {
            get
            {
                return (Module > 0) && (NumTeeth > 0) && (PressureAngle > 0);
            }
        }
      
        public SpurGear() : this(1.0, 12) {}
 
        public SpurGear(double mod, int n, double pa = 20.0) 
			:base((mod * n / 2) * Math.Cos(rad *pa)) 
		{
            SuggestedNameTemplate = "Gear-Mod{0}-N{1}-Pa{2}";
			_module = mod;
			_numTeeth = n;
            _pressureAngle = pa;
            CalcDiameters();
		}
        
        [XmlIgnore]
        public double Addendum
        {
            get { return Module; }
        }
        [XmlIgnore]
        public double Dedendum
        {
            get { return Math.Round(Module * 1.25,ApproxToNDP); }
        }

        [XmlIgnore]
        public double TopLandWidth
        {
            get { return Math.Round(Module * 0.25,ApproxToNDP); }
        }

        double _wholedepth;
        [XmlIgnore]
        public double WholeDepth
        {
            get { return _wholedepth; }
            set { SetProperty(ref _wholedepth, value); }
        }

        double _pitchDiameter;
        [XmlIgnore]
        public double PitchDiameter
        {
            get { return _pitchDiameter; }
            set { SetProperty(ref _pitchDiameter, value); }
        }

        double _rootDiameter;
        [XmlIgnore]
        public double RootDiameter
        {
            get { return _rootDiameter; }
            set { SetProperty(ref _rootDiameter, value); }
        }

        double _outsideDiameter;
        [XmlIgnore]
        public double OutsideDiameter
        {
            get { return _outsideDiameter; }
            set { SetProperty(ref _outsideDiameter, value); }
        }

        double _baseDiameter;
        [XmlIgnore]
        public double BaseDiameter
        {
            get  { return _baseDiameter; }
            set  { SetProperty(ref _baseDiameter, value); }
        }

        void CalcDiameters()
        {
           
            WholeDepth = Module * 2.25;
            PitchDiameter = Module * NumTeeth;
            OutsideDiameter = Module * (NumTeeth + 2);
            BaseDiameter = PitchDiameter * Math.Cos(rad * PressureAngle);
            RootDiameter = PitchDiameter -2.5 * Module;
            ToothPitchAngle = twopi / NumTeeth;
            CalcToothSideAngle();
            Undercut = 0;
        }

        [XmlIgnore]
        public double CircularPitch
        {
            get { return Math.Round(Math.PI * Module,ApproxToNDP); }
        }

        [XmlIgnore]
        public double CircularToothThickness
        {
            get { return Math.Round(CircularPitch / 2,ApproxToNDP); }
        }

        double _toothPitchAngle;
        [XmlIgnore]
        public double ToothPitchAngle
        {
            get { return _toothPitchAngle;}
            set { SetProperty(ref _toothPitchAngle, value); }
        }

        [XmlIgnore]
        public double NormalToothThickness
        {
            get { 
                double tt = PitchDiameter * Math.Sin(rad * 90 / NumTeeth);
                return Math.Round(tt,ApproxToNDP);
            }
        }

        // angle in degrees
        [XmlIgnore]
        public double TopLandAngle
        {
            get
            {
                double l = 2 * Math.Asin(TopLandWidth / OutsideDiameter);
                return Math.Round(l, ApproxToNDP);
            }
        }

        [XmlIgnore]
        public double ToothSideAngle
        {
            get { return _thetaOD;   }
        }

        public void CalcToothSideAngle()
        {
            double phiODr = Phi_Of_D(OutsideDiameter);  // inv param angle at outer diameter
            _thetaOD = Math.Round( Theta_Of_Phi(phiODr), ApproxToNDP); 
        }

        /// <summary>
        /// param lies between 0 and 1
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override double OffsetAt(double param)
        {
            double angle = param * twopi + Phase_In_Radians;

            //if (Valid && (angle < ActualTeeth * ToothPitchAngle ))
            //{
                double rot = Math.Floor(angle / ToothPitchAngle) * ToothPitchAngle;
                double adiff = angle - rot;


                // angle of point at outer diameter

                double l1 = _thetaOD + TopLandAngle;
                double l2 = l1 + _thetaOD;

                if (adiff < _thetaOD)
                {
                    //Point p = Involute1(BaseDiameter, adiff);
                    //return Utilities.Length(p);
                    //return Undercut + InvRadius(BaseDiameter, adiff);
                    return Math.Min(Undercut + InvRadius(BaseDiameter, adiff), OutsideDiameter / 2);
                }
                else if (adiff > _thetaOD && adiff <= l1)
                {
                    return OutsideDiameter / 2;
                }
                else if ((adiff > l1) && (adiff <= l2))
                {
                    double a = l2 - adiff;
                    return Undercut + InvRadius(BaseDiameter, a);
                }
                else
                {
                    return RootDiameter / 2;
                }
   
        }

        public Point Involute(double d, double a)
        {
            double phi = Phi_Of_Theta(a);
            double y = d/2 * (Math.Cos(phi) + phi * Math.Sin(phi));
            double x = d/2 * (Math.Sin(phi) - phi * Math.Cos(phi));
            return new Point(y, x);
        }

        public double InvRadius(double d, double a )
        {
            double phi = Phi_Of_Theta(a);
            return (d / 2) * Math.Sqrt(1 + phi * phi);
        }

        // angle in radians of point at (r, phi) on involute 
        // theta = tan(phi_polar) - phi_polar
        // result in radians
        public double Inv(double phi)
        {
            return Math.Tan(phi) - phi;
        }

        // returns value of theta based on phi
        // parameter phi is in radians
        // theta = atan ( tan(phi) - phi / (1 + phi * tan(phi))
        public double Theta_Of_Phi(double phi)
        {
            double t = Math.Tan(phi);
            return Math.Atan2(t - phi, 1 + phi * t);
        }

        // Returns value of phi based on diameter d
        // value in radians
        public double Phi_Of_D(double d)
        {
            double rd = BaseDiameter;
            return Math.Sqrt(((d * d) / (rd * rd)) - 1);
        }

        // Binary search for phi
        // angle is in radians
        // result in radians
        public double Phi_Of_Theta(double theta)
        {
            double phi = 0;
            double angle0 = 0;
            double angle1 = Math.PI / 2;
            double InvDif = theta;
           
            while (Math.Abs(InvDif) >0.00000001)
            {
                phi = (angle0 + angle1) / 2;
                InvDif = Inv(phi) - theta;
                if (InvDif > 0.0)
                    angle1 = phi;
                else
                    angle0 = phi;
            }
            return phi;
        }
    }
}
