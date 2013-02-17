using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTWB.Settings
{
    public class SettingsNames
    {
        // for code generation
        public static string CODE_POINT_TEMPLATE = "CODE_POINT_TEMPLATE";
        public static string SUB_START_TEMPLATE = "SubStartTemplate";
        public static string SUB_END_TEMPLATE = "SubEndTemplate";

        public static string PATH_START_TEMPLATE = "PathStartTemplate";
        public static string PATH_END_TEMPLATE = "PathEndTemplate";

        public static string USE_ROTARY_TABLE = "UseRotaryTable";
        public static string USE_ABSOLUTE_MOVES = "UseAbsoluteMoves";
        public static string USE_SUBROUTINE = "UseSubroutine";

        public static string PROGRAM_END_TEMPLATE = "ProgramEndTemplate";
        public static string MAIN_PROGRAM_BODY = "MAIN_PROGRAM_BODY";
        // Non templated strings
        public static string PROGRAM_END_COMMENT = "ProgramEndComment";
        public static string SUB_END_COMMENT = "SubEndComment";
        public static string ABSOLUTE_MODE = "AbsMode";
        public static string RELATIVE_MODE = "RelMode";

        public static string MOVE_TO = "MoveTo";
        public static string LINEAR_MOVE_TO = "LinearMoveTo";

        public static string DECIMAL_PLACES = "DP";
        public static string FEED_RATE_TEMPLATE = "FeedrateTemplate";
        public static string FEED_RATE_VALUE = "FeedrateValue";
        public static string PATH_NAME_TEMPLATE = "PathNameTemplate";

    }

    class DefaultSettings
    {
        // for code generation
        public static string CODE_POINT_TEMPLATE = "X{0} Y{1}";

        public static string SUB_START_TEMPLATE = "o<{0}> sub";
        public static string SUB_END_TEMPLATE = "o<{0}> endsub";

        public static string PATH_START_TEMPLATE = "PATH {0}>";
        public static string PATH_END_TEMPLATE = "END PATH {0}>";

        public static bool USE_ROTARY_TABLE = false;
        public static bool USE_ABSOLUTE_MOVES = true;
        public static bool USE_SUBROUTINE = true;

        public static string PROGRAM_END_TEMPLATE = "M2   {0}";
        public static string PROGRAM_END_COMMENT = "( *** End of Program *** )";
        public static string SUB_END_COMMENT = "( *** End of Subroutine *** )";
        public static string FEED_RATE_TEMPLATE = "F{0}";
        public static string PATH_NAME_TEMPLATE = "{0}_{1}";
        
        public static List<string> MAIN_PROGRAM_BODY_TEMPLATE
            = new List<string>(){" G0 {FirstPoint} {Feedrate} ",  "o<{Pathname}> call", "M2 "};
        // standard G codes
        public static string ABSOLUTE_MODE = "G90 ";
        public static string RELATIVE_MODE = "G91 ";
        public static string MOVE_TO = "G0 ";
        public static string LINEAR_MOVE_TO = "G1 ";
      

        public static int DECIMAL_PLACES_VALUE = 3;
        public static double FEED_RATE_VALUE = 100;

        public static object Value(string name)
        {
            if (name == SettingsNames.ABSOLUTE_MODE)
                return ABSOLUTE_MODE;
            else if (name == SettingsNames.CODE_POINT_TEMPLATE)
                return CODE_POINT_TEMPLATE;
            else if (name == SettingsNames.DECIMAL_PLACES)
                return DECIMAL_PLACES_VALUE;
            else if (name == SettingsNames.FEED_RATE_VALUE)
                return FEED_RATE_VALUE;
            else if (name == FEED_RATE_TEMPLATE)
                return FEED_RATE_TEMPLATE;
            else if (name == SettingsNames.LINEAR_MOVE_TO)
                return LINEAR_MOVE_TO;
            else if (name == SettingsNames.MOVE_TO)
                return MOVE_TO;
            else if (name == SettingsNames.PATH_END_TEMPLATE)
                return PATH_END_TEMPLATE;
            else if (name == SettingsNames.PATH_NAME_TEMPLATE)
                return PATH_NAME_TEMPLATE;
            else if (name == SettingsNames.PATH_START_TEMPLATE)
                return PATH_START_TEMPLATE;
            else if (name == SettingsNames.PROGRAM_END_COMMENT)
                return PROGRAM_END_COMMENT;
            else if (name == SettingsNames.PROGRAM_END_TEMPLATE)
                return PROGRAM_END_TEMPLATE;
            else if (name == SettingsNames.RELATIVE_MODE)
                return RELATIVE_MODE;
            else if (name == SettingsNames.SUB_END_COMMENT)
                return SUB_END_COMMENT;
            else if (name == SettingsNames.SUB_END_TEMPLATE)
                return SUB_END_TEMPLATE;
            else if (name == SettingsNames.SUB_START_TEMPLATE)
                return SUB_START_TEMPLATE;
            else if (name == SettingsNames.USE_ABSOLUTE_MOVES)
                return USE_ABSOLUTE_MOVES;
            else if (name == SettingsNames.USE_ROTARY_TABLE)
                return USE_ROTARY_TABLE;
            else if (name == SettingsNames.USE_SUBROUTINE)
                return USE_SUBROUTINE;
            else
                return null;
        }
    }
}
