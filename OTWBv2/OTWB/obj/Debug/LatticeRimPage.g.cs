﻿

#pragma checksum "C:\Users\ALAN\Documents\Windows 8 Projects\OTWBv2\OTWB\LatticeRimPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "56E86C22C69AE0B5A4F62FB496E08C38"
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
    partial class LatticeRimPage : global::OTWB.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 35 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.ToggleButton_Tapped;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 36 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Grid_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 37 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Points_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 27 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ExportPaths_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 28 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Export_Pattern_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 29 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Import_Pattern_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 30 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.New_Lattice_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 31 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Clear_Lattice_Click;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 32 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Gcode_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 146 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Popup)(target)).Opened += this.Bigdisplay_Opened;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 158 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_2;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 159 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Add_Line_Click;
                 #line default
                 #line hidden
                break;
            case 13:
                #line 160 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Remove_Line_Click;
                 #line default
                 #line hidden
                break;
            case 14:
                #line 144 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.RangeBase)(target)).ValueChanged += this.LatticePosition_ValueChanged;
                 #line default
                 #line hidden
                break;
            case 15:
                #line 84 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.LatticeLineList_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 16:
                #line 131 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.IncrementCombo_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 17:
                #line 109 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_1;
                 #line default
                 #line hidden
                break;
            case 18:
                #line 114 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Add_Line_Click;
                 #line default
                 #line hidden
                break;
            case 19:
                #line 119 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Remove_Line_Click;
                 #line default
                 #line hidden
                break;
            case 20:
                #line 69 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.PatternChoices_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 21:
                #line 76 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_3;
                 #line default
                 #line hidden
                break;
            case 22:
                #line 59 "..\..\LatticeRimPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

