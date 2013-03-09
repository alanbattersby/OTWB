using OTWB.Coordinates;
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
    public class CodeGenDataContext
    {
        CodeGenViewModel Context { get; set; }

        public CodeGenDataContext( CodeGenViewModel cntxt)
        {
            Context = cntxt;
            _gcodes = new GCODES();
            _mcodes = new MCODES();
        }

        public bool UseRotary
        {
            get { return Context.UseRotaryTable; }
        }
        public bool UseAbsolute
        {
            get { return Context.UseAbsoluteMoves; }
        }
        public bool UseSub
        {
            get { return Context.UseSubroutine;  }
        }
        public bool UseSingleFile
        {
            get { return Context.UseSingleFile; }
        }
        public string Now 
        { 
            get { return DateTime.Now.ToString(); } 
        }

        public int DP
        {
            get {return Context.DP;}
        }
        public double Feedrate
        {
            get { return Context.FeedRate; }
        }
        public double Safeheight
        {
            get { return Context.SafeHeight; }
        }
        public double Cuttingdepth
        {
            get { return Context.CuttingDepth; }
        }

        public int CurrentPathIndex
        {
            get { return Context.CurrentPathIndex; }
        }

        public string PatternName
        {
            get {
                if (Context.PathName.Contains("."))
                    return (Context.PathName.Substring(0, Context.PathName.IndexOf(".")));
                else
                    return Context.PathName;
            }
        }

        public string SubPathName
        {
            get
            {
                BindableCodeTemplate t = Context.Templates.SubFilenameTemplate;
                t.DataContext = this;
                return t.Text.Substring(0,t.Text.IndexOf("."));
            }
        }
        public int PathCount
        {
            get { return Context.CurrentPath.Count(); }
        }
        public Point FirstPoint
        {
            get { return Context.CurrentPath[Context.CurrentPathIndex][0]; }
        }

        public ICoordinate CurrentPoint
        {
            get { return Context.CurrentPoint; }
        }

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
