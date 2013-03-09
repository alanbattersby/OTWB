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

        // int values
        public static string DECIMAL_PLACES = "DP";

        // double values
        public static string FEED_RATE_VALUE = "FeedrateValue";
        public static string SAFE_HEIGHT = "SafeHeight";
        public static string CUTTING_DEPTH = "CuttingDepth";

        // string templates for code generation
        public static string XY_POINT_TEMPLATE = "XY_POINT_TEMPLATE";
        public static string RA_POINT_TEMPLATE = "RA_POINT_TEMPLATE";

        public static string SUB_START_TEMPLATE = "SUB_START_TEMPLATE";
        public static string SUB_END_TEMPLATE = "SUB_END_TEMPLATE";
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
                PATH_START_TEMPLATE,
                PATH_END_TEMPLATE,
                PROGRAM_END_TEMPLATE,
                MAIN_BODY_TEMPLATE,
                PATH_NAME_TEMPLATE,
                HEADER_TEMPLATE,
                SUB_FILE_NAME_TEMPLATE,
                MAIN_FILE_NAME_TEMPLATE,
                GLOBALS_TEMPLATE
            };
        }
    }

    class DefaultSettings
    {
        // Templates for code generation
        public static string XY_POINT_TEMPLATE_FORMAT 
= @"X{Binding CurrentPoint.X} Y{Binding CurrentPoint.Y}";
        public static string RA_POINT_TEMPLATE_FORMAT
= @"X{Binding CurrentPoint.Radius} B{Binding CurrentPoint.Angle}";

        public static string SUB_START_TEMPLATE_FORMAT 
= @"o<{Binding SubPathName}> sub
{Binding Gcodes.Absolute_Moves}
{Binding Gcodes.Rapid_Move} Z {Binding Safeheight} 
{Binding Gcodes.Rapid_Move} X {Binding FirstPoint.X} Y {Binding FirstPoint.Y}
{Binding Gcodes.Linear_Move} Z {Binding CuttingDepth} F {Binding Feedrate}
";
        public static string SUB_END_TEMPLATE_FORMAT
= @"{Binding Gcodes.Rapid_Move} Z {Binding Safeheight} 
o<{Binding SubPathName}> endsub
";
        public static string PATH_START_TEMPLATE_FORMAT 
= @"(PATH {Binding CurrentPathName}  )
{Binding Gcodes.Absolute_Moves}
{Binding Gcodes.Rapid_Move} Z {Binding Safeheight} 
{Binding Gcodes.Rapid_Move} X {Binding FirstPoint.X} Y {Binding FirstPoint.Y}
{Binding Gcodes.Linear_Move} Z {Binding Cuttingdepth} F {Binding Feedrate}
";
        public static string PATH_END_TEMPLATE_FORMAT
= @" {Binding Gcodes.Rapid_Move} Z {Binding Safeheight} 
( END PATH {Binding CurrentPathName}  )
";    
        public static string PROGRAM_END_TEMPLATE_FORMAT 
= @"{Binding Mcodes.End_of_Program}
( *** End of Program *** )
";
        //public static string PATH_NAME_TEMPLATE_FORMAT = "{0}_{1}";       
        public static string MAIN_PROGRAM_BODY_TEMPLATE_FORMAT
= @"(****** Start of Main program *****) 
o<{Binding SubPathName}> call 
";

        public static string HEADER_TEMPLATE_FORMAT
= @"(-----------------------------------------)
(    Gcode Program                          )
( BY:    OTWB                               )
( ON:    {Binding Now}                      )
( Flags:                                    )
(   Rotary      : {Binding UseRotary}       )
(   Absolute    : {Binding UseAbsolute}     )
(   Subroutine  : {Binding UseSub}          )
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
";
        public static string SUB_FILE_NAME_TEMPLATE_FORMAT
 = @"{Binding PatternName}_{Binding CurrentPathIndex}.ngc";

        public static string MAIN_FILE_NAME_TEMPLATE_FORMAT
= @"{Binding PatternName}_main.ngc";
 
        // values
        public static int DECIMAL_PLACES_VALUE = 3;
        public static double FEED_RATE_VALUE = 100;
        public static double SAFE_HEIGHT = 5;
        public static double CUTTING_DEPTH = -1;
        public static bool USE_ROTARY_TABLE = false;
        public static bool USE_ABSOLUTE_MOVES = true;
        public static bool USE_SUBROUTINE = true;
        public static bool USE_SINGLE_FILE = true;

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
            else
                return null;
        }

        //public static string TemplateFormat(string name)
        //{
        //     if (name == SettingsNames.ABSOLUTE_MODE_TEMPLATE)
        //        return ABSOLUTE_MODE_FORMAT;
        //    else if (name == SettingsNames.CODE_POINT_TEMPLATE)
        //        return CODE_POINT_TEMPLATE_FORMAT;
        //    else if (name == FEED_RATE_TEMPLATE_FORMAT)
        //        return FEED_RATE_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.LINEAR_MOVE_TO_TEMPLATE)
        //        return LINEAR_MOVE_TO_FORMAT;
        //    else if (name == SettingsNames.MOVE_TO_TEMPLATE)
        //        return MOVE_TO_FORMAT;
        //    else if (name == SettingsNames.PATH_END_TEMPLATE)
        //        return PATH_END_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.PATH_NAME_TEMPLATE)
        //        return PATH_NAME_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.PATH_START_TEMPLATE)
        //        return PATH_START_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.PROGRAM_END_COMMENT_TEMPLATE)
        //        return PROGRAM_END_COMMENT_FORMAT;
        //    else if (name == SettingsNames.PROGRAM_END_TEMPLATE)
        //        return PROGRAM_END_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.RELATIVE_MODE_TEMPLATE)
        //        return RELATIVE_MODE_FORMAT;
        //    else if (name == SettingsNames.SUB_END_COMMENT_TEMPLATE)
        //        return SUB_END_COMMENT_FORMAT;
        //    else if (name == SettingsNames.SUB_END_TEMPLATE)
        //        return SUB_END_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.SUB_START_TEMPLATE)
        //        return SUB_START_TEMPLATE_FORMAT;
        //    else
        //        return string.Empty;
        //}

        //public static string DefaultPath(string name)
        //{
        //    if (name == SettingsNames.ABSOLUTE_MODE_TEMPLATE)
        //        return string.Empty;
        //    else if (name == SettingsNames.CODE_POINT_TEMPLATE)
        //        return CODE_POINT_TEMPLATE_FORMAT;
        //    else if (name == FEED_RATE_TEMPLATE_FORMAT)
        //        return FEED_RATE_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.LINEAR_MOVE_TO_TEMPLATE)
        //        return LINEAR_MOVE_TO_FORMAT;
        //    else if (name == SettingsNames.MOVE_TO_TEMPLATE)
        //        return MOVE_TO_FORMAT;
        //    else if (name == SettingsNames.PATH_END_TEMPLATE)
        //        return PATH_END_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.PATH_NAME_TEMPLATE)
        //        return PATH_NAME_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.PATH_START_TEMPLATE)
        //        return PATH_START_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.PROGRAM_END_COMMENT_TEMPLATE)
        //        return PROGRAM_END_COMMENT_FORMAT;
        //    else if (name == SettingsNames.PROGRAM_END_TEMPLATE)
        //        return PROGRAM_END_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.RELATIVE_MODE_TEMPLATE)
        //        return RELATIVE_MODE_FORMAT;
        //    else if (name == SettingsNames.SUB_END_COMMENT_TEMPLATE)
        //        return SUB_END_COMMENT_FORMAT;
        //    else if (name == SettingsNames.SUB_END_TEMPLATE)
        //        return SUB_END_TEMPLATE_FORMAT;
        //    else if (name == SettingsNames.SUB_START_TEMPLATE)
        //        return SUB_START_TEMPLATE_FORMAT;
        //    else
        //        return string.Empty;
        //}
    }
}
