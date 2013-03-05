using OTWB.CodeGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OTWB.Settings
{
    public sealed partial class CodeSettingsContent : UserControl
    {
        Windows.Storage.ApplicationDataContainer localSettings;
        public CodeGenTemplates CodeTemplates { get; set; }

        public bool UseRotaryTable
        {
            get 
            {
                if (localSettings.Values[SettingsNames.USE_ROTARY_TABLE] != null)
                    return (bool)localSettings.Values[SettingsNames.USE_ROTARY_TABLE];
                else
                    return DefaultSettings.USE_ROTARY_TABLE;
            
            }
            set { localSettings.Values[SettingsNames.USE_ROTARY_TABLE] = value; }
        }
        public bool UseAbsoluteMoves
        {
            get
            {
                if (localSettings.Values[SettingsNames.USE_ABSOLUTE_MOVES] != null)
                    return (bool)localSettings.Values[SettingsNames.USE_ABSOLUTE_MOVES];
                else
                    return DefaultSettings.USE_ABSOLUTE_MOVES;

            }
            set { localSettings.Values[SettingsNames.USE_ABSOLUTE_MOVES] = value; }
        }
        public bool UseSubroutine
        {
            get
            {
                if (localSettings.Values[SettingsNames.USE_SUBROUTINE] != null)
                    return (bool)localSettings.Values[SettingsNames.USE_SUBROUTINE];
                else
                    return DefaultSettings.USE_SUBROUTINE;

            }
            set { localSettings.Values[SettingsNames.USE_SUBROUTINE] = value; }
        }
        public bool UseSingleFile
        {
            get
            {
                if (localSettings.Values[SettingsNames.USE_SINGLE_FILE] != null)
                    return (bool)localSettings.Values[SettingsNames.USE_SINGLE_FILE];
                else
                    return DefaultSettings.USE_SINGLE_FILE;

            }
            set { localSettings.Values[SettingsNames.USE_SINGLE_FILE] = value; }
        }

        public CodeSettingsContent()
        {
            this.InitializeComponent();
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            DataContext = this;
        }
    }
}
