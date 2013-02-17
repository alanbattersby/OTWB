using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Windows.Data.Xml.Dom;
namespace grendgine_collada
{
	 //[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
	public partial class Grendgine_Collada_Set_Param
	{
		[XmlAttribute("ref")]
		public string Ref;
		
		
		/// <summary>
		/// The element is the type and the element text is the value or space delimited list of values
		/// </summary>
		[XmlAnyElement]
		public object[] Data;	
	}
}

