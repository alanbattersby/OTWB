using Geometric_Chuck.Common;
using OTWB.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OTWB.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BindableString : BindableBase
    {
        string format;
        public string Format
        {
            get { return format; }
            set { SetProperty(ref format, value); }
        }

        string _value;
        public string Value
        {
            get
            {
                if (_value == string.Empty)
                    update();
                return _value;
            }
        }

        ObservableCollection<Association> _params;
        public ObservableCollection<Association> Params 
        {
            get { return _params; }
            set
            {
                _params = value;
                foreach (Association a in _params)
                    a.PropertyChanged += param_PropertyChanged;
            }
        }

        void update()
        {
            _value = new string(format.ToCharArray());

            foreach (Association a in _params)
            {
                _value = _value.Replace(a.Parameter, a.Value);
            }
        }

        void param_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            update();
        }

        public void Add(Association a)
        {
            _params.Add(a);
            a.PropertyChanged += param_PropertyChanged;
            update();
        }

        public void Add(string pname, object src, string ppath)
        {
            this.Add(new Association(pname, src, ppath));
        }

        public BindableString()
        {
            _value = string.Empty;
            _params = new ObservableCollection<Association>();
        }


    }
}
