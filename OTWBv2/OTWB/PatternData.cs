using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace Geometric_Chuck
{
    class PatternData
    {

        private static List<object> _data = new List<object>();
 
        public static List<object> Data
        {
            get { return _data; }
            set { _data = value; }
        }
 
        public static StorageFile file { get; set; }
 
        public const string filename = "patterndata.xml";
 
        static async public Task Save<T>()
        {
            try
            {

                if (await DoesFileExistAsync())
                {
                    await Windows.System.Threading.ThreadPool.RunAsync((sender)
                        => PatternData.SaveAsync<T>().Wait(), Windows.System.Threading.WorkItemPriority.Normal);
                }
                else
                {
                    file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    await Windows.System.Threading.ThreadPool.RunAsync((sender)
                           => PatternData.SaveAsync<T>().Wait(), Windows.System.Threading.WorkItemPriority.Normal);
                }
            }
            catch (AggregateException ae)
            {

                ae.Handle((x) =>
                {
                    if (x is UnauthorizedAccessException) // This we know how to handle.
                    {
                        new MessageDialog("You do not have permission to access all folders in this path.\n See your network administrator or try another path.");
                        return true;
                    }
                    return false; // Let anything else stop the application.
                });

            }
        }
 
        static async public Task Restore<T>()
        {
            if (await DoesFileExistAsync())
            {
                await Windows.System.Threading.ThreadPool.RunAsync((sender) 
                    => PatternData.RestoreAsync<T>().Wait(), Windows.System.Threading.WorkItemPriority.Normal);
            }
            else
            {
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename);
            }
        }
 
        public static async Task<bool> DoesFileExistAsync()
        {
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync(PatternData.filename);
                return true;
            }
            catch
            {
                return false;
            }
        }
 
        static async private Task SaveAsync<T>()
        {
                StorageFile sessionFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                IRandomAccessStream sessionRandomAccess = await sessionFile.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
                var serializer = new XmlSerializer(typeof (List<object>), new Type[] {typeof (T)});
 
                //Using XmlSerializer , look at the Dog-class
                serializer.Serialize(sessionOutputStream.AsStreamForWrite(), _data);
                sessionRandomAccess.Dispose();
                await sessionOutputStream.FlushAsync();
                sessionOutputStream.Dispose();
        }
 
        static async private Task RestoreAsync<T>()
        {
                StorageFile sessionFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                if (sessionFile == null )
                {
                    return;
                }

                IInputStream sessionInputStream = await sessionFile.OpenReadAsync();

                //Using DataContractSerializer , look at the cat-class
                // var sessionSerializer = new DataContractSerializer(typeof(List<object>), new Type[] { typeof(T) });
                //_data = (List<object>)sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());

                //Using XmlSerializer , look at the Dog-class
                try
                {
                    var serializer = new XmlSerializer(typeof(List<object>), new Type[] { typeof(T) });
                    _data = (List<object>)serializer.Deserialize(sessionInputStream.AsStreamForRead());
                }
                catch (System.InvalidOperationException)
                {
                    throw new Exception("Restore Error");
                }
                finally
                {
                    sessionInputStream.Dispose();
                }
        }

        static private async Task ReadData()
        {
            string data;
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"GEODATA.txt");
            var stream = await file.OpenStreamForReadAsync();
            var rdr = new StreamReader(stream);
            data = await rdr.ReadToEndAsync();
            var lines = data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                var info = line.Split(new char[] { ':' });
                int indx = int.Parse(info[0].Substring(1, info[0].Length - 2));
                string[] pdata = info[1].Split(new char[] { ',' });
                PatternType typ = PatternType.BAZELEY;
                if ((indx > MainPage.Max_Built_In_Bazeley_Index) &&
                    (indx < MainPage.Max_Built_In_Ross_Index))
                    typ = PatternType.ROSS;
                BazelyChuck bc = new BazelyChuck(indx, pdata);
                _data.Add(bc);
            }
        }
    }
}

