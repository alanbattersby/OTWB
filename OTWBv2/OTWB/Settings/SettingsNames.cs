using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTWB.Settings
{
    public class SettingsNames
    {
        // boolean flags
        public static string USE_ROTARY_TABLE = "UseRotaryTable";
        public static string USE_ABSOLUTE_MOVES = "UseAbsoluteMoves";
        public static string USE_SUBROUTINE = "UseSubroutine";
        public static string USE_SINGLE_FILE = "UseSingleFile";
        public static string USE_NAMED_O_WORDS = "UseNamedOwords";
        public static string CLIP = "Clip";
        public static string HYPO = "Hypo";

        // int values
        public static string DECIMAL_PLACES = "DP";
        public static string SUB_START_INDEX = "SubStartIndex";
        public static string SUB_INDEX_INC = "SubIndexIncrement";
        public static string REPEAT_X = "Repeat_X";
        public static string REPEAT_Y = "Repeat_Y";
        // double values
        public static string FEED_RATE_VALUE = "FeedrateValue";
        public static string SAFE_HEIGHT = "SafeHeight";
        public static string CUTTING_DEPTH = "CuttingDepth";
        public static string TOOL_POSITION = "Tool_Position";
        public static string WORK_PIECE_RADIUS = "WorkPieceRadius";
        public static string WIDTH = "Width";
        public static string HEIGHT = "Height";
        public static string MARGIN = "Margin";
        public static string OFFSET_X = "Offset_X";
        public static string OFFSET_Y = "Offset_Y";
        public static string HYPO_K = "Hypo_K";

        // string values
        public static string GCODE_FILE_EXT = "GcodeFileExt"; 

        // string templates for code generation
        public static string FIRST_POINT_TEMPLATE = "FIRST_POINT_TEMPLATE";
        public static string LAST_POINT_TEMPLATE = "LAST_POINT_TEMPLATE";
        
        public static string XY_POINT_TEMPLATE = "XY_POINT_TEMPLATE";
        public static string RA_POINT_TEMPLATE = "RA_POINT_TEMPLATE";

        public static string SUB_START_TEMPLATE = "SUB_START_TEMPLATE";
        public static string SUB_END_TEMPLATE = "SUB_END_TEMPLATE";
        public static string SUB_CALL_TEMPLATE = "SUB_CALL_TEMPLATE";

        public static string PATH_START_TEMPLATE = "PATH_START_TEMPLATE";
        public static string PATH_END_TEMPLATE = "PATH_END_TEMPLATE";

        public static string PROGRAM_END_TEMPLATE = "PROGRAM_END_TEMPLATE";
        public static string MAIN_BODY_TEMPLATE = "MAIN_BODY_TEMPLATE";
        public static string PATH_NAME_TEMPLATE = "PATH_NAME_TEMPLATE";
        public static string HEADER_TEMPLATE = "HEADER_TEMPLATE";
        public static string GLOBALS_TEMPLATE = "GLOBALS_TEMPLATE";

        // file name templates
        public static string SUB_FILE_NAME_TEMPLATE = "SUB_FILE_NAME_TEMPLATE";
        public static string MAIN_FILE_NAME_TEMPLATE = "MAIN_FILE_NAME_TEMPLATE";
       

        public static IList<string> AllTemplates()
        {
            return new List<string>()
            {
                XY_POINT_TEMPLATE,
                RA_POINT_TEMPLATE,
                SUB_START_TEMPLATE,
                SUB_END_TEMPLATE,
                SUB_CALL_TEMPLATE,
                PATH_START_TEMPLATE,
                PATH_END_TEMPLATE,
                PROGRAM_END_TEMPLATE,
                MAIN_BODY_TEMPLATE,
                PATH_NAME_TEMPLATE,
                HEADER_TEMPLATE,
                SUB_FILE_NAME_TEMPLATE,
                MAIN_FILE_NAME_TEMPLATE,
                GLOBALS_TEMPLATE,
                FIRST_POINT_TEMPLATE,
                LAST_POINT_TEMPLATE
            };
        }
    }

    class DefaultSettings
    {
        // Templates for code generation
        public static string XY_POINT_TEMPLATE_FORMAT 
= @"X{Binding CurrentPoint.X} Y{Binding CurrentPoint.Y}";
        public static string RA_POINT_TEMPLATE_FORMAT
= @"X{Binding CurrentPoint.XYLength} B{Binding CurrentPoint.Angle}";

        public static string SUB_START_TEMPLATE_FORMAT 
= @"o{Binding SubPathName} sub ";
        public static string SUB_END_TEMPLATE_FORMAT
= @"{Binding Gcodes.Rapid_Move} Z {Binding Safeheight} 
o{Binding SubPathName} endsub ";
        public static string SUB_CALL_TEMPLATE_FORMAT
= @"o{Binding SubPathName}  call ";

        public static string PATH_START_TEMPLATE_FORMAT 
= @"(PATH {Binding CurrentPathName}  )
{Binding Gcodes.Absolute_Moves}
{Binding Gcodes.Rapid_Move} Z {Binding Safeheight} 
{Binding Gcodes.Rapid_Move} X {Binding FirstPoint.X} Y {Binding FirstPoint.Y}
{Binding Gcodes.Linear_Move} Z {Binding Cuttingdepth} F {Binding Feedrate}
";
        public static string PATH_END_TEMPLATE_FORMAT
= @"( END PATH {Binding CurrentPathName}  )";

        public static string FIRST_POINT_TEMPLATE_FORMAT
= @"{Binding Gcodes.Rapid_Move} Z {Binding Safeheight}    
{Binding Gcodes.Rapid_Move} X {Binding FirstPoint.X} Y {Binding FirstPoint.Y}
{Binding Gcodes.Linear_Move} Z {Binding Cuttingdepth} F {Binding Feedrate}";

        public static string LAST_POINT_TEMPLATE_FORMAT
= @"{Binding Gcodes.Rapid_Move} Z {Binding Safeheight}";

        public static string PROGRAM_END_TEMPLATE_FORMAT 
= @"{Binding Mcodes.End_of_Program}
( *** End of Program *** )";
 
        public static string MAIN_PROGRAM_BODY_TEMPLATE_FORMAT
= @"(****** Start of Main program *****) ";

        public static string HEADER_TEMPLATE_FORMAT
= @"(-----------------------------------------)
(    Gcode Program                          )
( BY:    OTWB                               )
( ON:    {Binding Now}                      )
( Flags:                                    )
(   Rotary      : {Binding UseRotaryTable}  )
(   Absolute    : {Binding UseAbsoluteMoves})
(   Subroutine  : {Binding UseSubroutine}   )
(   Single File : {Binding UseSingleFile}   )
(                                           )
( Machine Settings                          )
(   Feed rate   : {Binding Feedrate}        )
(   Safe height : {Binding Safeheight}      )
(   Cut depth   : {Binding Cuttingdepth}    )
(   Accuracy    : {Binding DP}  dp          )
(   Num Paths   : {Binding PathCount}       )
(-------------------------------------------)
";
        public static string GLOBALS_TEMPLATE_FORMAT
= @"#<_safeheight> = {Binding Safeheight} 
#<_cutdepth> = {Binding Cuttingdepth}
#<_feedrate> = {Binding Feedrate }
";
        public static string SUB_FILE_NAME_TEMPLATE_FORMAT
 = @"{Binding PatternName}_{Binding CurrentPathIndex}";

        public static string MAIN_FILE_NAME_TEMPLATE_FORMAT
= @"{Binding PatternName}_main";

       

        // values
        public static int DECIMAL_PLACES_VALUE = 3;
        public static int SUB_START_INDEX = 1000;
        public static int SUB_INDEX_INC = 100;

        public static double FEED_RATE_VALUE = 100;
        public static double SAFE_HEIGHT = 5;
        public static double CUTTING_DEPTH = -1;
        public static bool USE_ROTARY_TABLE = false;
        public static bool USE_ABSOLUTE_MOVES = true;
        public static bool USE_SUBROUTINE = true;
        public static bool USE_SINGLE_FILE = true;
        public static bool USE_O_WORDS = true;
        public static string GCODE_FILE_EXT = "ngc";

        // User default settings for Lattice Layout
        public static int REPEAT_X = 1;
        public static int REPEAT_Y = 1;
        public static double TOOL_POSITION = 100.0;
        public static double WORK_PIECE_RADIUS = 250;
        public static double WIDTH = 50.0;
        public static double HEIGHT = 50.0;
        public static double MARGIN = 0;
        public static double OFFSET_X = 0;
        public static double OFFSET_Y = 0;
        public static bool CLIP = false;
        public static bool HYPO = false;
        public static double HYPO_K = 100;

        /// <summary>
        /// Default settings for basic values
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object Value(string name)
        {
            if (name == SettingsNames.DECIMAL_PLACES)
                return DECIMAL_PLACES_VALUE;
            else if (name == SettingsNames.FEED_RATE_VALUE)
                return FEED_RATE_VALUE;
            else if (name == SettingsNames.CUTTING_DEPTH)
                return CUTTING_DEPTH;
            else if (name == SettingsNames.SAFE_HEIGHT)
                return SAFE_HEIGHT;
            else if (name == SettingsNames.USE_ABSOLUTE_MOVES)
                return USE_ABSOLUTE_MOVES;
            else if (name == SettingsNames.USE_ROTARY_TABLE)
                return USE_ROTARY_TABLE;
            else if (name == SettingsNames.USE_SUBROUTINE)
                return USE_SUBROUTINE;
            else if (name == SettingsNames.USE_SINGLE_FILE)
                return USE_SINGLE_FILE;
            else if (name == SettingsNames.USE_NAMED_O_WORDS)
                return USE_O_WORDS;
            else if (name == SettingsNames.SUB_START_INDEX)
                return SUB_START_INDEX;
            else if (name == SettingsNames.SUB_INDEX_INC)
                return SUB_INDEX_INC;
            else if (name == SettingsNames.GCODE_FILE_EXT)
                return GCODE_FILE_EXT;
            else if (name == SettingsNames.REPEAT_X)
                return REPEAT_X;
            else if (name == SettingsNames.REPEAT_Y)
                return REPEAT_Y;
            else if (name == SettingsNames.TOOL_POSITION)
                return TOOL_POSITION;
            else if (name == SettingsNames.WORK_PIECE_RADIUS)
                return WORK_PIECE_RADIUS;
            else if (name == SettingsNames.WIDTH)
                return WIDTH;
            else if (name == SettingsNames.HEIGHT)
                return HEIGHT;
            else if (name == SettingsNames.MARGIN)
                return MARGIN;
            else if (name == SettingsNames.OFFSET_X)
                return OFFSET_X;
            else if (name == SettingsNames.OFFSET_Y)
                return OFFSET_Y;
            else if (name == SettingsNames.CLIP)
                return CLIP;
            else if (name == SettingsNames.HYPO)
                return HYPO;
            else if (name == SettingsNames.HYPO_K)
                return HYPO_K;
            else
                return null;
        }

             
    }
}
