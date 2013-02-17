using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OTWB.CodeGeneration
{
    public class CodeTemplate : BindableBase
    {
        public static string TemplateEmpty = string.Empty;
        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        Collection<string> _lines;

        [XmlArray("LINES")]
        [XmlArrayItem("LINE")]
        public string[] SLines
        {
            get { return Lines.ToArray(); }
            set {
                    Lines = new Collection<string>(value);
            }
        }

        [XmlIgnore]
        public Collection<string> Lines
        {
            get { return _lines; }
            set { SetProperty(ref _lines, value); }
        }

        public CodeTemplate(string name, string tmpl)
        {
            Lines = new Collection<string>();
            Name = name;
            Lines.Add( tmpl);
        }
        public CodeTemplate(string name) : this(name, string.Empty) { }
        public CodeTemplate() : this(string.Empty, string.Empty) { }
        public CodeTemplate(string name, List<string> items)
        {
            Name = name;
            Lines = new Collection<string>(items);
        }

        public void AddLine(string line)
        {
            Lines.Add(line);
        }

        public bool IsMultiLine
        {
            get { return Lines.Count > 1; }
        }

        public string this [int indx]
        {
            get { return Lines[indx]; }
            set
            {
                if (Lines.Count > (indx + 1))
                    Lines[indx] = value;
                else
                    Lines.Add(value);
            }

        }

        [XmlIgnore]
        public string SingleLine
        {
            get 
            {
                if (Lines.Count >= 1)
                    return Lines.ElementAt<string>(0);
                else
                    return CodeTemplate.TemplateEmpty;
            }
        }

       
    }
}
