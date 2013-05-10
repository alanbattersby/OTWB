using OTWB.Common;
using OTWB.Coordinates;
using OTWB.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OTWB.CodeGeneration
{
    /// <summary>
    /// Class that encapsulates the bound properties 
    /// used during template binding
    /// </summary>
    public class CodeGenDataContext : BindableBase
    {
        public CodeGenViewModel codegenViewmodel { get; set; }

        TemplateCollection _templates;
        public TemplateCollection Templates 
        {
            get { return _templates; }
            set { SetProperty(ref _templates, value); }
        }

        public CodeGenDataContext()
        {
            _gcodes = new GCODES();
            _mcodes = new MCODES();
            Templates = new TemplateCollection();
        }

        // local properties defined in Settings
        public bool UseRotaryTable
        {
            get { return (bool)BasicLib.GetSetting(SettingsNames.USE_ROTARY_TABLE); }
            set { BasicLib.SetSetting(SettingsNames.USE_ROTARY_TABLE, value); }
        }
        public bool UseAbsoluteMoves
        {
            get { return (bool)BasicLib.GetSetting(SettingsNames.USE_ABSOLUTE_MOVES); }
            set { BasicLib.SetSetting(SettingsNames.USE_ABSOLUTE_MOVES, value); }
        }
        public bool UseSubroutine
        {
            get { return (bool)BasicLib.GetSetting(SettingsNames.USE_SUBROUTINE); }
            set { BasicLib.SetSetting(SettingsNames.USE_SUBROUTINE, value); }
        }
        public bool UseOwords
        {
            get { return (bool)BasicLib.GetSetting(SettingsNames.USE_NAMED_O_WORDS); }
            set { BasicLib.SetSetting(SettingsNames.USE_NAMED_O_WORDS, value); }
        }
        public bool UseSingleFile
        {
            get { return (bool)BasicLib.GetSetting(SettingsNames.USE_SINGLE_FILE); }
            set { BasicLib.SetSetting(SettingsNames.USE_SINGLE_FILE, value); }
        }
        public string Now 
        { 
            get { return DateTime.Now.ToString(); } 
        }
        public int DP
        {
            get { return (int)BasicLib.GetSetting(SettingsNames.DECIMAL_PLACES); }
            set { BasicLib.SetSetting(SettingsNames.DECIMAL_PLACES, value); }
        }
        public double Feedrate
        {
            get { return (double)BasicLib.GetSetting(SettingsNames.FEED_RATE_VALUE);   }
            set { BasicLib.SetSetting(SettingsNames.FEED_RATE_VALUE, value); }
        }
        public double Safeheight
        {
            get { return (double)BasicLib.GetSetting(SettingsNames.SAFE_HEIGHT); }
            set { BasicLib.SetSetting(SettingsNames.SAFE_HEIGHT, value); }
        }
        public double Cuttingdepth
        {
            get { return (double)BasicLib.GetSetting(SettingsNames.CUTTING_DEPTH); }
            set { BasicLib.SetSetting(SettingsNames.CUTTING_DEPTH, value); }
        }

        // Properties required from viewmodel
        public int CurrentPathIndex
        {
            get { return codegenViewmodel.CurrentPathIndex; }
        }
        public string PatternName
        {
            get {
                if (codegenViewmodel.PathName.Contains("."))
                    return (codegenViewmodel.PathName.Substring(0, codegenViewmodel.PathName.IndexOf(".")));
                else
                    return codegenViewmodel.PathName;
            }
        }
        public string SubPathName
        {
            get
            {
                bool useNamed = (bool)BasicLib.GetSetting(SettingsNames.USE_NAMED_O_WORDS);
                if (useNamed)
                {
                    BindableCodeTemplate t = Templates.SubFilenameTemplate;
                    t.DataContext = this;
                    return "<" + t.Text + ">";
                }
                else
                {
                    int start = (int)BasicLib.GetSetting(SettingsNames.SUB_START_INDEX);
                    int inc = (int)BasicLib.GetSetting(SettingsNames.SUB_INDEX_INC);
                    return (start + CurrentPathIndex * inc).ToString();
                }

            }
        }
       
        public int PathCount
        {
            get { return codegenViewmodel.CurrentPath.Count(); }
        }

        Cartesian _firstpoint;
        public Cartesian FirstPoint
        {
            get { return _firstpoint; }
            set { SetProperty(ref _firstpoint, value); }
        }

        Cartesian _lastpoint;
        public Cartesian LastPoint
        {
            get { return _lastpoint; }
            set { SetProperty(ref _lastpoint, value); }
        }

        public ICoordinate CurrentPoint
        {
            get { return codegenViewmodel.CurrentPoint; }
        }

        public double ProfileHeight
        {
            get
            {
                return codegenViewmodel.ProfileHeight;

            }
        }

        // standard G codes and M codes  
        GCODES _gcodes;
        public GCODES Gcodes
        {
            get { return _gcodes; }
        }

        MCODES _mcodes;
        public MCODES Mcodes
        {
            get { return _mcodes; }
        }
    }
}
