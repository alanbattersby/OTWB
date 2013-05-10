using OTWB.Common;
using OTWB.Interfaces;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OTWB
{
    public class RossData : PathGenData
    {
        #region Properties
        double _ex1;
        public double Ex1
        {
            get { return _ex1; }
            set { SetProperty(ref _ex1, value, "EX1"); }
        }

        double _ex2;
        public double Ex2
        {
            get { return _ex2; }
            set { SetProperty(ref _ex2, value, "EX2"); }
        }

        double _sr;
        public double SR
        {
            get { return _sr; }
            set { SetProperty(ref _sr, value, "SR"); }
        }

        double _fl;
        public double Fl
        {
            get { return _fl; }
            set { SetProperty(ref _fl, value, "Fl"); }
        }

        double _fr;
        public double Fr
        {
            get { return _fr; }
            set { SetProperty(ref _fr, value, "Fr"); }
        }

        double _N;
        public double N
        {
            get { return _N; }
            set { SetProperty(ref _N, value, "N"); }
        }

        double _M;
        public double M
        {
            get { return _M; }
            set { SetProperty(ref _M, value, "M"); }
        }

        double _L;
        public double L
        {
            get { return _L; }
            set { SetProperty(ref _L, value, "L"); }
        }

        double _K;
        public double K
        {
            get { return _K; }
            set { SetProperty(ref _K, value, "K"); }
        }

        double _v4;
        public double V4
        {
            get { return _v4; }
            set { SetProperty(ref _v4, value, "V4"); }
        }

        double _Phi1;
        public double Phi1
        {
            get { return _Phi1; }
            set { SetProperty(ref _Phi1, value, "Phi1"); }
        }

        double _Phi2;
        public double Phi2
        {
            get { return _Phi2; }
            set { SetProperty(ref _Phi2, value, "Phi2"); }
        }

        double _Phi3;
        public double Phi3
        {
            get { return _Phi3; }
            set { SetProperty(ref _Phi3, value, "Phi3"); }
        }

        string _Script;
        public string Script
        {
            get { return _Script; }
            set { SetProperty(ref _Script, value, "Script"); }
        }

        #endregion
      
        public RossData() : base (PatternType.ross,0,2)
        {
            Script = "run()";
        }

        public RossData(int pnum) : base(PatternType.ross,pnum,2)
        {
        }
       
    }
}
