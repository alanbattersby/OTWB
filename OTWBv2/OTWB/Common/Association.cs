using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace OTWB.Common
{
    public class Association : BindableBase
    {

        public string ParamName { get; set; }
        public string Parameter
        {
            get { return "{" + ParamName + "}"; }
        }

        private object source;
        public object Source
        {
            set { source = value; }
        }

        private Windows.UI.Xaml.PropertyPath path;
        public string PropertyPath
        {
            set
            {
                if (value != string.Empty)
                    path = new Windows.UI.Xaml.PropertyPath(value);
            }
        }

        public bool IsConstant
        {
            get { return (path == null); }
        }

        public string Value
        {
            get 
            {
               Binding binding = new Binding();
               binding.Source = source;
               binding.Path = path;
               Text.SetBinding(TextBlock.TextProperty, binding);
               return Text.Text;
             }
        }

        private TextBlock Text;
        public Association() : base()
        {
            Text = new TextBlock();
            path = null;
            source = null;
          
        }

        public Association(string pname, object src, string ppath)
            : base()
        {
            Text = new TextBlock();
            ParamName = pname;
            Source = src;
            PropertyPath = ppath;
        }

    }
}
