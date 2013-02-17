using Geometric_Chuck.Common;
using Geometric_Chuck.Interfaces;
using Geometric_Chuck.PathGenerators;
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
using Geometric_Chuck.Spindle;
using grendgine_collada;
using Windows.UI.Xaml.Media;

namespace Geometric_Chuck
{
    /// <summary>
    /// This class is used to hold the data on all Bazeley and Ross 
    /// Defined Patterns
    /// </summary>
    public class ViewModel : BindableBase
    {
        public event EventHandler WheelsDataChanged;

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

        IPathData _lastBazelyPattern;
        public IPathData LastBazelyPattern
        {
            get { return _lastBazelyPattern; }
            set { SetProperty(ref _lastBazelyPattern, value, "LastBazelyPattern"); }
        }

        double _lastBazelyIncrement;
        public double LastBazelyIncrement
        {
            get { return _lastBazelyIncrement; }
            set { SetProperty(ref _lastBazelyIncrement, value, "LastBazelyIncrement"); }
        }

        IPathData _lastRossPattern;
        public IPathData LastRossPattern
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

        IPathData _lastWheelsPattern;
        public IPathData LastWheelsPattern
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

        IPathData _lastBarrelPattern;
        public IPathData LastBarrelPattern
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
        //TODO add Other path options to Save and restore
        void SaveCurrentPathdata()
        {
            if (_currentPathData != null)
            {
                if (_currentPathData.PathType == PatternType.BAZELEY)
                {
                    LastBazelyPattern = _currentPathData;
                    LastBazelyIncrement = Increment;
                }
                else if (_currentPathData.PathType == PatternType.ROSS)
                {
                    LastRossPattern = _currentPathData;
                    LastRossIncrement = Increment;
                }
                else if (_currentPathData.PathType == PatternType.WHEELS)
                {
                    LastWheelsPattern = _currentPathData;
                    LastWheelsIncrement = Increment;
                }
                else if (_currentPathData.PathType == PatternType.BARREL)
                {
                    LastBarrelPattern = _currentPathData;
                    LastBarrelIncrement = Increment;
                }
                else  // defaults to Bazely
                {
                    LastBazelyPattern = _currentPathData;
                    LastBazelyIncrement = Increment;
                }
            }
        }
        void RestoreLastPathdata(PatternType typ)
        {
            if (typ == PatternType.BAZELEY)
            {
                _currentPathData = (_lastBazelyPattern != null) ? _lastBazelyPattern : BazeleyPatterns[0];
                _increment = (_lastBazelyIncrement > 0) ? _lastBazelyIncrement : 0.001;
            }
            else if (typ == PatternType.ROSS)
            {
                _currentPathData = (_lastRossPattern != null) ? _lastRossPattern : RossPatterns[0];
                _increment = (_lastRossIncrement > 0) ? _lastRossIncrement : 0.0005;
            }
            else if (typ == PatternType.WHEELS)
            {
                _currentPathData = (_lastWheelsPattern != null) ? _lastWheelsPattern : null;
                _increment = (_lastWheelsIncrement > 0) ? _lastWheelsIncrement : 0.001;
            }
            else if (typ == PatternType.BARREL)
            {
                _currentPathData = (_lastBarrelPattern != null) ? _lastBarrelPattern : null;
                _increment = (_lastBarrelIncrement > 0) ? _lastBarrelIncrement : 0.001;
            }
            else  // defaults to Bazely
            {
                _currentPathData = (_lastBazelyPattern != null) ? _lastBazelyPattern : BazeleyPatterns[0];
                _increment = (_lastBazelyIncrement > 0) ? _lastBazelyIncrement : 0.001;
            }
        }

        IPathData _currentPathData;
        public IPathData CurrentPathData
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
                        if (value.PathType == PatternType.BAZELEY)
                            LastBazelyPattern = value;
                        else if (value.PathType == PatternType.ROSS)
                            LastRossPattern = value;
                        else if (value.PathType == PatternType.WHEELS)
                            LastWheelsPattern = value;
                        else if (value.PathType == PatternType.BARREL)
                            LastBarrelPattern = value;
                    }
                    if (value != null)
                    {
                        try
                        {
                            SetProperty(ref _currentPathData, value, "CurrentPathData");
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


        IPathGenerator _pathEngine;

        double _increment;
        public double Increment
        {
            get { return _increment; }
            set 
            { 
                SetProperty(ref _increment, value, "Increment");
            }
        }

        public ViewModel()
        {
            BazeleyPatterns = new ObservableCollection<BazelyChuck>();
            CreateBazelyData();
            _lastBazelyPattern = null;

            RossPatterns = new ObservableCollection<RossData>();
            _currentPath = new PolygonCollection();
            _lastRossPattern = null;
            CreateRossData();

            _lastWheelsPattern = null;
            CreateWheelsData();

            BarrelPatterns = new ObservableCollection<Barrel>();
            _lastBarrelPattern = null;
            CreateBarrelData();
        }

        private void CreateBarrelData()
        {
            Barrel b = new Barrel(0);
            b.Add(new Geometric_Chuck.Spindle.Ellipse(0.3));
            BarrelPatterns.Add(b);
            b = new Barrel(1);
            b.Add(new Geometric_Chuck.Spindle.Wave(8, 1, 0));
            BarrelPatterns.Add(b);
            b = new Barrel(2);
            b.Add(new Geometric_Chuck.Spindle.Poly(5));
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
                case PatternType.BARREL:
                    _pathEngine = new OffsetPathEngine();
                    break;
                case PatternType.BAZELEY:
                    _pathEngine = new BazelyEngine();
                    break;
                case PatternType.ROSS:
                    _pathEngine = new RossEngine();
                    break;
                case PatternType.WHEELS:
                    _pathEngine = new WheelsEngine();
                    break;
            }
        }

        public void SetupPattern(PatternType ptyp, IPathData d, double inc)
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
            set { SetProperty(ref _numPoints, value, "NumPoints"); }
        }

        public void AddPath(Windows.UI.Xaml.Shapes.Polygon poly)
        {
            _currentPath.AddPoly(poly);
            SelectedPathIndex = _currentPath.IndexOf(poly);
        }

        PolygonCollection _currentPath;
        public PolygonCollection CurrentPath
        {
            get { return _currentPath; }
            set
            {
                SetProperty(ref _currentPath, value, "CurrentPath");
            }
        }

        protected int _selectedPathIndex;
        public int SelectedPathIndex
        {
            get { return _selectedPathIndex; }
            set { SetProperty(ref _selectedPathIndex, value, "SelectedRossPathIndex"); }

        }
        public int SelectedPathSize
        {
            get
            {
                int indx = 0;
                if ((_currentPath != null && SelectedPathIndex >= 0))
                {
                    indx = _currentPath[SelectedPathIndex].Points.Count;
                }

                return indx;
            }
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
            CurrentPath = _pathEngine.CreatePaths(_currentPathData, Increment);
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
            _lastBazelyPattern = _BazelyPatterns[0];
        }

        void CreateWheelsData()
        {
            WheelsPatterns = new ObservableCollection<WheelsData>();
            
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
            if (CurrentPathData.PathType == PatternType.WHEELS)
                savePicker.FileTypeChoices.Add("Wheel",new List<string>() {".wheel"});
            else if (CurrentPathData.PathType == PatternType.ROSS)
                savePicker.FileTypeChoices.Add("Ross", new List<string>() { ".ross"});
            else if (CurrentPathData.PathType == PatternType.BAZELEY)
                savePicker.FileTypeChoices.Add("Bazely", new List<string>() { ".baz"});
            else if (CurrentPathData.PathType == PatternType.BARREL)
                savePicker.FileTypeChoices.Add("Barrel", new List<string>() { ".bar"});
            //savePicker.FileTypeChoices.Add("Xml", new List<string>() { ".xml" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = _currentPathData.Name;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                IRandomAccessStream sessionRandomAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
                XmlSerializer serializer ;
                if (CurrentPathData.PathType == PatternType.BAZELEY)
                {
                    serializer = new XmlSerializer(typeof(BazelyChuck));
                    serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as BazelyChuck));
                }
                else if (CurrentPathData.PathType == PatternType.ROSS)
                {
                    serializer = new XmlSerializer(typeof(RossData));
                    serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as RossData));
                }
                else if (CurrentPathData.PathType == PatternType.WHEELS)
                {
                    serializer = new XmlSerializer(typeof(WheelsData));
                    serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as WheelsData));
                }
                else if (CurrentPathData.PathType == PatternType.BARREL)
                {
                    serializer = new XmlSerializer(typeof(Barrel));
                    serializer.Serialize(sessionOutputStream.AsStreamForWrite(), (CurrentPathData as Barrel));
                }
                sessionRandomAccess.Dispose();
                await sessionOutputStream.FlushAsync();
                sessionOutputStream.Dispose();
            }
        }

        public async void ImportPattern(PatternType typ)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            if (typ == PatternType.WHEELS)
                openPicker.FileTypeFilter.Add(".wheel");
            else if (typ == PatternType.ROSS)
                openPicker.FileTypeFilter.Add(".ross");
            else if (typ == PatternType.BAZELEY)
                openPicker.FileTypeFilter.Add(".baz");
            else if (typ == PatternType.BARREL)
                openPicker.FileTypeFilter.Add(".bar");
            openPicker.FileTypeFilter.Add(".xml");
            StorageFile file = await openPicker.PickSingleFileAsync();
            XmlSerializer ser;
            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                if (typ == PatternType.WHEELS)
                {
                    ser = new XmlSerializer(typeof(WheelsData));                
                    WheelsData wd = (WheelsData)ser.Deserialize(stream);
                    wd.PatternIndex = WheelsPatterns.Count;
                    wd.FixUp();
                    WheelsPatterns.Add(wd);
                    SelectedPathIndex = wd.PatternIndex;
                }
                else if (typ == PatternType.ROSS)
                {
                    ser = new XmlSerializer(typeof(RossData));
                    RossData rd = (RossData)ser.Deserialize(stream);
                    rd.PatternIndex = RossPatterns.Count;
                    RossPatterns.Add(rd);
                    SelectedPathIndex = rd.PatternIndex;

                }
                else if (typ == PatternType.BAZELEY)
                {
                    ser = new XmlSerializer(typeof(BazelyChuck));
                    BazelyChuck bd = (BazelyChuck)ser.Deserialize(stream);
                    bd.PatternIndex = BazeleyPatterns.Count;
                    BazeleyPatterns.Add(bd);
                    SelectedPathIndex = bd.PatternIndex;
                }
                else if (typ == PatternType.BARREL)
                {
                    ser = new XmlSerializer(typeof(Barrel));
                    Barrel bd = (Barrel)ser.Deserialize(stream);
                    bd.PatternIndex = BarrelPatterns.Count;
                    BarrelPatterns.Add(bd);
                    SelectedPathIndex = bd.PatternIndex;
                }          
            }
            else
            {
                
            }
           
        }

        public void WriteColladaFile()
        {
            SimpleColladaGenerator gen = new SimpleColladaGenerator();
            SimpleCollada.Save_File(gen.Create(CurrentPath));
        }



        public List<List<Point>> CurrentPathAsListofPoint
        {
            get
            {
                List<List<Point>> pc = new List<List<Point>>();
                foreach (Polygon poly in CurrentPath.Polygons)
                {
                    List<Point> lp = poly.Points.ToList<Point>();
                    pc.Add(lp);
                }
                return pc;
            }
        
        }
    }
 
}

      
