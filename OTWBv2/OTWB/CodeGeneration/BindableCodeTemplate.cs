using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Xml.Serialization;
using Windows.UI.Xaml.Documents;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace OTWB.CodeGeneration
{
    /// <summary>
    /// Item which may be plain text or a bound value
    /// </summary>
    class CodeLineItem
    {

        public string Value { get; set; }
        [XmlIgnore]
        public Binding Binding { get; set; }
        [XmlIgnore]
        public bool IsBinding { get { return Binding != null; } }

        public CodeLineItem(string v)
        {
            Value = v;
            Binding = null;
        }

        public CodeLineItem(Binding b)
        {
            Value = string.Empty;
            Binding = b;
        }
    }

    /// <summary>
    /// This class creates a bindable code template which may
    /// be a mixture of plain text and bound items.
    /// Bound values are surrounded by braces { } and reflect the values of
    /// Properties defined in the DataContext. They are in the format 
    /// {Bindable pathstring}
    /// where pathstring is a path to the required property value.
    /// 
    /// The Template property is  used to set the string to be bound and
    /// The Text property returns this string with all bindings replaced by
    /// actual values.
    /// 
    /// Internally this class relies upon a TextBlock to instigate the binding action.
    /// </summary>
    public class BindableCodeTemplate : BindableBase
    {
        TextBlock txt;  // the text block to be constructed
        List<CodeLineItem> Items { get; set; }

        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public BindableCodeTemplate()
            : this(string.Empty, string.Empty) {}
       
        public BindableCodeTemplate(string name)
            : this(name, string.Empty) {}

        public BindableCodeTemplate(string name, string tmpl)
        {
            Items = new List<CodeLineItem>();
            txt = new TextBlock();
            Name = name;
            Template = tmpl;
        }

        /// <summary>
        /// This property returns the processed text where all bound items have been
        /// replaced by their values.
        /// </summary>
        [XmlIgnore]
        public string Text
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                foreach (CodeLineItem itm in this.Items)
                {
                    if (!itm.IsBinding)
                        sb.Append(itm.Value);
                    else
                    {
                        txt.SetBinding(TextBlock.TextProperty, itm.Binding);
                        sb.Append(txt.Text);
                    }
                }
                return sb.ToString();
            }
        }

        string _template;
        /// <summary>
        /// This property is the raw content of the template and is a
        /// mixture of one or more pieces of plain text and/or bound values.
        /// </summary>
        public string Template
        {
            get { return _template; }
            set { 
                    SetProperty(ref _template, value);
                    if ((DataContext != null) && (value != string.Empty))
                        Parse1();
                }
        }

        private void Parse1()
        {
            Items.Clear();
            Regex regx = new Regex(@"\{(.*?)\}");
            string[] sruns = regx.Split(_template);
            foreach (string srun in sruns)
            {
                if (srun.StartsWith("Binding"))
                {
                    string ppath = srun.Substring(srun.IndexOf(' '));
                    Binding b = new Binding();
                    b.Source = _datacontext;
                    b.Path = new Windows.UI.Xaml.PropertyPath(ppath.Trim());
                    Items.Add(new CodeLineItem(b));
                }
                else
                    Items.Add(new CodeLineItem(srun));
            }
        }

        object _datacontext;
        /// <summary>
        /// This parameter provides the context i.e the data from which the 
        /// value of the bound item is obtained.
        /// </summary>
        [XmlIgnore]
        public object DataContext
        {
            get { return _datacontext; }
            set { 
                    SetProperty(ref _datacontext, value);
                    if (Template != string.Empty)
                        Parse1();
            }
        }
    }
}
