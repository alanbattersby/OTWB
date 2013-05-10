using OTWB.Common;
using OTWB.Interfaces;
using OTWB.PathGenerators;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;
using System.Xml;
using OTWB.Spindle;
using Windows.UI.Xaml.Media;
using OTWB.Braid;
using OTWB.Lattice;
using OTWB.Collections;
using OTWB.Profiles;
using OTWB.Coordinates;

namespace OTWB
{
    /// <summary>
    /// This class is used to hold the data on all Bazeley and Ross 
    /// Defined Patterns
    /// </summary>
    public class ViewModel : BindableBase
    {
        public event EventHandler WheelsDataChanged;
        public event EventHandler LatticeDataChanged;
        public event EventHandler BraidDataChanged;

        public static bool NeedsInitialising { get; set; }

        // Now we have the list of Bazeley patterns
        public ObservableCollection<BazelyChuck> _BazelyPatterns;
        public ObservableCollection<BazelyChuck> BazeleyPatterns
        {
            get { return _BazelyPatterns; }
            set { SetProperty(ref _BazelyPatterns, value, "BazelyPatterns"); }
        }

        public ObservableCollection<RossData> _RossPatterns;
        public ObservableCollection<RossData> RossPatterns
        {
            get { return _RossPatterns; }
            set { SetProperty(ref _RossPatterns, value, "RossPatterns"); }
        }

        public ObservableCollection<WheelsData> _WheelsPatterns;
        public ObservableCollection<WheelsData> WheelsPatterns
        {
            get { return _WheelsPatterns; }
            set { SetProperty(ref _WheelsPatterns, value, "WheelsPatterns"); }
        }

        PathData _lastBazleyPattern;
        public PathData LastBazleyPattern
        {
            get { return _lastBazleyPattern; }
            set { SetProperty(ref _lastBazleyPattern, value, "LastBazelyPattern"); }
        }

        double _lastBazleyIncrement;
        public double LastBazleyIncrement
        {
            get { return _lastBazleyIncrement; }
            set { SetProperty(ref _lastBazleyIncrement, value, "LastBazelyIncrement"); }
        }

        PathData _lastRossPattern;
        public PathData LastRossPattern
        {
            get { return _lastRossPattern; }
            set { SetProperty(ref _lastRossPattern, value, "LastRossPattern"); }
        }
        double _lastRossIncrement;
        public double LastRossIncrement
        {
            get { return _lastRossIncrement; }
            set { SetProperty(ref _lastRossIncrement, value, "LastBazelyIncrement"); }
        }

        PathData _lastWheelsPattern;
        public PathData LastWheelsPattern
        {
            get { return _lastWheelsPattern; }
            set { SetProperty(ref _lastWheelsPattern, value, "LastWheelsPattern"); }
        }
        double _lastWheelsIncrement;
        public double LastWheelsIncrement
        {
            get { return _lastWheelsIncrement; }
            set { SetProperty(ref _lastWheelsIncrement, value, "LastWheelsIncrement"); }
        }

        public ObservableCollection<Barrel> _BarrelsPatterns;
        public ObservableCollection<Barrel> BarrelPatterns
        {
            get { return _BarrelsPatterns; }
            set { SetProperty(ref _BarrelsPatterns, value, "BarrelsPatterns"); }
        }

        PathData _lastBarrelPattern;
        public PathData LastBarrelPattern
        {
            get { return _lastBarrelPattern; }
            set { SetProperty(ref _lastBarrelPattern, value, "LastBarrelPattern"); }
        }

        double _lastBarrelIncrement;
        public double LastBarrelIncrement
        {
            get { return _lastBarrelIncrement; }
            set { SetProperty(ref _lastBarrelIncrement, value, "LastBarrelIncrement"); }
        }

        public ObservableCollection<LatticeData> _LatticePatterns;
        public ObservableCollection<LatticeData> LatticePatterns
        {
            get { return _LatticePatterns; }
            set { SetProperty(ref _LatticePatterns, value); }
        }

        PathData _lastLatticePattern;
        public PathData LastLatticePattern
        {
            get { return _lastLatticePattern; }
            set { SetProperty(ref _lastLatticePattern, value); }
        }

        double _lastLatticeIncrement;
        public double LastLatticeIncrement
        {
            get { return _lastLatticeIncrement; }
            set { SetProperty(ref _lastLatticeIncrement, value); }
        }
        public ObservableCollection<BraidData> _BraidPatterns;
        public ObservableCollection<BraidData> BraidPatterns
        {
            get { return _BraidPatterns; }
            set { SetProperty(ref _BraidPatterns, value); }
        }

        PathData _lastBraidPattern;
        public PathData LastBraidPattern
        {
            get { return _lastBraidPattern; }
            set { SetProperty(ref _lastBraidPattern, value); }
        }

        double _lastBraidIncrement;
        public double LastBraidIncrement
        {
            get { return _lastBraidIncrement; }
            set { SetProperty(ref _lastBraidIncrement, value); }
        }
        //TODO add Other path options to Save and restore
        void SaveCurrentPathdata()
        {
            if (_currentPathData != null)
            {
                if (_currentPathData.PathType == PatternType.bazley)
                {
                    LastBazleyPattern = _currentPathData;
                    LastBazleyIncrement = Increment;
                }
                else if (_currentPathData.PathType == PatternType.ross)
                {
                    LastRossPattern = _currentPathData;
                    LastRossIncrement = Increment;
                }
                else if (_currentPathData.PathType == PatternType.wheels)
                {
                    LastWheelsPattern = _currentPathData;
                    LastWheelsIncrement = Increment;
                }
                else if (_currentPathData.PathType == PatternType.barrel)
                {
                    LastBarrelPattern = _currentPathData;
                    LastBarrelIncrement = Increment;
                }
                else if ((_currentPathData.PathType == PatternType.latticeRim) ||
                        (_currentPathData.PathType == PatternType.latticeFace))
                {
                    LastLatticePattern = _currentPathData;
                    LastLatticeIncrement = Increment;
                }
                else if (_currentPathData.PathType == PatternType.braid)
                {
                    LastBraidPattern = _currentPathData;
                    LastBraidIncrement = Increment;
                }
                else  // defaults to Bazley
                {
                    LastBazleyPattern = _currentPathData;
                    LastBazleyIncrement = Increment;
                }
            }
        }
        void RestoreLastPathdata(PatternType typ)
        {
            if (typ == PatternType.bazley)
            {
                _currentPathData = (_lastBazleyPattern != null) ? _lastBazleyPattern : BazeleyPatterns[0];
                _increment = (_lastBazleyIncrement > 0) ? _lastBazleyIncrement : 0.001;
            }
            else if (typ == PatternType.ross)
            {
                _currentPathData = (_lastRossPattern != null) ? _lastRossPattern : RossPatterns[0];
                _increment = (_lastRossIncrement > 0) ? _lastRossIncrement : 0.0005;
            }
            else if (typ == PatternType.wheels)
            {
                _currentPathData = (_lastWheelsPattern != null) ? _lastWheelsPattern : null;
                _increment = (_lastWheelsIncrement > 0) ? _lastWheelsIncrement : 0.001;
            }
            else if (typ == PatternType.barrel)
            {
                _currentPathData = (_lastBarrelPattern != null) ? _lastBarrelPattern : null;
                _increment = (_lastBarrelIncrement > 0) ? _lastBarrelIncrement : 0.001;
            }
            else if ((typ == PatternType.latticeRim) || (typ == PatternType.latticeFace))
            {
                _currentPathData = (_lastLatticePattern != null) ? _lastLatticePattern : null;
                _increment = (_lastLatticeIncrement > 0) ? _lastLatticeIncrement : 0.01;
            }
            else if (typ == PatternType.braid)
            {
                _currentPathData = (_lastBraidPattern != null) ? _lastBraidPattern : null;
                _increment = (_lastBraidIncrement > 0) ? _lastBraidIncrement : 0.01;
            }
            else  // defaults to Bazley
            {
                _currentPathData = (_lastBazleyPattern != null) ? _lastBazleyPattern : BazeleyPatterns[0];
                _increment = (_lastBazleyIncrement > 0) ? _lastBazleyIncrement : 0.001;
            }
        }

        PathData _currentPathData = new Barrel();
        public PathData CurrentPathData
        {
            get { return _currentPathData; }
            set
            {
                if (value != _currentPathData)
                {
                    if ((value == null) && (_currentPathData != null))
                    {
                        SaveCurrentPathdata();
                    }
                    else if ((_currentPathData == null) && (value != null))
                    {
                        if (value.PathType == PatternType.bazley)
                            LastBazleyPattern = value;
                        else if (value.PathType == PatternType.ross)
                            LastRossPattern = value;
                        else if (value.PathType == PatternType.wheels)
                            LastWheelsPattern = value;
                        else if (value.PathType == PatternType.barrel)
                            LastBarrelPattern = value;
                        else if (value.PathType == PatternType.latticeRim)
                            LastLatticePattern = value;
                        else if (value.PathType == PatternType.latticeFace)
                            LastLatticePattern = value;
                        else if (value.PathType == PatternType.braid)
                            LastBraidPattern = value;
                    }
                    if (value != null)
                    {
                        try
                        {
                            //SetProperty(ref _currentPathData, (value as PathData));
                            _currentPathData = value;
                            CreatePaths();
                        }
                        catch (System.ArgumentException sae)
                        {
                            Debug.WriteLine(sae.InnerException);
                        }
                    }
                }
            }
        }

        Profile _profile;
        public Profile CurrentProfile
        {
            get { return _profile; }
            set { SetProperty(ref _profile, value); }
        }

        IPathGenerator _pathEngine;

        double _increment;
        public double Increment
        {
            get { return _increment; }
            set 
            { 
                SetProperty(ref _increment, value);
            }
        }

        public void CreateData()
        {
            BazeleyPatterns = new ObservableCollection<BazelyChuck>();
            _lastBazleyPattern = null;
            RossPatterns = new ObservableCollection<RossData>();
            _currentPath = new ShapeCollection();
            _lastRossPattern = null;
            WheelsPatterns = new ObservableCollection<WheelsData>();
            _lastWheelsPattern = null;
            BarrelPatterns = new ObservableCollection<Barrel>();
            _lastBarrelPattern = null;
            LatticePatterns = new ObservableCollection<LatticeData>();
            _lastLatticePattern = null;
            BraidPatterns = new ObservableCollection<BraidData>();
            _lastBraidPattern = null;

            CreateBazelyData();
            CreateRossData();
            CreateWheelsData();
            CreateBarrelData();
            CreateLatticeData();
            CreateBraidData();
            NeedsInitialising = false;
        }

       
        public ViewModel()
        {     
            NeedsInitialising = true;
            LastBazleyPattern = new BazelyChuck();
            LastRossPattern = new RossData();
            LastWheelsPattern = new WheelsData();
            LastBarrelPattern = new Barrel();
            LastLatticePattern = new LatticeData();
            LastBraidPattern = new BraidData();
        }

        private void CreateBraidData()
        {
            BraidData b = new BraidData();
            b.Repeats = 4;
            Permutation p = new Permutation();
            p.SetPermOf(0, 1);
            p.SetPermOf(1, 0);
            b.Add(p);
            p = new Permutation();
            b.Add(p);
            p = new Permutation();
            p.SetPermOf(0, 1);
            p.SetPermOf(1, 0);
            b.Add(p);
            BraidPatterns.Add(b);
            b.PropertyChanged += b_PropertyChanged;
        }

        void b_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (BraidDataChanged != null)
                BraidDataChanged(this, new EventArgs());
        }

        private void CreateLatticeData()
        {
            LatticeData l = new LatticeData(1);
            l.Layout.ToolPosition = 60;
            l.Rows = 10;
            l.Columns = 10;
            l.Layout.RepeatX = 10;
            l.Layout.RepeatY = 10;
            l.Layout.Width = 30;
            l.Layout.Height = 30;
            l.Layout.Clip = true;
            l.Layout.ClipRange.Start = 50;
            l.Layout.ClipRange.End = 200;
            l.PropertyChanged += l_PropertyChanged;
            l.Add(new Line2D(0, 0, 5, 0));
            l.Add(new Line2D(0, 5, 5, 5));
            l.Add(new Line2D(0, 0, 0, 5));
            l.Add(new Line2D(5, 0, 5, 5));
            l.Add(new Line2D(0, 0, 5, 5));
            l.Add(new Line2D(5, 0, 0, 5));
            l.LineChanged += l_LineChanged;
            l.LineRemoved += l_LineChanged;
            LatticePatterns.Add(l);
        }

        void l_LineChanged(object sender, Line2D e)
        {
            if (LatticeDataChanged != null)
                LatticeDataChanged(this, new EventArgs());
        }

        void Lines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        void l_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "K" && (!(sender as LatticeData).Layout.Hyper))
                return;
            if (LatticeDataChanged != null)
                LatticeDataChanged(this, new EventArgs());
        }


        private void CreateBarrelData()
        {
            Barrel b = new Barrel(0);
            b.Add(new OTWB.Spindle.Ellipse(0.3));
            BarrelPatterns.Add(b);
            b = new Barrel(1);
            b.Add(new OTWB.Spindle.Wave(8, 1, 0));
            BarrelPatterns.Add(b);
            b = new Barrel(2);
            b.Add(new OTWB.Spindle.Poly(5));
            BarrelPatterns.Add(b);

           
        }

        public string CurrentPatternName
        {
            get
            {
                if (CurrentPath == null)
                    return string.Empty;
                else
                    return CurrentPath.PatternName;
            }

            set
            {
                try
                {
                    CurrentPath.PatternName = value;
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Need to
        /// Set CurrentPaths to null
        /// Set CurrentPathData to null
        /// Set MaxPathRadius to ZERO
        /// </summary>
        public void CleanUpForPageChange(bool clearCurrentPath)
        {
            if (clearCurrentPath)
            {
                CurrentPath = null;  
                SaveCurrentPathdata();
                CurrentPathData = null;
            }
           
            SelectedPathIndex = -1; 
            _pathEngine = null;

        }

        void SetupEngine(PatternType ptyp)
        {
            switch (ptyp)
            {
                case PatternType.barrel:
                    _pathEngine = new OffsetPathEngine();
                    break;
                case PatternType.bazley:
                    _pathEngine = new BazelyEngine();
                    break;
                case PatternType.ross:
                    _pathEngine = new RossEngine();
                    break;
                case PatternType.wheels:
                    _pathEngine = new WheelsEngine();
                    break;
                case PatternType.latticeRim:
                    _pathEngine = new LatticeRimEngine();
                    break;
                case PatternType.latticeFace:
                    _pathEngine = new LatticeFaceEngine();
                    break;
                case PatternType.braid:
                    _pathEngine = new BraidEngine();
                    break;
            }
        }

        public void SetupPattern(PatternType ptyp, PathData d, double inc)
        {
            if (d == null)
            {
                RestoreLastPathdata(ptyp);
            }
            else
            {
                CurrentPathData = d;
            }

            if (inc > 0)
                Increment = inc;
            SetupEngine(ptyp);
        }

        int _numPoints;
        public int NumPoints
        {
            get {return _numPoints; }
            set { SetProperty(ref _numPoints, value); }
        }

        double _workDiameter;
        public double WorkDiameter
        {
            get { return _workDiameter; }
            set { SetProperty(ref _workDiameter, value); }
        }
     
        ShapeCollection _currentPath;
        public ShapeCollection CurrentPath
        {
            get { return _currentPath;  }
            set { SetProperty(ref _currentPath, value); }
        }

        protected int _selectedPathIndex;
        public int SelectedPathIndex
        {
            get { return _selectedPathIndex; }
            set { SetProperty(ref _selectedPathIndex, value, "SelectedRossPathIndex"); }

        }
     
        void CreateRossData()
        {
            RossPatterns = new ObservableCollection<RossData>();
            RossData val = new RossData(0);
            val.Ex1 = 70; val.SR = 20; val.Fl = 12; val.Fr = -22; val.N = -2; val.M = 159; val.L = -161;
            RossPatterns.Add(val);
            val = new RossData(1);
            val.Ex1 = 70; val.SR = 20; val.Fl = 12; val.Fr = 22; val.N = -2; val.M = 159; val.L = -161;
            RossPatterns.Add(val);
            val = new RossData(2);
            val.Ex1 = 60; val.SR = 60; val.Fl = 12; val.Fr = -27; val.N = -2; val.M = -121; val.L = 119;
            val.Phi1 = 60; val.Phi2 = 60; val.Phi3 = 60; val.Ex2 = 0; val.M = 0; val.V4 = 0;
            RossPatterns.Add(val);
            val = new RossData(3);
            val.Ex1 = 60; val.SR = 10; val.Fl = 10; val.Fr = -9; val.N = -2; val.M = -13; val.L = 11;
            val.Phi1 = 180; val.Phi2 = 180; val.Phi3 = 180;
            RossPatterns.Add(val);
            val = new RossData(4);
            val.Ex1 = 60; val.SR = 10; val.Fl = 10; val.Fr = -9; val.N = -2; val.M = -13; val.L = 11;
            val.Phi1 = 180;
            RossPatterns.Add(val);
            val = new RossData(5);
            val.Ex1 = 60; val.SR = 10; val.Fl = 10; val.Fr = 9; val.N = -2; val.M = -13; val.L = 11;
            val.Phi1 = 180; val.Phi2 = 180;
            RossPatterns.Add(val);
            val = new RossData(6);
            val.Ex1 = 60; val.SR = 10; val.Fl = 10; val.Fr = 9; val.N = -2; val.M = -13; val.L = 11;
            val.Phi1 = 180;
            RossPatterns.Add(val);
            val = new RossData(7);
            val.SuggestedMaxTurns = 4;
            val.Ex1 = 75; val.SR = 45; val.Fl = 10; val.Fr = -23; val.N = -3.5; val.M = 202.5; val.L = -207.5;
            val.Phi1 = 135; val.Phi2 = 135; val.Phi3 = 135;
            RossPatterns.Add(val);
            val = new RossData(8);
            val.Ex1 = 90; val.SR = 10; val.Fl = 12; val.Fr = -20; val.N = -4; val.M = -189; val.L = 183;
            RossPatterns.Add(val);
            val = new RossData(9);
            val.Ex1 = 75; val.SR = 40; val.Fl = 12; val.Fr = 18; val.N = -4; val.M = -253; val.L = 247;
            RossPatterns.Add(val);
            val = new RossData(10);
            val.Ex1 = 80; val.SR = 11; val.Fl = 23; val.Fr = -20; val.N = -4; val.M = -131; val.L = 253;
            val.Phi2 = 90;
            RossPatterns.Add(val);
            val = new RossData(11);
            val.Ex1 = 80; val.SR = 11; val.Fl = 23; val.Fr = -20; val.N = -4; val.M = -131; val.L = 253;
            val.Phi1 = 180; val.Phi2 = 180; val.Phi3 = 180;
            RossPatterns.Add(val);
            val = new RossData(12);
            val.Ex1 = 75; val.SR = 12; val.Fl = 25; val.Fr = -14; val.N = -4; val.M = -131; val.L = 189;
            val.Phi1 = 180; val.Phi2 = 180; val.Phi3 = 180;
            RossPatterns.Add(val);
            val = new RossData(13);
            val.Ex1 = 70; val.SR = 9; val.Fl = 40; val.Fr = -10; val.N = -4; val.M = 189; val.L = -195;
            val.Phi1 = 180; val.Phi3 = 180;
            RossPatterns.Add(val);
            val = new RossData(14);
            val.Ex1 = 80; val.SR = 13; val.Fl = 25; val.Fr = -6; val.N = -4; val.M = 189; val.L = 195;
            RossPatterns.Add(val);
            val = new RossData(15);
            val.Ex1 = 100; val.SR = 10; val.Fl = 12; val.Fr = -12; val.N = 8; val.M = 375; val.L = -357;
            val.Phi1 = 180; val.Phi2 = 180; val.Phi3 = 180;
            RossPatterns.Add(val);
            val = new RossData(16);
            val.Ex1 = 100; val.SR = 12; val.Fl = 12; val.Fr = 11; val.N = 8; val.M = 375; val.L = -357;
            RossPatterns.Add(val);
            val = new RossData(17);
            val.Ex1 = 90; val.SR = 14; val.Fl = 15; val.Fr = -14; val.N = -8; val.M = 313; val.L = -327;
            RossPatterns.Add(val);
            val = new RossData(18);
            val.Ex1 = 100; val.SR = 9; val.Fl = 12; val.Fr = 11; val.N = -8; val.M = 313; val.L = -327;
            RossPatterns.Add(val);
            val = new RossData(19);
            val.Ex1 = 100; val.SR = 50; val.Fl = -19; val.Fr = -7; val.N = 9; val.M = -17; val.L = 1000;
            RossPatterns.Add(val);
            val = new RossData(20);
            val.Ex1 = 85; val.SR = 40; val.Fl = 25; val.Fr = 6; val.N = -6; val.M = 19; val.L = 1000;
            RossPatterns.Add(val);
            val = new RossData(21);
            val.Ex1 = 90; val.SR = 20; val.Fl = 25; val.Fr = -15; val.N = -18; val.M = 19; val.L = 1000;
            RossPatterns.Add(val);
            val = new RossData(22);
            val.Ex1 = 90; val.SR = 20; val.Fl = 30; val.Fr = 20; val.N = 15; val.M = -14; val.L = 600;
            RossPatterns.Add(val);
            val = new RossData(23);
            val.Ex1 = 17; val.SR = 34; val.Fl = 34; val.Fr = 9; val.N = 2; val.M = -1; val.L = 192;
            RossPatterns.Add(val);
            val = new RossData(24);
            val.Ex2 = 17; val.Ex1 = 34; val.SR = 34; val.Fl = 12; val.Fr = 12;
            val.V4 = 2; val.M = -1; val.L = 189; val.K = -191;
            RossPatterns.Add(val);
            val = new RossData(25);
            val.Ex2 = 17; val.Ex1 = 34; val.SR = 34; val.Fl = 12; val.Fr = -12;
            val.V4 = 2; val.M = -1; val.L = 189; val.K = -191;
            RossPatterns.Add(val);
            val = new RossData(26);
            val.Ex2 = 80; val.Ex1 = 16; val.SR = 10; val.Fl = 9; val.Fr = 7;
            val.V4 = -2; val.M = 11; val.L = 469; val.K = -447;
            RossPatterns.Add(val);
            val = new RossData(27);
            val.Ex2 = 80; val.Ex1 = 30; val.SR = 11; val.Fl = -10; val.Fr = 0;
            val.V4 = -8; val.M = 249; val.L = -247; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(28);
            val.Ex2 = 80; val.Ex1 = 21; val.SR = 19; val.Fl = 20; val.Fr = 0;
            val.V4 = -8; val.M = 249; val.L = -247; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(29);
            val.Ex2 = 80; val.Ex1 = 21; val.SR = 19; val.Fl = -20; val.Fr = 0;
            val.V4 = -7; val.M = 330; val.L = -321; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(30);
            val.Ex2 = 70; val.Ex1 = 13; val.SR = 20; val.Fl = 23; val.Fr = 0;
            val.V4 = -5; val.M = 236; val.L = -229; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(31);
            val.Ex2 = 80; val.Ex1 = 32; val.SR = 19; val.Fl = -18; val.Fr = 0;
            val.V4 = 8; val.M = -375; val.L = 369; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(32);
            val.Ex2 = 75; val.Ex1 = 30; val.SR = 18; val.Fl = 17; val.Fr = 0;
            val.V4 = -6; val.M = 379; val.L = -365; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(33);
            val.Ex2 = 80; val.Ex1 = 32; val.SR = 8; val.Fl = -14; val.Fr = 0;
            val.V4 = 5; val.M = -314; val.L = 306; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(34);
            val.Ex2 = 70; val.Ex1 = 35; val.SR = 9; val.Fl = 18; val.Fr = 0;
            val.V4 = -6; val.M = 251; val.L = -245; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(35);
            val.Ex2 = 80; val.Ex1 = 35; val.SR = 11; val.Fl = -12; val.Fr = 0;
            val.V4 = 6; val.M = 331; val.L = -299; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(36);
            val.Ex2 = 80; val.Ex1 = 40; val.SR = 10; val.Fl = 11; val.Fr = 0;
            val.V4 = 6; val.M = 331; val.L = -299; val.K = 0;
            RossPatterns.Add(val);
            val = new RossData(37);
            val.Ex2 = 95; val.Ex1 = 30; val.SR = 30; val.Fl = 8; val.Fr = 7;
            val.V4 = -8; val.M = 13; val.L = 662; val.K = -588;
            RossPatterns.Add(val);
            val = new RossData(38);
            val.Ex2 = 96; val.Ex1 = 36; val.SR = 30; val.Fl = 9; val.Fr = -8;
            val.V4 = -8; val.M = 11; val.L = 662; val.K = -592;
            RossPatterns.Add(val);

        }

        public async Task ExportCurrentPath()
        {
            // Code to call file picker
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Xml", new List<string>() { ".xml" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = _currentPathData.Name;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                IRandomAccessStream sessionRandomAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
                List<List<Point>> Allpaths = CurrentPath.AllPoints;
                var serializer = new XmlSerializer(typeof(List<List<Point>>));
                serializer.Serialize(sessionOutputStream.AsStreamForWrite(), Allpaths);
                sessionRandomAccess.Dispose();
                await sessionOutputStream.FlushAsync();
                sessionOutputStream.Dispose();
            }
        }

        public void CreatePaths()
        {
            //Debug.WriteLine("In ViewModel.CreatePaths with Increment {0}", Increment);
            if (_currentPathData == null)
            {
                //Debug.WriteLine("Exit Viewmodel.CreatePaths as null path data");
                return;
            }
            _currentPath = _pathEngine.CreatePaths(_currentPathData, Increment);
            NumPoints =  CurrentPath.NumPoints;
         }

        async void CreateBazelyData()
        {
            string data;
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"GEODATA.txt");
            var stream = await file.OpenStreamForReadAsync();
            var rdr = new StreamReader(stream);
            data = await rdr.ReadToEndAsync();
            var lines = data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var index = 0;
            foreach (string line in lines)
            {
                var info = line.Split(new char[] { ':' });
                int indx = int.Parse(info[0].Substring(1, info[0].Length - 2));
                string[] pdata = info[1].Split(new char[] { ',' });
                BazelyChuck bc = new BazelyChuck(index++, pdata);
                _BazelyPatterns.Add(bc);
            }
            _lastBazleyPattern = _BazelyPatterns[0];
        }

        void CreateWheelsData()
        {
           
            WheelsData val = new WheelsData(0);
            val.PathDataChanged +=val_StageDataChanged;
            val.Add(50, 1);
            val.Add(25, 7);
            val.Add(16.667, -17);
            WheelsPatterns.Add(val);
            val = new WheelsData(1);
            val.PathDataChanged += val_StageDataChanged;
            val.Add(50, 1);
            val.Add(25, -16);
            val.Add(16.667, -17);
            WheelsPatterns.Add(val);
        }

        private void val_StageDataChanged(object sender, EventArgs e)
        {
            if (WheelsDataChanged != null)
                WheelsDataChanged(this, new EventArgs());
        }

        async void ReadWheelsData()
        {
            string data;
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"GEODATA.txt");
            var stream = await file.OpenStreamForReadAsync();
            var rdr = new StreamReader(stream);
            data = await rdr.ReadToEndAsync();
            var lines = data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var index = 0;
            foreach (string line in lines)
            {
                var info = line.Split(new char[] { ':' });
                int indx = int.Parse(info[0].Substring(1, info[0].Length - 2));
                string[] pdata = info[1].Split(new char[] { ',' });
                BazelyChuck bc = new BazelyChuck(index++, pdata);
                _BazelyPatterns.Add(bc);
            }
            _lastWheelsPattern = _WheelsPatterns[0];
        }

        public async void ExportCurrentPatternData()
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            if (CurrentPathData.PathType == PatternType.wheels)
                savePicker.FileTypeChoices.Add("wheel",new List<string>() {".wheel"});
            else if (CurrentPathData.PathType == PatternType.ross)
                savePicker.FileTypeChoices.Add("ross", new List<string>() { ".ross"});
            else if (CurrentPathData.PathType == PatternType.bazley)
                savePicker.FileTypeChoices.Add("Bazley", new List<string>() { ".baz"});
            else if (CurrentPathData.PathType == PatternType.barrel)
                savePicker.FileTypeChoices.Add("barrel", new List<string>() { ".bar"});
            else if (CurrentPathData.PathType == PatternType.latticeRim)
                savePicker.FileTypeChoices.Add("lattice", new List<string>() { ".lattice" });
            else if (CurrentPathData.PathType == PatternType.braid)
                savePicker.FileTypeChoices.Add("braid", new List<string>() { ".braid" });
            //savePicker.FileTypeChoices.Add("Xml", new List<string>() { ".xml" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = _currentPathData.Name;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                try
                {
                    IRandomAccessStream sessionRandomAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
                    IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
                    XmlSerializer serializer;
                    if (CurrentPathData.PathType == PatternType.bazley)
                    {
                        serializer = new XmlSerializer(typeof(BazelyChuck));
                        serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as BazelyChuck));
                    }
                    else if (CurrentPathData.PathType == PatternType.ross)
                    {
                        serializer = new XmlSerializer(typeof(RossData));
                        serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as RossData));
                    }
                    else if (CurrentPathData.PathType == PatternType.wheels)
                    {
                        serializer = new XmlSerializer(typeof(WheelsData));
                        serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as WheelsData));
                    }
                    else if (CurrentPathData.PathType == PatternType.barrel)
                    {
                        serializer = new XmlSerializer(typeof(Barrel));
                        serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as Barrel));
                    }
                    else if (CurrentPathData.PathType == PatternType.latticeRim)
                    {
                        serializer = new XmlSerializer(typeof(LatticeData));
                        serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as LatticeData));
                    }
                    else if (CurrentPathData.PathType == PatternType.braid)
                    {
                        serializer = new XmlSerializer(typeof(BraidData));
                        serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as BraidData));
                    }
                    sessionRandomAccess.Dispose();
                    await sessionOutputStream.FlushAsync();
                    sessionOutputStream.Dispose();
                }
                catch (Exception e)
                {
                    
                }
            }
        }

        bool ContainsPatternIndex(ObservableCollection<PathGenData> l, int indx)
        {
            return l.Select(p => p.PatternIndex == indx).Count() > 0;
        }

        public async void ImportPattern(PatternType typ)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            if (typ == PatternType.wheels)
                openPicker.FileTypeFilter.Add(".wheel");
            else if (typ == PatternType.ross)
                openPicker.FileTypeFilter.Add(".ross");
            else if (typ == PatternType.bazley)
                openPicker.FileTypeFilter.Add(".baz");
            else if (typ == PatternType.barrel)
                openPicker.FileTypeFilter.Add(".bar");
            else if (typ == PatternType.latticeRim)
                openPicker.FileTypeFilter.Add(".lattice");
            else if (typ == PatternType.braid)
                openPicker.FileTypeFilter.Add(".braid");
            openPicker.FileTypeFilter.Add(".xml");
            StorageFile file = await openPicker.PickSingleFileAsync();
            XmlSerializer ser;
            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                if (typ == PatternType.wheels)
                {
                    ser = new XmlSerializer(typeof(WheelsData));                
                    WheelsData wd = (WheelsData)ser.Deserialize(stream);
                    wd.PatternIndex = WheelsPatterns.Count;
                    wd.FixUp();
                    WheelsPatterns.Add(wd);
                    SelectedPathIndex = wd.PatternIndex;
                }
                else if (typ == PatternType.ross)
                {
                    ser = new XmlSerializer(typeof(RossData));
                    RossData rd = (RossData)ser.Deserialize(stream);
                    rd.PatternIndex = RossPatterns.Count;
                    RossPatterns.Add(rd);
                    SelectedPathIndex = rd.PatternIndex;

                }
                else if (typ == PatternType.bazley)
                {
                    ser = new XmlSerializer(typeof(BazelyChuck));
                    BazelyChuck bd = (BazelyChuck)ser.Deserialize(stream);
                    bd.PatternIndex = BazeleyPatterns.Count;
                    BazeleyPatterns.Add(bd);
                    SelectedPathIndex = bd.PatternIndex;
                }
                else if (typ == PatternType.barrel)
                {
                    ser = new XmlSerializer(typeof(Barrel));
                    Barrel bd = (Barrel)ser.Deserialize(stream);
                    bd.PatternIndex = BarrelPatterns.Count;
                    BarrelPatterns.Add(bd);
                    SelectedPathIndex = bd.PatternIndex;
                }
                else if (typ == PatternType.latticeRim)
                {
                    ser = new XmlSerializer(typeof(LatticeData));
                    LatticeData bd = (LatticeData)ser.Deserialize(stream);
                    if (LatticePatterns.Select(p => p.PatternIndex == bd.PatternIndex).Count() > 0)
                       bd.PatternIndex = LatticePatterns.Count + 1;

                    LatticePatterns.Add(bd);
                    SelectedPathIndex = bd.PatternIndex;
                }
                else if (typ == PatternType.braid)
                {
                    ser = new XmlSerializer(typeof(BraidData));
                    BraidData bd = (BraidData)ser.Deserialize(stream);
                    bd.PatternIndex = BraidPatterns.Count;
                    BraidPatterns.Add(bd);
                    SelectedPathIndex = bd.PatternIndex;
                }          
            }
            else
            {
                
            }
           
        }
  
        public List<List<Point>> CurrentPathAsListofPoint
        {
            get
            {
                List<List<Point>> pc = new List<List<Point>>();
                foreach (Shape s in CurrentPath.Shapes)
                {
                    if (s is Polygon)
                    {
                        List<Point> lp = (s as Polygon).Points.ToList();
                        pc.Add(lp);
                    }
                    else if (s is Windows.UI.Xaml.Shapes.Path)
                    {

                    }
                }
                return pc;
            }
        
        }

        internal async void SaveProfile()
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("profile", new List<string>() { ".prof" });
            savePicker.FileTypeChoices.Add("csv", new List<string>() { ".csv" });
            
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "new profile";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                IRandomAccessStream sessionRandomAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
                XmlSerializer serializer;
                serializer = new XmlSerializer(typeof(Profile));
                serializer.Serialize(sessionOutputStream.AsStreamForWrite(), CurrentProfile);
               
                sessionRandomAccess.Dispose();
                await sessionOutputStream.FlushAsync();
                sessionOutputStream.Dispose();
            }
        }

        internal async Task ImportProbeData()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");
            openPicker.FileTypeFilter.Add(".ngc");
            openPicker.FileTypeFilter.Add(".csv");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                IList<string> lines = await Windows.Storage.FileIO.ReadLinesAsync(file);
                CurrentProfile = ParseProbeData(lines);
            }
        }

        private Profile ParseProbeData(IList<string> lines)
        {
            PointProfile pp = new PointProfile();
            double X, Y;
            X = Y = 0;    
            foreach (string l in lines)
            {
                if (l != string.Empty)
                {
                    string[] vs = l.Split(new char[] { ',' });
                    X = double.Parse(vs[0]);
                    Y = double.Parse(vs[1]);
                    pp.Add(X, Y);
                }
            }
             
            return pp;
        }

        internal async void LoadProfile()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".profile");
            openPicker.FileTypeFilter.Add(".csv");
            StorageFile file = await openPicker.PickSingleFileAsync();
            XmlSerializer ser;
            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                ser = new XmlSerializer(typeof(Profile));
                CurrentProfile = (Profile)ser.Deserialize(stream);
            }
        }
    }
 
}

      
