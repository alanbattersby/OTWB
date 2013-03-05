using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace OTWB.Common
{
    public static class BindableExtender
    {
        public static string GetBindableText(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableTextProperty);
        }

        public static void SetBindableText(DependencyObject obj,
            string value)
        {
            obj.SetValue(BindableTextProperty, value);
        }

        public static readonly DependencyProperty BindableTextProperty =
            DependencyProperty.RegisterAttached("BindableText",
                typeof(string),
                typeof(BindableExtender),
                new PropertyMetadata(null,
                    BindableTextProperty_PropertyChanged));

        private static void BindableTextProperty_PropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is Run)
            {
                ((Run)dependencyObject).Text = (string)e.NewValue;
            }
        }
    }
}
