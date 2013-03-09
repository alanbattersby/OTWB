//  
//  Copyright (C) 2009 Alan Battersby
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Text;

namespace OTWB.CodeGeneration
{
	
	/// <summary>
	/// This ennumeration gives values to all gcodes
	/// </summary>
	/// 
	public class GCODES
	{
        public static string Rapid_Move
        {
            get { return "G00"; }
        }
        public static string Linear_Move
        {
            get { return "G01"; }
        }
		public static string CW_Circular_Move 
        {
            get { return "G02"; }
        }
        public static string CCW_Circular_Move
        {
            get { return "G03"; }
        }
        public static string Dwell
        {
            get { return "G04"; }
        }
        public static string Coordinate_Origin
        {
            get { return "G10"; }
        }

		// G05.1 Q1. 	Ai Nano contour control
		// G05 P10000 	HPCC
		// G10/G11 	Programmable Data input/Data write cancel
        public static string X_Y_Plane
        {
            get { return "G17"; }
        }
        public static string X_Z_Plane
        {
            get { return "G18"; }
        }
        public static string Y_Z_Plane
        {
            get { return "G19"; }
        }

        public static string Units_Inches
        {
            get { return "G20"; }
        }
        public static string Units_MM
        {
            get { return "G21"; }
        }
        public static string Return_To_Home
        {
            get { return "G28"; }
        }

        public static string Second_Reference_Point_Return
        {
            get { return "G30"; }
        }
        public static string Skip_Function
        {
            get { return "G31"; }
        }
        public static string Constant_Pitch_Threading
        {
            get { return "G33"; }
        }
        public static string Variable_Pitch_Threading
        {
            get { return "G34"; }
        }
        public static string Straight_Probe
        {
            get { return "G38.2"; }
        }

        public static string Tool_Radius_Comp_Off
        {
            get { return "G40"; }
        }
        public static string Tool_Radius_Comp_Left
        {
            get { return "G41"; }
        }
        public static string Tool_Radius_Comp_Right
        {
            get { return "G42"; }
        }

        public static string Tool_Offset_Comp_Negative
        {
            get { return "G43"; }
        }
        public static string Tool_Offset_Comp_Positive
        {
            get { return "G44"; }
        }
        public static string Tool_Offset_Comp_Cancel
        {
            get { return "G49"; }
        }

        public static string Axis_Offset_Single_Inc
        {
            get { return "G45"; }
        }
        public static string Axis_Offset_Single_Dec
        {
            get { return "G46"; }
        }
        public static string Axis_Offset_Double_Inc
        {
            get { return "G47"; }
        }
        public static string Axis_Offset_Double_Dec
        {
            get { return "G48"; }
        }

        public static string Machine_Coord_Sys
        {
            get { return "G53"; }
        }
        public static string Work_Coord_Sys_1
        {
            get { return "G54"; }
        }
        public static string Work_Coord_Sys_2
        {
            get { return "G55"; }
        }
        public static string Work_Coord_Sys_3
        {
            get { return "G56"; }
        }
        public static string Work_Coord_Sys_4
        {
            get { return "G57"; }
        }
        public static string Work_Coord_Sys_5
        {
            get { return "G58"; }
        }
        public static string Work_Coord_Sys_6
        {
            get { return "G59"; }
        }
        public static string Work_Coord_Sys_7
        {
            get { return "G59.1"; }
        }
        public static string Work_Coord_Sys_8
        {
            get { return "G59.2"; }
        }
        public static string Work_Coord_Sys_9
        {
            get { return "G59.3"; }
        }

        public static string Exact_Path
        {
            get { return "G61"; }
        }
        public static string Exact_stop
        {
            get { return "G61.1"; }
        }
        public static string Continuous_Path
        {
            get { return "G64"; }
        }


		// now missing the following codes 
		// G54.1 P1 - P48		Extended work coordinate systems
		// G73 	High speed drilling canned cycle
		// G74 	Left hand tapping canned cycle
		// G76 	Fine boring canned cycle
		// G80 	Cancel canned cycle
		// G81 	Simple drilling cycle
		// G82 	Drilling cycle with dwell
		// G83 	Peck drilling cycle
		// G84 	Tapping cycle
		// G84.2 	Direct right hand tapping canned cycle
		// G85 canned cycle: boring, no dwell, feed out
		// G86 canned cycle: boring, spindle stop, rapid out
		// G87 canned cycle: back boring
		// G88 canned cycle: boring, spindle stop, manual out
		// G89 canned cycle: boring, dwell, feed out

        public static string Absolute_Moves
        {
            get { return "G90"; }
        }
        public static string Incremental_Moves
        {
            get { return "G91"; }
        }
        public static string Set_Coord_System_Offsets
        {
            get { return "G92"; }
        }
        public static string Zero_Coord_System_Offsets
        {
            get { return "G92.1"; }
        }
        public static string Cancel_Coord_System_Offsets
        {
            get { return "G92.2"; }
        }
        public static string Apply_Coord_System_offsets
        {
            get { return "G92.3"; }
        }

        public static string Inverse_Time_Mode
        {
            get { return "G93"; }
        }
        public static string Units_Per_Minute
        {
            get { return "G94"; }
        }
        public static string Units_Per_Rev
        {
            get { return "G95"; }
        }

        public static string Spindle_Const_Cutting_Speed
        {
            get { return "G96"; }
        }
        public static string Spindle_Const_Rot_Speed
        {
            get { return "G97"; }
        }
        public static string Return_to_Initial_Z_plane
        {
            get { return "G98"; }
        }
        public static string Return_to_Initial_R_plane
        {
            get { return "G99"; }
        }
	}
	
	public class MCODES
	{
        public static string Program_Stop
        {
            get { return "M0"; }
        }
        public static string Optional_Stop
        {
            get { return "M1"; }
        }
        public static string End_of_Program
        {
            get { return "M2"; }
        }
        public static string Spindle_on_CW
        {
            get { return "M3"; }
        }
        public static string Spindle_on_CCW
        {
            get { return "M4"; }
        }
        public static string Spindle_off
        {
            get { return "M5"; }
        }
        public static string Tool_Change
        {
            get { return "M6"; }
        }
        public static string Coolant_on_flood
        {
            get { return "M7"; }
        }
        public static string Coolant_on_mist
        {
            get { return "M8"; }
        }
        public static string Coolant_off
        {
            get { return "M9"; }
        }
        public static string Pallet_clamp
        {
            get { return "M10"; }
        }
        public static string Pallet_un_clamp
        {
            get { return "M11"; }
        }
        public static string End_of_program
        {
            get { return "M30"; }
        }
        public static string Enable_speed_and_feed_overrides
        {
            get { return "M48"; }
        }
        public static string Disable_speed_and_feed_overrides
        {
            get { return "M49"; }
        }
        public static string Pallet_shuttle_and_program_stop
        {
            get { return "M60"; }
        }
        public static string Call_Subroutine
        {
            get { return "M98"; }
        }
        public static string End_Subroutine
        {
            get { return "M99"; }
        }
	}
	
		
	public class PCODE
	{
		public static string Sub			= "sub";
		public static string Endsub		= "endsub";
		public static string O			= "O";
		public static string CODE			= "code";
		public static string HASH			= "#";
		public static string OPENBRACKET	= "[";
		public static string CLOSEBRACKET	= "]";
		public static string Callsub		= "call";
		public static string EndIf 		= "endif";
		public static string Else  		= "else";
		public static string EndWhile		= "endwhile";
		public static string Do 			= "do";
		public static string Addition		= "+";
		public static string Subtraction	= "-";
		public static string Times		= "*";
		public static string Division		= "-";
		public static string OPENANGLE	= "<";
		public static string CLOSEANGLE	= ">";
		public static string UNDERSCORE	= "_";
	
		
		public static string Less_than_or_equal 	= "LE";	
		public static string Equal 					= "EQ";
		public static string Not_equal 				= "NE";
		public static string Greater_than 			= "GT";
		public static string Greater_than_or_equal 	= "GE";
		public static string Less_than 				= "LT";
		
		// pcode functions
		public static string ABS			="abs";
		
		public static string Onum(int n)
		{
			return string.Format("{0}{1}",PCODE.O,n);	
		}
		
		public static string OwordName(string n)
		{
			return string.Format("{0}{1}{2}",PCODE.OPENANGLE,n,PCODE.CLOSEANGLE);
		}
		
		public static string NamedOWord(string name)
		{
			return string.Format("{0}{1}{2}{3}", PCODE.O,PCODE.OPENANGLE,name,PCODE.CLOSEANGLE);
		}
		
		public static string Param(int n)
		{
			return string.Format("{0}{1}",PCODE.HASH,n);	
		}
		
		public static string CallParam(int n)
		{
			return Bracket(Param(n));	
		}
		
		public static string LocalVariable(string v)
		{
			return string.Format("{0}{1}{2}{3}",
			            PCODE.HASH,PCODE.OPENANGLE,v,PCODE.CLOSEANGLE);
		}
		
		public static string GlobalVariable(string v)
		{
			return string.Format("{0}{1}{2}{3}{4}",
			            PCODE.HASH,PCODE.OPENANGLE,PCODE.UNDERSCORE,v,PCODE.CLOSEANGLE);
		}
		
		public static string ValofGlobal(string v)
		{
			return Bracket(GlobalVariable(v));
		}
		
		public static string ValofLocal(string v)
		{
			return Bracket(LocalVariable(v));
		}
		
		public static string Valof(string v)
		{
			return Bracket(v);	
		}
		
		public static string Bracket(string expr)
		{
			return PCODE.OPENBRACKET + expr + PCODE.CLOSEBRACKET;	
		}
		
		public static string While(string test)
		{
			return string.Format("while [{0}]",test);		                     
		}
		
		public static string IncVariable(string v, string exp)
		{
			return string.Format("{0}= [{0}+{1}]",v,exp);
		}
		
		public static string Assign(string v, string expr)
		{
			return string.Format("{0} = {1}",v,expr);
		}
		
		public static string If(string cond)
		{	
			return string.Format("if [{0}]",cond);
		}
		
		public static string Test(string v1, string cond, string expr)
		{
			return string.Format("{0} {1} {2}",v1,cond,expr);
		}
		
		public static string Binary(string e1, string op, string e2)
		{
			return PCODE.Bracket(string.Format("{0} {1} {2}",e1,op,e2));	
		}
		
		public static string Mult(string e1, string e2)
		{
			return PCODE.Binary(e1,PCODE.Times,e2);	
		}
		
		public static string Add(string e1, string e2)
		{
			return PCODE.Binary(e1,PCODE.Addition,e2);	
		}
		
		public static string Subtract(string e1, string e2)
		{
			return PCODE.Binary(e1,PCODE.Subtraction,e2);	
		}
		
		public static string Divide(string e1, string e2)
		{
			return PCODE.Binary(e1,PCODE.Division,e2);	
		}
		
		public static string Literal(float n)
		{
			return n.ToString();	
		}
		
		public static string gfuncall (string fun, params string[] values)
		{
			StringBuilder sb = new StringBuilder(fun);
			for (int i=0; i < values.Length; i++)
			{
				sb.Append(' ').Append(PCODE.Bracket(values[i]));
			}
			return sb.ToString();
		}
	}
	
	
}
