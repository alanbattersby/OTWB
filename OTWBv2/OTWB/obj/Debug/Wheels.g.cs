﻿

#pragma checksum "C:\Users\ALAN\Documents\Windows 8 Projects\OTWBv2\OTWB\Wheels.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0EA235F260AF8FAFE8EC80477DE7C22B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Geometric_Chuck
{
    partial class Wheels : global::Geometric_Chuck.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 34 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Grid_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 35 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Points_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 26 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ExportPaths_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 27 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click_2;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 28 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Import_Pattern_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 29 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Gcode_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 30 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Add_Wheel_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 31 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Remove_Wheel_click;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 112 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.IncrementCombo_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 68 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.PatternChoices_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 57 "..\..\Wheels.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

