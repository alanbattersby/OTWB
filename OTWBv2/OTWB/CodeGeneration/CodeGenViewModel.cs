using Geometric_Chuck.Common;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using System.Diagnostics;
using OTWB.Coordinates;
using OTWB.Settings;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Geometric_Chuck;

namespace OTWB.CodeGeneration
{
    public class CodeGenViewModel : BindableBase
    {
        List<List<Point>> _currentPath;
        CodeGenTemplates _templates;

        public CodeGenTemplates Templates
        {
            get { return _templates; }
            set { SetProperty(ref _templates, value); }
        }

        string _pathName;
        public string PathName
        {
            get { return _pathName; }
            set { SetProperty(ref _pathName, value); }
        }

        public int DP
        {
            get { return (int)BasicLib.GetSetting(SettingsNames.DECIMAL_PLACES); }
        }
        public double FeedRate
        {
            get {return (double)BasicLib.GetSetting(SettingsNames.FEED_RATE_VALUE);   }
        }
        public double SafeHeight
        {
            get { return (double)BasicLib.GetSetting(SettingsNames.SAFE_HEIGHT); }
        }
        public double CuttingDepth
        {
            get { return (double)BasicLib.GetSetting(SettingsNames.CUTTING_DEPTH); }
        }
  
        
        ObservableCollection<GcodeFile> _code;
        public ObservableCollection<GcodeFile> Code
        {
            get { return _code; }
            set 
            { 
                SetProperty(ref _code, value);
                _code.CollectionChanged += _code_CollectionChanged;
            }
        }

        void _code_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged();
        }

        public CodeGenViewModel()
        {
            PathName = "No Path";
            Code = new ObservableCollection<GcodeFile>();
            _templates = new CodeGenTemplates();
        }
        /// <summary>
        /// Template used to generate code for each point on path
        /// </summary>
        public bool UseRotaryTable
        {
            get
            {
                return (bool)BasicLib.GetSetting(SettingsNames.USE_ROTARY_TABLE);
            }
            set { BasicLib.SetSetting(SettingsNames.USE_ROTARY_TABLE, value); }
        }
        public bool UseAbsoluteMoves
        {
            get
            {
                return (bool)BasicLib.GetSetting(SettingsNames.USE_ABSOLUTE_MOVES);
            }
            set { BasicLib.SetSetting(SettingsNames.USE_ABSOLUTE_MOVES, value); }
        }
        public bool UseSubroutine
        {
            get
            {
                return (bool)BasicLib.GetSetting(SettingsNames.USE_SUBROUTINE);
            }
            set { BasicLib.SetSetting(SettingsNames.USE_SUBROUTINE, value); }
        }
        public bool UseSingleFile
        {
            get
            {
                return (bool)BasicLib.GetSetting(SettingsNames.USE_SINGLE_FILE);
            }
            set { BasicLib.SetSetting(SettingsNames.USE_SINGLE_FILE, value); }
        }

        public int _currentPathIndex;
        public int CurrentPathIndex
        {
            get { return _currentPathIndex; }
            set { SetProperty(ref _currentPathIndex, value); }
        }
        public ICoordinate _currentPoint;
        public ICoordinate CurrentPoint
        {
            get { return _currentPoint; }
            set { SetProperty(ref _currentPoint, value); }
        }

        public List<List<Point>> CurrentPath
        {
            get { return _currentPath; }
            set
            {
                SetProperty(ref _currentPath, value, "CurrentPath");
            }
        }

        private List<ICoordinate> OffsetList(List<Point> p)
        {
            List<ICoordinate> offsets = new List<ICoordinate>();
            for (int indx = 1; indx < p.Count; indx++)
            {
                Cartesian pnt = new Cartesian(p[indx].X - p[indx - 1].X,
                                      p[indx].Y - p[indx - 1].Y, 0);
                offsets.Add(pnt);
            }
            return offsets;
        }
       

        private List<ICoordinate> OffsetList(List<ICoordinate> p)
        {
            List<ICoordinate> offsets = new List<ICoordinate>();
            for (int indx = 1; indx < p.Count; indx++)
            {
                ICoordinate pnt = null;
                if (p[indx] is Cartesian)
                {
                    Cartesian pc = p[indx] as Cartesian;
                    Cartesian pc1 = p[indx - 1] as Cartesian;
                    pnt = new Cartesian(pc.X - pc1.X,
                                         pc.Y - pc1.Y,
                                         pc.Z - pc1.Z);
                }
                else if (p[indx] is Cylindrical)
                {
                    Cylindrical pc = p[indx] as Cylindrical;
                    Cylindrical pc1 = p[indx - 1] as Cylindrical;
                    pnt = new Cylindrical(pc.Radius - pc1.Radius,
                                         pc.Angle - pc1.Angle,
                                         pc.Depth - pc1.Depth);
                }
                else if (p[indx] is Spherical)
                {
                    Spherical pc = p[indx] as Spherical;
                    Spherical pc1 = p[indx - 1] as Spherical;
                    pnt = new Spherical(pc.Radius - pc1.Radius,
                                         pc.CoLattitude - pc1.CoLattitude,
                                         pc.Depth - pc1.Depth);
                }
                offsets.Add(pnt);
            }
            return offsets;
        }
        public List<List<ICoordinate>> CreateOffsetList
        {
            get
            {
                List<List<ICoordinate>> newpoints = new List<List<ICoordinate>>();
                foreach (List<Point> p in _currentPath)
                {
                    newpoints.Add(OffsetList(p));
                }
                return newpoints;
            }
        }
        public List<List<ICoordinate>> CreateCylindricalList
        {
            get
            {
                
                List<List<ICoordinate>> c = new List<List<ICoordinate>>();
                foreach (List<Point> lp in CurrentPath)
                {
                    List<ICoordinate> lc = new List<ICoordinate>();
                    int winding = 0;
                    double lastAngle = 0;
                    foreach (Point p in lp)
                    {
                        Cylindrical cp = new Cylindrical(p);
                        if (BasicLib.Quadrant3To0(lastAngle, cp.Angle))
                            winding += 1;
                        else if (BasicLib.Quadrant0To3(lastAngle, cp.Angle))
                            winding -= 1;
                        cp.Angle += winding * 360;
                        lc.Add(cp);
                        lastAngle = cp.Angle;
                    }
                    c.Add(lc);
                }
                return c;
            }
        }
        public List<List<ICoordinate>> CreateCylindricalOffsetList
        {
            get
            {
                List<List<ICoordinate>> col = new List<List<ICoordinate>>();
                foreach (List<ICoordinate> p in CreateCylindricalList)
                {
                    col.Add(OffsetList(p));
                }
                return col;
            }
        }

        private List<ICoordinate> MapToCartesianOffsetList(List<Point> pl)
        {
            List<ICoordinate> p = MapToCartesianList(pl);
            return OffsetList(p);
        }
        private List<ICoordinate> MapToCylindricalOffsetList(List<Point> pl)
        {
            List<ICoordinate> p = MapToCylindricalList(pl);
            return OffsetList(p);
        }

        private List<ICoordinate> MapToCartesianList(List<Point> pl)
        {
            List<ICoordinate> lc = new List<ICoordinate>();
            foreach (Point p in pl)
            {
                lc.Add(new Cartesian(p.X, p.Y, 0));
            }
            return lc;
        }
        private List<ICoordinate> MapToCylindricalList(List<Point> pl)
        {
            List<ICoordinate> lc = new List<ICoordinate>();
            int winding = 0;
            double lastAngle = 0;
            foreach (Point p in pl)
            {
                Cylindrical cp = new Cylindrical(p);
                if (BasicLib.Quadrant3To0(lastAngle, cp.Angle))
                    winding += 1;
                else if (BasicLib.Quadrant0To3(lastAngle, cp.Angle))
                    winding -= 1;
                cp.Angle += winding * 360;
                lc.Add(cp);
                lastAngle = cp.Angle;
            }
            return lc;
        }

        public async void  ImportPath()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".xml");
            StorageFile file = await openPicker.PickSingleFileAsync();
            XmlSerializer ser;
            if (file != null)
            {
                try
                {
                    var stream = await file.OpenStreamForReadAsync();

                    ser = new XmlSerializer(typeof(List<List<Point>>));
                    CurrentPath = (List<List<Point>>)ser.Deserialize(stream);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message); /// break here
                }
                PathName = file.Name;
            }
        }

        private BindableCodeTemplate PathStartTemplate()
        {
            if (this.UseSubroutine)
            {
                // generate sub header
                return _templates.SubStartTemplate;
            }
            else
            {
                return _templates.PathStartTemplate;
            }
        }
        private BindableCodeTemplate PathEndTemplate()
        {
            if (this.UseSubroutine)
            {
               return _templates.SubEndTemplate;
            }
            else
            {
                return _templates.PathEndTemplate;
            }
        }

        public void GenerateCode()
        {
            if (CurrentPath == null) return;
            CodeGenDataContext cntxt = new CodeGenDataContext(this);
            Code.Clear();

            if (UseSingleFile)
                GenerateCodeInSingleFile(cntxt);
            else
                GenerateCodeInMultipleFiles(cntxt);
        }

        void GenerateCodeInMultipleFiles(CodeGenDataContext cntxt)
        {
            // first generate subroutines or paths
            // then main program to call them
            GenerateSubroutines(cntxt);
            GenerateMain(cntxt);
        }

        private void GenerateMain(CodeGenDataContext cntxt)
        {
            StringBuilder sb = new StringBuilder();
            Bind(ref sb, Templates.Header_Template, cntxt);
            //BindableCodeTemplate tmpl = Templates.Header_Template;
            //tmpl.DataContext = cntxt;
            //sb.AppendLine(tmpl.Text);
            Bind(ref sb, Templates.Globals_Template, cntxt);
            //tmpl = Templates.Globals_Template;
            //tmpl.DataContext = cntxt;
            //sb.AppendLine(tmpl.Text);

            Bind(ref sb, Templates.MainProgramTemplate, cntxt);
            //tmpl = _templates.MainProgramTemplate;
            //tmpl.DataContext = cntxt;
            //sb.AppendLine(tmpl.Text);
            Bind(ref sb, Templates.ProgramEndTemplate, cntxt);

            BindableCodeTemplate tmpl = _templates.MainFilenameTemplate;
            tmpl.DataContext = cntxt;
            Code.Add(new GcodeFile(tmpl.Text, sb.ToString()));
        }

        /// <summary>
        /// Subroutines each have an individual header file
        /// </summary>
        /// <param name="cntxt"></param>
        void GenerateSubroutines(CodeGenDataContext cntxt)
        {
            foreach (List<Point> pl in CurrentPath)
            {
                CurrentPathIndex = CurrentPath.IndexOf(pl);
                BindableCodeTemplate tmpl;
                StringBuilder sb = new StringBuilder();
                Bind(ref sb, Templates.Header_Template, cntxt);
                //BindableCodeTemplate tmpl = Templates.Header_Template;
                //tmpl.DataContext = cntxt;
                //sb.AppendLine(tmpl.Text);
                
                // add start of path 
                Bind(ref sb, PathStartTemplate(), cntxt);
                //tmpl = PathStartTemplate();
                //tmpl.DataContext = cntxt;
                //sb.AppendLine(tmpl.Text);
                // now generate points in path
                List<ICoordinate> points = (this.UseRotaryTable)
                    ? MapToCylindricalList(pl) : MapToCartesianList(pl);
                if (!this.UseAbsoluteMoves)
                    points = OffsetList(points);
                // loop round points in list
                foreach (ICoordinate coord in points)
                {
                    CurrentPoint = coord;
                    tmpl = (coord is Cartesian)
                         ?_templates.XY_Point_Template
                         :_templates.RA_Point_Template;

                    Bind(ref sb, tmpl, cntxt);
                    //tmpl.DataContext = cntxt;
                    //sb.AppendLine(tmpl.Text); 
                }
                // add end of path
                Bind(ref sb, PathEndTemplate(), cntxt);
                //tmpl = PathEndTemplate();
                //tmpl.DataContext = cntxt;
                //sb.AppendLine(tmpl.Text);

                // now deal with name
                tmpl = _templates.SubFilenameTemplate;
                tmpl.DataContext = cntxt;
                Code.Add(new GcodeFile(tmpl.Text,sb.ToString()));
            }
        }

        void Bind(ref StringBuilder sb, BindableCodeTemplate t, CodeGenDataContext cntxt)
        {
            if (t.Include)
            {
                t.DataContext = cntxt;
                sb.AppendLine(t.Text);
            }
        }

        void GenerateCodeInSingleFile(CodeGenDataContext cntxt)
        {
            BindableCodeTemplate tmpl;
            StringBuilder sb = new StringBuilder();
            Bind(ref sb, Templates.Header_Template, cntxt);
            Bind(ref sb, Templates.Globals_Template, cntxt);
            //BindableCodeTemplate tmpl = Templates.Header_Template;
            //tmpl.DataContext = cntxt;
            //sb.AppendLine(tmpl.Text);

            //BindableCodeTemplate tmpl = Templates.Globals_Template;
            //tmpl.DataContext = cntxt;
            //sb.AppendLine(tmpl.Text);

            foreach (List<Point> pl in CurrentPath)
            {
                // add start of path 
                Bind(ref sb, PathStartTemplate(), cntxt);
                //BindableCodeTemplate tmpl = PathStartTemplate();
                //tmpl.DataContext = cntxt;
                //sb.AppendLine(tmpl.Text);
                // now generate points in path
                List<ICoordinate> points = (this.UseRotaryTable)
                    ? MapToCylindricalList(pl) : MapToCartesianList(pl);
                if (!this.UseAbsoluteMoves)
                    points = OffsetList(points);
                // loop round points in list
                foreach (ICoordinate coord in points)
                {
                    CurrentPoint = coord;
                    tmpl = (coord is Cartesian)
                         ? _templates.XY_Point_Template
                         : _templates.RA_Point_Template;

                    Bind(ref sb, tmpl, cntxt);
                    //tmpl.DataContext = cntxt;
                    //sb.AppendLine(tmpl.Text);
                }
                // add end of path
                Bind(ref sb, PathEndTemplate(), cntxt);
                //tmpl = PathEndTemplate();
                //tmpl.DataContext = cntxt;
                //sb.AppendLine(tmpl.Text);
            }

            Bind(ref sb, Templates.MainProgramTemplate, cntxt);
            //tmpl = _templates.MainProgramTemplate;
            //tmpl.DataContext = cntxt;
            //sb.AppendLine(tmpl.Text);
            Bind(ref sb, Templates.ProgramEndTemplate, cntxt);
            tmpl = _templates.MainFilenameTemplate;
            tmpl.DataContext = cntxt;
            // now add main program
            Code.Add(new GcodeFile(tmpl.Text,sb.ToString()));
        }

        internal void Clear()
        {
            Code.Clear();
        }

        internal async void SaveCode()
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add(".");
            folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            foreach (GcodeFile f in Code)
            {
                try
                {

                    StorageFile file = await folder.CreateFileAsync(f.FileName, CreationCollisionOption.ReplaceExisting);
                    if (file != null)
                    {
                        await Windows.Storage.FileIO.WriteTextAsync(file, f.Code);
                    }
                }
                catch (Exception e)
                {
                    CoreWindowDialog dlg = new CoreWindowDialog(e.Message);
                    dlg.ShowAsync();
                }
            }
        }

        public async void SaveTemplateCollection()
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Template", new List<string>() { ".ctmpl" });
            savePicker.FileTypeChoices.Add("Xml", new List<string>() { ".xml" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Template Collection";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                IRandomAccessStream sessionRandomAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
               
                var serializer = new XmlSerializer(typeof(CodeGenTemplates));
                serializer.Serialize(sessionOutputStream.AsStreamForWrite(),Templates);
                sessionRandomAccess.Dispose();
                await sessionOutputStream.FlushAsync();
                sessionOutputStream.Dispose();
            }      

        }

        public async void LoadTemplates()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".ctpl");
            openPicker.FileTypeFilter.Add(".xml");
            StorageFile file = await openPicker.PickSingleFileAsync();
            XmlSerializer ser;
            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                ser = new XmlSerializer(typeof(CodeGenTemplates));
                Templates = (CodeGenTemplates)ser.Deserialize(stream);
                GenerateCode();
            }
        }
    }
}
