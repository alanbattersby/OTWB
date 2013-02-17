using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Windows.Data.Xml.Dom;
namespace grendgine_collada
{
	
	/// <summary>
	/// This is the core <technique>
	/// </summary>
    // //[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
	public partial class Grendgine_Collada_Technique
	{
		[XmlAttribute("profile")]
		public string profile;
		[XmlAttribute("xmlns")]
		public string xmlns;

		[XmlAnyElement]
		public object[] Data;	

	}
}

