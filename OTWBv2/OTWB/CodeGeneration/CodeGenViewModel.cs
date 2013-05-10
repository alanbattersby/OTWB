using OTWB.Common;
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
using OTWB;
using OTWB.PathGenerators;
using Windows.UI.Popups;
using Windows.UI.Xaml.Shapes;
using OTWB.Collections;
using OTWB.Interfaces;
using OTWB.Profiles;

namespace OTWB.CodeGeneration
{
    public class CodeGenViewModel : BindableBase
    {
        List<List<Point>> _currentPath;
        ShapeCollection _currentToolPaths;
        CodeGenDataContext _context;
        Profile _profile;

        string _pathName;
        public string PathName
        {
            get { return _pathName; }
            set { SetProperty(ref _pathName, value); }
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
            _context = App.CodeSettingsContext;
        }
       
        public int _currentPathIndex;
        public int CurrentPathIndex
        {
            get { return _currentPathIndex; }
            set { SetProperty(ref _currentPathIndex, value); }
        }
        public Cartesian _currentPoint;
        public Cartesian CurrentPoint
        {
            get { return _currentPoint; }
            set { SetProperty(ref _currentPoint, value); }
        }

        public ShapeCollection ToolPaths
        {
            get { return _currentToolPaths; }
            set { SetProperty(ref _currentToolPaths, value); }
        }

        public List<List<Point>> CurrentPath
        {
            get { return _currentPath; }
            set { SetProperty(ref _currentPath, value);}
        }

        public Profile Profile
        {
            get { return _profile; }
            set { SetProperty(ref _profile, value); }
        }

        private PathFragment OffsetList(PathFragment p)
        {
            PathFragment offsets = new PathFragment();
            for (int indx = 1; indx < p.Count; indx++)
            {
                Cartesian pc = p[indx] as Cartesian;
                Cartesian pc1 = p[indx - 1] as Cartesian;
                Cartesian pnt = new Cartesian(pc.X - pc1.X,
                                         pc.Y - pc1.Y,
                                         pc.Z - pc1.Z);
               offsets.Add(pnt);
            }
            return offsets;
        }
     
        public async Task  ImportPath()
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
                    _currentPath = (List<List<Point>>)ser.Deserialize(stream);
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
            if (_context.UseSubroutine)
                return _context.Templates.SubStartTemplate;
            else
                 return _context.Templates.PathStartTemplate;
        }
        private BindableCodeTemplate PathEndTemplate()
        {
            if (_context.UseSubroutine)
                return _context.Templates.SubEndTemplate;
            else
                return _context.Templates.PathEndTemplate;
        }

        public void GenerateCode()
        {
            if (ToolPaths == null) return;
            _context.codegenViewmodel = this;
            Code.Clear();

            if (_context.UseSingleFile)
                GenerateCodeInSingleFile();
            else
                GenerateCodeInMultipleFiles();
        }

        void GenerateCodeInMultipleFiles()
        {
            // first generate subroutines or paths
            // then main program to call them
            StringBuilder sb = new StringBuilder();
            foreach (Shape s in _currentToolPaths.Shapes)
            {
                if (s is Polygon)
                    GenerateSubFromPolygon(ref sb, (s as Polygon));
                else if (s is Windows.UI.Xaml.Shapes.Path)
                    GenerateSubFromPath(ref sb, (s as Windows.UI.Xaml.Shapes.Path));

                string filename = string.Empty;
                string ext = (string)BasicLib.GetSetting(SettingsNames.GCODE_FILE_EXT);
                // now deal with name and save in code list
                if ((bool)BasicLib.GetSetting(SettingsNames.USE_NAMED_O_WORDS))
                {
                    BindableCodeTemplate tmpl = _context.Templates.SubFilenameTemplate;
                    tmpl.DataContext = _context;
                    filename = System.IO.Path.ChangeExtension(tmpl.Text,ext );
                }
                else
                {
                    filename = System.IO.Path.ChangeExtension(_context.SubPathName.ToString(), ext);
                }
                Code.Add(new GcodeFile(filename, sb.ToString()));
            }
                
            GenerateMain();
        }

        private void GenerateMain()
        {
            StringBuilder sb = new StringBuilder();
            Bind(ref sb, _context.Templates.Header_Template);
            Bind(ref sb, _context.Templates.Globals_Template);
            Bind(ref sb, _context.Templates.MainProgramTemplate);
            foreach (Shape s in _currentToolPaths.Shapes)
            {
                   Bind(ref sb,_context.Templates.SubCallTemplate);
            }
            Bind(ref sb, _context.Templates.ProgramEndTemplate);

            string ext = (string)BasicLib.GetSetting(SettingsNames.GCODE_FILE_EXT);
            BindableCodeTemplate tmpl = _context.Templates.MainFilenameTemplate;
            tmpl.DataContext = _context;
            Code.Add(new GcodeFile(System.IO.Path.ChangeExtension(tmpl.Text, ext), sb.ToString()));
        }

        void GenerateSubFromPolygon(ref StringBuilder sb, Polygon poly)
        {
            BindableCodeTemplate tmpl;
            CurrentPathIndex = (int)poly.Tag;

            // add start of path 
            Bind(ref sb, PathStartTemplate());
            PathFragment points = new PathFragment(poly.Points);

            if (!_context.UseAbsoluteMoves)
                points = OffsetList(points);
            // loop round points in list
            _currentPoint =  _context.FirstPoint = points[0];
           
            tmpl = _context.Templates.FirstPointTemplate;
            Bind(ref sb, tmpl);

            for (int i = 1; i < points.Count - 1 ; i++)
            {
                _currentPoint = points[i];
                tmpl = (_context.UseRotaryTable)
                     ? _context.Templates.RA_Point_Template
                     : _context.Templates.XY_Point_Template;
                Bind(ref sb, tmpl);
            }
            _currentPoint =  _context.LastPoint = points[points.Count - 1];
            tmpl = _context.Templates.LastPointTemplate;
            Bind(ref sb, tmpl);
            // add end of path
            Bind(ref sb, PathEndTemplate());
        }

        void GenerateSubFromPath(ref StringBuilder sb, Windows.UI.Xaml.Shapes.Path path)
        {
            CurrentPathIndex = (int)path.Tag;
            Bind(ref sb, PathStartTemplate());
            PathFragment points = new PathFragment();
            if (path.Data is GeometryGroup)
            {
                GeometryGroup gg = path.Data as GeometryGroup;
                foreach (PathGeometry pg in gg.Children)
                {
                    foreach (PathFigure pf in pg.Figures)
                    {
                                            
                        // now deal with all lines in this figure
                        foreach (PathSegment ps in pf.Segments)
                        { 
                            if (ps is LineSegment)
                            {
                                _currentPoint = _context.FirstPoint = new Cartesian(pf.StartPoint);
                                Bind(ref sb, _context.Templates.FirstPointTemplate);
                                LineSegment ls = ps as LineSegment;
                                _currentPoint = _context.LastPoint = new Cartesian(ls.Point);
                                BindableCodeTemplate tmpl = (_context.UseRotaryTable)
                                     ? _context.Templates.RA_Point_Template
                                     : _context.Templates.XY_Point_Template;
                                Bind(ref sb, tmpl);
                                Bind(ref sb, _context.Templates.LastPointTemplate);
                            }
                            if (ps is PolyLineSegment)
                            {
                                PolyLineSegment pls = ps as PolyLineSegment;
                                BindableCodeTemplate tmpl = (_context.UseRotaryTable)
                                        ? _context.Templates.RA_Point_Template
                                        : _context.Templates.XY_Point_Template;

                                _context.FirstPoint = new Cartesian(pls.Points.First());
                                _context.LastPoint = new Cartesian(pls.Points.Last());
                                foreach (Point p in pls.Points)
                                {
                                    _currentPoint = new Cartesian(p);
                                    if (_currentPoint == _context.FirstPoint)
                                    {
                                         Bind(ref sb, _context.Templates.FirstPointTemplate);
                                    }
                                    else if (_currentPoint == _context.LastPoint)
                                    {
                                        Bind(ref sb, _context.Templates.LastPointTemplate);
                                    }
                                    else
                                        Bind(ref sb, tmpl);
                                }
                               
                            }
                            Bind(ref sb, PathEndTemplate());
                        }
                    }
                }
            }
        }
  
        
        /// <summary>
        /// Subroutines each have an individual header file
        /// </summary>
        /// <param name="cntxt"></param>
        void GenerateSubroutines()
        {
            foreach (List<Point> pl in CurrentPath)
            {
                CurrentPathIndex = CurrentPath.IndexOf(pl);
                BindableCodeTemplate tmpl;
                StringBuilder sb = new StringBuilder();
                Bind(ref sb, _context.Templates.Header_Template);
               
                // add start of path 
                Bind(ref sb, PathStartTemplate());
              
                // now generate points in path
                PathFragment points = new PathFragment(pl);
                    //(_context.UseRotaryTable)
                    //? MapToCylindricalList(pl) : MapToCartesianList(pl);
                if (!_context.UseAbsoluteMoves)
                    points = OffsetList(points);
                // loop round points in list
                foreach (Cartesian coord in points)
                {
                    CurrentPoint = coord;
                    tmpl = (coord is Cartesian)
                         ?_context.Templates.XY_Point_Template
                         :_context.Templates.RA_Point_Template;

                    Bind(ref sb, tmpl);
                   
                }
                // add end of path
                Bind(ref sb, PathEndTemplate());
              
                // now deal with name
                tmpl = _context.Templates.SubFilenameTemplate;
                tmpl.DataContext = _context;
                Code.Add(new GcodeFile(tmpl.Text,sb.ToString()));
            }
        }

        void Bind(ref StringBuilder sb, BindableCodeTemplate t)
        {
            if (t.Include)
            {
                t.DataContext = _context;
                sb.AppendLine(t.Text);
            }
        }

        void GenerateCodeInSingleFile()
        {
            BindableCodeTemplate tmpl;
            StringBuilder sb = new StringBuilder();
            Bind(ref sb, _context.Templates.Header_Template);
            Bind(ref sb, _context.Templates.Globals_Template);

            foreach (Shape s in _currentToolPaths.Shapes)
            {
                if (s is Polygon)
                    GenerateSubFromPolygon(ref sb, (s as Polygon));
                else if (s is Windows.UI.Xaml.Shapes.Path)
                    GenerateSubFromPath(ref sb, (s as Windows.UI.Xaml.Shapes.Path));
            }

            Bind(ref sb, _context.Templates.MainProgramTemplate);
            foreach (Shape s in _currentToolPaths.Shapes)
            {
                Bind(ref sb, _context.Templates.SubCallTemplate);
            }
            Bind(ref sb, _context.Templates.ProgramEndTemplate);
            tmpl = _context.Templates.MainFilenameTemplate;
            tmpl.DataContext = _context;
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
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            string ext = (string)BasicLib.GetSetting(SettingsNames.GCODE_FILE_EXT);
            foreach (GcodeFile f in Code)
            {
                try
                {
                    if (System.IO.Path.GetExtension(f.FileName) != ext)
                        f.FileName = System.IO.Path.ChangeExtension(f.FileName, ext);

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



        public double ProfileHeight 
        { 
            get
            {
                if (Profile == null)
                    return 0;
                else
                    return Profile.Height(CurrentPoint);
            }
        }
    }
}
