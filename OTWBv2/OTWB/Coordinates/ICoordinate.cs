// Coordinate.cs
// 
// Copyright (C) 2009 [Alan Battersby]
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Xml.Serialization;

namespace  OTWB.Coordinates
{
	
	/// <summary>
	/// This is the base for all coordinate systems
	/// </summary>
	
	[XmlInclude (typeof(Spherical))]
	[XmlInclude (typeof(Cartesian))]
	[XmlInclude (typeof(Cylindrical))]
	[XmlRootAttribute("Coordinate", Namespace="", IsNullable=false)]
	public interface ICoordinate 
	{
		[XmlIgnore]
		Cartesian  toCartesian3{ get ; }	// get as 3D cartesian
		
		[XmlIgnore]
		Cylindrical toCylindrical{ get;}	// get as 3D cylindrical
		
		[XmlIgnore]
		Spherical   toSpherical{ get; } 	// get as 3D spherical
		
		[XmlElement]
		double		Depth { get; set; }			// get its depth
		
		bool Equals(ICoordinate c);
		void Add(ICoordinate c1);			// add to this if makes sense
		
		[XmlIgnore]
		ICoordinate Copy { get; }			// make a copy of this 
		
		[XmlIgnore]
		bool PointAtInfinity { get; }    // return true if point at infinity 
		
        //void Transform(Transformation t);	// transform this coordinate
	}
}
