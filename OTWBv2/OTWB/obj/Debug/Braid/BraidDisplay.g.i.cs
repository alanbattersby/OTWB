﻿

#pragma checksum "C:\Users\ALAN\Documents\Windows 8 Projects\OTWBv2\OTWB\Braid\BraidDisplay.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9EEFDB9150F2DC7F8676B3F2331571E8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OTWB.Braid
{
    partial class BraidDisplay : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Border BraidBorder; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::OTWB.MyControls.ClipToBoundsControl BraidClip; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Canvas BraidCanvas; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///Braid/BraidDisplay.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            BraidBorder = (global::Windows.UI.Xaml.Controls.Border)this.FindName("BraidBorder");
            BraidClip = (global::OTWB.MyControls.ClipToBoundsControl)this.FindName("BraidClip");
            BraidCanvas = (global::Windows.UI.Xaml.Controls.Canvas)this.FindName("BraidCanvas");
        }
    }
}



