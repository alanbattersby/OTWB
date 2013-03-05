using Geometric_Chuck.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;

namespace OTWB.CodeGeneration
{
    public class GcodeFile : BindableBase
    {
        string _filename;
        public string FileName
        {
            get { return _filename; }
            set { SetProperty(ref _filename, value); }
        }

        string _code;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        public GcodeFile(string fn, string content)
        {
            FileName = fn;
            Code = content;
        }
        public GcodeFile() : this(string.Empty, string.Empty) {}

        //public async void SaveCode()
        //{
        //    FolderPicker folderPicker = new FolderPicker();
        //    folderPicker.FileTypeFilter.Add(".");
        //    folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        //    StorageFolder folder = await folderPicker.PickSingleFolderAsync();

        //    foreach (string s in Code)
        //    {
        //        string pn = System.IO.Path.GetFileNameWithoutExtension(PathName);
        //        string filename = string.Format("{0}_Path{1}.{2}",
        //            pn, Code.IndexOf(s), "ngc");
        //        try
        //        {

        //            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        //            if (file != null)
        //            {
        //                await Windows.Storage.FileIO.WriteTextAsync(file, s);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            CoreWindowDialog dlg = new CoreWindowDialog(e.Message);
        //            dlg.ShowAsync();
        //        }
        //    }
        //}
    }
}
