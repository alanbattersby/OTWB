﻿

#pragma checksum "C:\Users\ALAN\Documents\Windows 8 Projects\OTWBv2\OTWB\MyControls\RangeControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E24EBCA8F849B88449DB4A84C5374E4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OTWB.MyControls
{
    partial class RangeControl : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Callisto.Controls.NumericUpDown Start; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Callisto.Controls.NumericUpDown Inc; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Callisto.Controls.NumericUpDown End; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///MyControls/RangeControl.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            Start = (global::Callisto.Controls.NumericUpDown)this.FindName("Start");
            Inc = (global::Callisto.Controls.NumericUpDown)this.FindName("Inc");
            End = (global::Callisto.Controls.NumericUpDown)this.FindName("End");
        }
    }
}



