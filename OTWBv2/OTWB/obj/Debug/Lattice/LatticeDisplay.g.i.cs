﻿

#pragma checksum "C:\Users\ALAN\Documents\Windows 8 Projects\OTWBv2\OTWB\Lattice\LatticeDisplay.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "BD3D91ED04E5FD9BADCF107A573AF71F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OTWB.Lattice
{
    partial class LatticeDisplay : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Border LatticeBorder; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::OTWB.MyControls.ClipToBoundsControl LatticeClip; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Canvas LatticeCanvas; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///Lattice/LatticeDisplay.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            LatticeBorder = (global::Windows.UI.Xaml.Controls.Border)this.FindName("LatticeBorder");
            LatticeClip = (global::OTWB.MyControls.ClipToBoundsControl)this.FindName("LatticeClip");
            LatticeCanvas = (global::Windows.UI.Xaml.Controls.Canvas)this.FindName("LatticeCanvas");
        }
    }
}



