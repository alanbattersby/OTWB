﻿

#pragma checksum "C:\Users\ALAN\Documents\Windows 8 Projects\OTWBv2\OTWB\LatticeFacePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3A7A02BCAC3C91647ADE4E20A1EFD5D9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OTWB
{
    partial class LatticeFacePage : global::OTWB.Common.LayoutAwarePage
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::OTWB.Common.LayoutAwarePage pageRoot; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Primitives.Popup Bigdisplay; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::OTWB.Lattice.LatticeDisplay PopupDisplay; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::OTWB.PathDisplay LatticePathDisplay; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel LatticePanel; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock StatusBlock; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressRing ProgressRing1; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView LatticeLineList; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel SmallDisplay; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Callisto.Controls.NumericUpDown PathWidthUpdown; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ComboBox IncrementCombo; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::OTWB.Lattice.LatticeDisplay LatticeDisplay; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button BigDisplayButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button AddLineButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button RemoveLineButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ComboBox PatternChoices; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button backButton; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock pageTitle; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualStateGroup ApplicationViewStates; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState FullScreenLandscape; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState Filled; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState FullScreenPortrait; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState Snapped; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///LatticeFacePage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            pageRoot = (global::OTWB.Common.LayoutAwarePage)this.FindName("pageRoot");
            Bigdisplay = (global::Windows.UI.Xaml.Controls.Primitives.Popup)this.FindName("Bigdisplay");
            PopupDisplay = (global::OTWB.Lattice.LatticeDisplay)this.FindName("PopupDisplay");
            LatticePathDisplay = (global::OTWB.PathDisplay)this.FindName("LatticePathDisplay");
            LatticePanel = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("LatticePanel");
            StatusBlock = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("StatusBlock");
            ProgressRing1 = (global::Windows.UI.Xaml.Controls.ProgressRing)this.FindName("ProgressRing1");
            LatticeLineList = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("LatticeLineList");
            SmallDisplay = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("SmallDisplay");
            PathWidthUpdown = (global::Callisto.Controls.NumericUpDown)this.FindName("PathWidthUpdown");
            IncrementCombo = (global::Windows.UI.Xaml.Controls.ComboBox)this.FindName("IncrementCombo");
            LatticeDisplay = (global::OTWB.Lattice.LatticeDisplay)this.FindName("LatticeDisplay");
            BigDisplayButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("BigDisplayButton");
            AddLineButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("AddLineButton");
            RemoveLineButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("RemoveLineButton");
            PatternChoices = (global::Windows.UI.Xaml.Controls.ComboBox)this.FindName("PatternChoices");
            backButton = (global::Windows.UI.Xaml.Controls.Button)this.FindName("backButton");
            pageTitle = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("pageTitle");
            ApplicationViewStates = (global::Windows.UI.Xaml.VisualStateGroup)this.FindName("ApplicationViewStates");
            FullScreenLandscape = (global::Windows.UI.Xaml.VisualState)this.FindName("FullScreenLandscape");
            Filled = (global::Windows.UI.Xaml.VisualState)this.FindName("Filled");
            FullScreenPortrait = (global::Windows.UI.Xaml.VisualState)this.FindName("FullScreenPortrait");
            Snapped = (global::Windows.UI.Xaml.VisualState)this.FindName("Snapped");
        }
    }
}



