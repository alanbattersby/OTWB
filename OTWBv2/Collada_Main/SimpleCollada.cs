using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace grendgine_collada
{
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "COLLADA_S", Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
    public partial class SimpleCollada
    {
        [XmlAttribute("version")]
        public string Collada_Version;


        [XmlElement(ElementName = "asset")]
        public Grendgine_Collada_Asset Asset;

        [XmlElement(ElementName = "library_geometries")]
        public Grendgine_Collada_Library_Geometries Library_Geometries;

        [XmlElement(ElementName = "library_effects")]
        public Grendgine_Collada_Library_Effects Library_Effects;

        [XmlElement(ElementName = "library_materials")]
        public Grendgine_Collada_Library_Materials Library_Materials;

        [XmlElement(ElementName = "library_visual_scenes")]
		public Grendgine_Collada_Library_Visual_Scenes Library_Visual_Scenes;

        [XmlElement(ElementName = "scene")]
        public Grendgine_Collada_Scene Scene;

        public SimpleCollada()
        {
            Collada_Version = "1.5";
            Asset = new Grendgine_Collada_Asset();
           
            Library_Geometries = new Grendgine_Collada_Library_Geometries();
            Library_Effects = new Grendgine_Collada_Library_Effects();
            Library_Materials = new Grendgine_Collada_Library_Materials();
            Library_Visual_Scenes = new Grendgine_Collada_Library_Visual_Scenes();
            Scene = new Grendgine_Collada_Scene();
        }

		public async static Task<SimpleCollada> Load_File()
        {
            try
            {
				SimpleCollada col_scenes = null;
				XmlSerializer sr = new XmlSerializer(typeof(SimpleCollada));
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add(".xml");
                openPicker.FileTypeFilter.Add(".dae");
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    var stream = await file.OpenStreamForReadAsync();
                    col_scenes = (SimpleCollada)(sr.Deserialize(stream));
                }
               
				return col_scenes;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                //Console.ReadLine();
				return null;
            }			
		}

        public async static void Save_File(SimpleCollada data)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("DAE", new List<string>() { ".dae" });
            savePicker.FileTypeChoices.Add("Xml", new List<string>() { ".xml" });
            
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "Test";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                IRandomAccessStream sessionRandomAccess = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream sessionOutputStream = sessionRandomAccess.GetOutputStreamAt(0);
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SimpleCollada));
                    serializer.Serialize(sessionOutputStream.AsStreamForWrite(), data);
                }
                catch (System.InvalidOperationException e)
                {
                    Debug.WriteLine(e.InnerException);
                }
                sessionRandomAccess.Dispose();
                await sessionOutputStream.FlushAsync();
                sessionOutputStream.Dispose();
            }
        }
	}
}
