using Geometric_Chuck.Common;
using OTWB.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OTWB.CodeGeneration
{
    public class CodeGenTemplates : BindableBase
    {
        List<CodeTemplate> _allTemplates;
        public List<CodeTemplate> AllTemplates
        {
            get { return _allTemplates; }
            set { SetProperty(ref _allTemplates, value); }
        }

        CodeTemplate _pointTemplate;
        public CodeTemplate Point_Template
        {
            get { return _pointTemplate; }
            set { SetProperty(ref _pointTemplate, value); }
        }

        CodeTemplate _subStartTemplate;
        public CodeTemplate SubStartTemplate
        {
            get { return _subStartTemplate; }
            set { SetProperty(ref _subStartTemplate, value); }
        }

        CodeTemplate _subEndTemplate;
        public CodeTemplate SubEndTemplate
        {
            get { return _subEndTemplate; }
            set { SetProperty(ref _subEndTemplate, value); }
        }

        CodeTemplate _pathStartTemplate;
        public CodeTemplate PathStartTemplate
        {
            get { return _pathStartTemplate; }
            set { SetProperty(ref _pathStartTemplate, value);  }
        }

        CodeTemplate _pathEndTemplate;
        public CodeTemplate PathEndTemplate
        {
            get { return _pathEndTemplate;  }
            set { SetProperty(ref _pathEndTemplate, value); }
        }

       CodeTemplate _programEndTemplate;
        public CodeTemplate ProgramEndTemplate
        {
            get { return _programEndTemplate; }
            set { SetProperty(ref _programEndTemplate, value); }
        }


        // Non Templated strings
        CodeTemplate _programEndComment;
        public CodeTemplate ProgramEndComment
        {
            get { return _programEndComment; }
            set { SetProperty(ref _programEndComment, value); }
        }

        CodeTemplate _subEndComment;
        public CodeTemplate SubEndComment
        {
            get { return _subEndComment; }
            set { SetProperty(ref _subEndComment, value); }
        }

        CodeTemplate _mainProgramTemplate;
        public CodeTemplate MainProgramTemplate
        {
            get { return _mainProgramTemplate; }
            set { SetProperty(ref _mainProgramTemplate, value); }
        }

        // Gcode Constants
        CodeTemplate _absoluteMode;
        public CodeTemplate AbsoluteModeTemplate
        {
            get { return _absoluteMode; }
            set { SetProperty(ref _absoluteMode, value); }
        }

        CodeTemplate _relativeMode;
        public CodeTemplate RelativeModeTemplate
        {
            get { return _relativeMode; }
            set { SetProperty(ref _relativeMode, value); }
        }

        CodeTemplate _linearMoveTo;
        public CodeTemplate LinearMoveToTemplate
        {
            get { return _linearMoveTo; }
            set { SetProperty(ref _linearMoveTo, value); }
        }

        CodeTemplate _moveTo;
        public CodeTemplate MoveToTemplate
        {
            get { return _moveTo; }
            set { SetProperty(ref _moveTo, value); }
        }

        CodeTemplate _feedRate;
        public CodeTemplate FeedRateTemplate
        {
            get { return _feedRate; }
            set { SetProperty(ref _feedRate, value); }
        }

        CodeTemplate _pathNameTemplate;
        public CodeTemplate PathNameTemplate
        {
            get { return _pathNameTemplate; }
            set { SetProperty(ref _pathNameTemplate, value); }
        }

        public CodeGenTemplates()
        {
            _allTemplates = new List<CodeTemplate>();
            CodeTemplate template = new CodeTemplate(SettingsNames.CODE_POINT_TEMPLATE,
                                             DefaultSettings.CODE_POINT_TEMPLATE);
            Point_Template = template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.SUB_START_TEMPLATE,
                                            DefaultSettings.SUB_START_TEMPLATE);
            SubStartTemplate = template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.SUB_END_TEMPLATE,
                                            DefaultSettings.SUB_END_TEMPLATE);
            SubEndTemplate = template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.PATH_START_TEMPLATE,
                                            DefaultSettings.PATH_START_TEMPLATE);
            PathStartTemplate = template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.PROGRAM_END_TEMPLATE,
                                            DefaultSettings.PATH_END_TEMPLATE);
            PathEndTemplate = template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.PATH_NAME_TEMPLATE,
                                            DefaultSettings.PATH_NAME_TEMPLATE);
            PathNameTemplate = template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.SUB_END_COMMENT,
                                            DefaultSettings.SUB_END_COMMENT);
            SubEndComment = template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.PROGRAM_END_COMMENT,
                                                DefaultSettings.PROGRAM_END_COMMENT);
            ProgramEndComment = template;
            _allTemplates.Add(template);

             template = new CodeTemplate(SettingsNames.MAIN_PROGRAM_BODY,
                                          DefaultSettings.MAIN_PROGRAM_BODY_TEMPLATE);
            MainProgramTemplate= template;
            _allTemplates.Add(template);

             template = new CodeTemplate(SettingsNames.ABSOLUTE_MODE,
                                                DefaultSettings.ABSOLUTE_MODE);
            AbsoluteModeTemplate= template;
            _allTemplates.Add(template);
            
             template = new CodeTemplate(SettingsNames.RELATIVE_MODE,
                                                DefaultSettings.RELATIVE_MODE);

            RelativeModeTemplate= template;
            _allTemplates.Add(template);

             template = new CodeTemplate(SettingsNames.LINEAR_MOVE_TO,
                                                DefaultSettings.LINEAR_MOVE_TO);

            LinearMoveToTemplate= template;
            _allTemplates.Add(template);
             template = new CodeTemplate(SettingsNames.MOVE_TO,
                                                DefaultSettings.MOVE_TO);

            MoveToTemplate= template;
            _allTemplates.Add(template);

            template = new CodeTemplate(SettingsNames.FEED_RATE_TEMPLATE,
                                                DefaultSettings.FEED_RATE_TEMPLATE);

            FeedRateTemplate= template;
            _allTemplates.Add(template);
        }

        public CodeTemplate Lookup(string name)
        {
            try
            {
                CodeTemplate tmpl = AllTemplates.First<CodeTemplate>(
                                     t => t.Name == name);
                return tmpl;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
