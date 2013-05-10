using OTWB.CodeGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
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
       
        public CodeSettingsContent()
        {
            this.InitializeComponent();
            this.DataContext = App.CodeSettingsContext; // this;
        }

        //Import new Template collection
        private async void Button_OpenLocal_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".ctmpl");
            openPicker.FileTypeFilter.Add(".xml");
            StorageFile file = await openPicker.PickSingleFileAsync();
            XmlSerializer ser;
            TemplateCollection tmpls;
            if (file != null)
            {
                try
                {
                    var stream = await file.OpenStreamForReadAsync();
                    ser = new XmlSerializer(typeof(TemplateCollection));
                    tmpls = (TemplateCollection)ser.Deserialize(stream);
                   
                }
                catch (XmlException ex)
                {
                    MessageDialog dialog = new MessageDialog(ex.InnerException.ToString(),
                                               "Template file failed to load");
                    dialog.ShowAsync();
                    return;
                }
                App.CodeSettingsContext.Templates = tmpls;
                
            }
        }

        private async void Button_Savelocal_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Template", new List<string>() { ".ctmpl" });
            savePicker.FileTypeChoices.Add("Xml", new List<string>() { ".xml" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Template Collection";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                IRandomAccessStream sessionRandomAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);

                var serializer = new XmlSerializer(typeof(TemplateCollection));
                serializer.Serialize(sessionOutputStream.AsStreamForWrite(), App.CodeSettingsContext.Templates);
                sessionRandomAccess.Dispose();
                await sessionOutputStream.FlushAsync();
                sessionOutputStream.Dispose();
            }      
        }


    }
}
