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

        string _footerFile;
        public string FooterFile
        {
            get { return _footerFile; }
            set { SetProperty(ref _footerFile, value); }
        }

        string _pathName;
        public string PathName
        {
            get { return _pathName; }
            set { SetProperty(ref _pathName, value); }
        }

        int DP;
        double FeedRate;

        ObservableCollection<string> _code;
        public ObservableCollection<string> Code
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
            Code = new ObservableCollection<string>();
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

        public List<List<Point>> CurrentPath
        {
            get { return _currentPath; }
            set
            {
                SetProperty(ref _currentPath, value, "CurrentPath");
            }
        }
        private List<Point> OffsetList(List<Point> p)
        {
            List<Point> offsets = new List<Point>();
            for (int indx = 1; indx < p.Count; indx++)
            {
                Point pnt = new Point(p[indx].X - p[indx - 1].X,
                                      p[indx].Y - p[indx - 1].Y);
                offsets.Add(pnt);
            }
            return offsets;
        }
        private List<Cylindrical> OffsetList(List<Cylindrical> p)
        {
            List<Cylindrical> offsets = new List<Cylindrical>();
            for (int indx = 1; indx < p.Count; indx++)
            {
                Cylindrical pnt = new Cylindrical(p[indx].Radius - p[indx - 1].Radius,
                                      p[indx].Angle - p[indx - 1].Angle,
                                      p[indx].Depth - p[indx-1].Depth);
                offsets.Add(pnt);
            }
            return offsets;
        }

        public List<List<Point>> CreateOffsetList
        {
            get
            {
                List<List<Point>> newpoints = new List<List<Point>>();
                foreach (List<Point> p in _currentPath)
                {
                    newpoints.Add(OffsetList(p));
                }
                return newpoints;
            }
        }
        public List<List<Cylindrical>> CreateCylindricalList
        {
            get
            {
                
                List<List<Cylindrical>> c = new List<List<Cylindrical>>();
                foreach (List<Point> lp in CurrentPath)
                {
                    List<Cylindrical> lc = new List<Cylindrical>();
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
        public List<List<Cylindrical>> CreateCylindricalOffsetList
        {
            get
            {
                List<List<Cylindrical>> col = new List<List<Cylindrical>>();
                foreach (List<Cylindrical> p in CreateCylindricalList)
                {
                    col.Add(OffsetList(p));
                }
                return col;
            }
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

        private void PathStart(ref StringBuilder sb, string name)
        {
            if (this.UseSubroutine)
            {
                // generate sub header
                sb.AppendLine(string.Format(_templates.SubStartTemplate.SingleLine, name));
            }
            else
            {
                sb.AppendLine(string.Format(_templates.PathStartTemplate.SingleLine, name));
            }
        }
        private void PathEnd(ref StringBuilder sb, string name)
        {
            if (this.UseSubroutine)
            {
                // generate sub header
                sb.AppendLine(string.Format(_templates.SubEndTemplate.SingleLine, name));
            }
            else
            {
                sb.AppendLine(string.Format(_templates.PathEndTemplate.SingleLine, name));
            }
        }
        private void Header(ref StringBuilder sb)
        {
            sb.AppendLine("(    Gcode Program                   )");
            sb.AppendLine("( BY:    OTWB                        )");
                sb.Append("( ON:    ").Append(DateTime.Now.ToString()).AppendLine(")");
            sb.AppendLine("( Flags:                             )");
                sb.Append("(    Rotary      : ").Append(UseRotaryTable).AppendLine("  )");
                sb.Append("(    Absolute    : ").Append(UseAbsoluteMoves).AppendLine("  )");
                sb.Append("(    Subroutine  : ").Append(UseSubroutine).AppendLine("  )");
                sb.AppendLine("(********************************)");
        }
        private void Footer(ref StringBuilder sb)
        {
            if ((_footerFile == null) || (_footerFile == string.Empty))
            {
                if (!UseSubroutine)
                    sb.AppendLine(string.Format(_templates.ProgramEndTemplate.SingleLine,
                                                _templates.ProgramEndComment.SingleLine));
                else
                    sb.AppendLine(_templates.SubEndComment.SingleLine);
            }
           
        }

        private string Instantiate(string tmplt)
        {
            string result = new string(tmplt.ToCharArray());

            if (result.Contains("{FirstPoint}"))
            {
                Point fp = CurrentPath[0][0];
                result = result.Replace("{FirstPoint}",
                            string.Format(_templates.Point_Template.SingleLine,
                                            Math.Round(fp.X, DP),
                                            Math.Round(fp.Y, DP)));
            }
            
            if (result.Contains("{Feedrate}"))
            {
                result = result.Replace("{Feedrate}",
                            string.Format(_templates.FeedRateTemplate.SingleLine, FeedRate));
            }

            if (result.Contains("{Pathname}"))
            {
                result = result.Replace("{Pathname}",
                            string.Format(_templates.PathNameTemplate.SingleLine, PathName, 0));
            }
          
            return result;
        }

        public void GenerateCode()
        {
            if (CurrentPath == null) return;
            Code.Clear();
            DP = (int)BasicLib.GetSetting(SettingsNames.DECIMAL_PLACES);
            FeedRate = (double)BasicLib.GetSetting(SettingsNames.FEED_RATE_VALUE);

            string gcode = ((bool)BasicLib.GetSetting(SettingsNames.USE_ABSOLUTE_MOVES)
                         ? _templates.AbsoluteModeTemplate.SingleLine
                         : _templates.RelativeModeTemplate.SingleLine);

            StringBuilder sb = new StringBuilder();
            Header(ref sb);
            if (this.UseRotaryTable)
            {
                List<List<Cylindrical>> cpoints;
                cpoints = (this.UseAbsoluteMoves) ? CreateCylindricalList :CreateCylindricalOffsetList;
                foreach (List<Cylindrical> lcy in cpoints)
                {
                    string name =string.Format(_templates.PathNameTemplate.SingleLine, PathName,cpoints.IndexOf(lcy));
                    PathStart(ref sb,name);
                    foreach (Cylindrical cp in lcy)
                    {
                        if (lcy.IndexOf(cp) == 0)
                            sb.Append(gcode).AppendLine(_templates.LinearMoveToTemplate.SingleLine);
                        sb.AppendLine(string.Format(_templates.Point_Template.SingleLine,
                                                    Math.Round(cp.Radius,DP), 
                                                    Math.Round(cp.Angle,DP)));
                    }
                    PathEnd(ref sb,name);  
                }
            }
            else
            {
                List<List<Point>> points;
                points = (this.UseAbsoluteMoves) ?CurrentPath :CreateOffsetList;
                foreach (List<Point> lcy in points)
                {
                    string name = string.Format(_templates.PathNameTemplate.SingleLine, PathName, points.IndexOf(lcy));
                    PathStart(ref sb, name);
                    foreach (Point cp in lcy)
                    {
                        if (lcy.IndexOf(cp) == 0)
                            sb.Append(gcode).AppendLine(_templates.LinearMoveToTemplate.SingleLine);
                        sb.AppendLine(string.Format(_templates.Point_Template.SingleLine, 
                                                    Math.Round(cp.X,DP), 
                                                    Math.Round(cp.Y,DP)));
                    }
                    PathEnd(ref sb, name);
                }
                
            }
            Footer(ref sb);
            Main(ref sb);
            // now add main program
            Code.Add(sb.ToString());
        }

        private void Main(ref StringBuilder sb)
        {
            foreach (string tmplt in _templates.MainProgramTemplate.Lines)
            {
                    sb.AppendLine(Instantiate(tmplt));
            }
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
          
            foreach (string s in Code)
            {
                string pn = System.IO.Path.GetFileNameWithoutExtension(PathName);
                string filename = string.Format("{0}_Path{1}.{2}", 
                    pn, Code.IndexOf(s), "ngc");
                try
                {

                    StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    if (file != null)
                    {
                        await Windows.Storage.FileIO.WriteTextAsync(file, s);
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
