using Geometric_Chuck.Common;
using OTWB.Common;
using OTWB.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OTWB.CodeGeneration
{
    public class CodeGenTemplates : BindableBase
    {
        List<BindableCodeTemplate> _allTemplates;
        [XmlIgnore]
        public List<BindableCodeTemplate> AllTemplates
        {
            get { return _allTemplates; }
            set { SetProperty(ref _allTemplates, value); }
        }

        BindableCodeTemplate _headerTemplate;
        public BindableCodeTemplate Header_Template
        {
            get { return _headerTemplate; }
            set { SetProperty(ref _headerTemplate, value); }
        }

        BindableCodeTemplate _pointTemplate;
        public BindableCodeTemplate XY_Point_Template
        {
            get { return _pointTemplate; }
            set { SetProperty(ref _pointTemplate, value); }
        }

        BindableCodeTemplate _rpointTemplate;
        public BindableCodeTemplate RA_Point_Template
        {
            get { return _rpointTemplate; }
            set { SetProperty(ref _rpointTemplate, value); }
        }

        BindableCodeTemplate _subStartTemplate;
        public BindableCodeTemplate SubStartTemplate
        {
            get { return _subStartTemplate; }
            set { SetProperty(ref _subStartTemplate, value); }
        }

        BindableCodeTemplate _subEndTemplate;
        public BindableCodeTemplate SubEndTemplate
        {
            get { return _subEndTemplate; }
            set { SetProperty(ref _subEndTemplate, value); }
        }

        BindableCodeTemplate _pathStartTemplate;
        public BindableCodeTemplate PathStartTemplate
        {
            get { return _pathStartTemplate; }
            set { SetProperty(ref _pathStartTemplate, value);  }
        }

        BindableCodeTemplate _pathEndTemplate;
        public BindableCodeTemplate PathEndTemplate
        {
            get { return _pathEndTemplate;  }
            set { SetProperty(ref _pathEndTemplate, value); }
        }

       BindableCodeTemplate _programEndTemplate;
        public BindableCodeTemplate ProgramEndTemplate
        {
            get { return _programEndTemplate; }
            set { SetProperty(ref _programEndTemplate, value); }
        }

        BindableCodeTemplate _sub_filename_Template;
        public BindableCodeTemplate SubFilenameTemplate
        {
            get { return _sub_filename_Template; }
            set { SetProperty(ref _sub_filename_Template, value); }
        }

        BindableCodeTemplate _main_filename_Template;
        public BindableCodeTemplate MainFilenameTemplate
        {
            get { return _main_filename_Template; }
            set { SetProperty(ref _main_filename_Template, value); }
        }

        // Non Templated strings
        BindableCodeTemplate _programEndComment;
        public BindableCodeTemplate ProgramEndComment
        {
            get { return _programEndComment; }
            set { SetProperty(ref _programEndComment, value); }
        }

        BindableCodeTemplate _subEndComment;
        public BindableCodeTemplate SubEndComment
        {
            get { return _subEndComment; }
            set { SetProperty(ref _subEndComment, value); }
        }

        BindableCodeTemplate _mainProgramTemplate;
        public BindableCodeTemplate MainProgramTemplate
        {
            get { return _mainProgramTemplate; }
            set { SetProperty(ref _mainProgramTemplate, value); }
        }

        // Gcode Constants
     
        BindableCodeTemplate _pathNameTemplate;
        public BindableCodeTemplate PathNameTemplate
        {
            get { return _pathNameTemplate; }
            set { SetProperty(ref _pathNameTemplate, value); }
        }

        public CodeGenTemplates()
        {
            _allTemplates = new List<BindableCodeTemplate>();

            BindableCodeTemplate template = new BindableCodeTemplate(SettingsNames.HEADER_TEMPLATE,
                                            DefaultSettings.HEADER_TEMPLATE);
            Header_Template = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.XY_POINT_TEMPLATE,
                                             DefaultSettings.XY_POINT_TEMPLATE_FORMAT);
            XY_Point_Template = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.RA_POINT_TEMPLATE,
                                            DefaultSettings.RA_POINT_TEMPLATE_FORMAT);
            RA_Point_Template = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.SUB_START_TEMPLATE,
                                            DefaultSettings.SUB_START_TEMPLATE_FORMAT);
            SubStartTemplate = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.SUB_END_TEMPLATE,
                                            DefaultSettings.SUB_END_TEMPLATE_FORMAT);
            SubEndTemplate = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.PATH_START_TEMPLATE,
                                            DefaultSettings.PATH_START_TEMPLATE_FORMAT);
            PathStartTemplate = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.PROGRAM_END_TEMPLATE,
                                            DefaultSettings.PATH_END_TEMPLATE_FORMAT);
            PathEndTemplate = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.PATH_NAME_TEMPLATE,
                                            DefaultSettings.PATH_NAME_TEMPLATE_FORMAT);
            PathNameTemplate = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.SUB_END_COMMENT_TEMPLATE,
                                            DefaultSettings.SUB_END_COMMENT_FORMAT);
            SubEndComment = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.PROGRAM_END_COMMENT_TEMPLATE,
                                                DefaultSettings.PROGRAM_END_COMMENT_FORMAT);
            ProgramEndComment = template;
            _allTemplates.Add(template);

             template = new BindableCodeTemplate(SettingsNames.MAIN_BODY_TEMPLATE,
                                          DefaultSettings.MAIN_PROGRAM_BODY_TEMPLATE);
            MainProgramTemplate= template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.SUB_FILE_NAME_TEMPLATE,
                                         DefaultSettings.SUB_FILE_NAME_TEMPLATE);
            SubFilenameTemplate = template;
            _allTemplates.Add(template);

            template = new BindableCodeTemplate(SettingsNames.MAIN_FILE_NAME_TEMPLATE,
                                         DefaultSettings.MAIN_FILE_NAME_TEMPLATE);
            MainFilenameTemplate = template;
            _allTemplates.Add(template);
        }

        public BindableCodeTemplate Lookup(string name)
        {
            try
            {
                BindableCodeTemplate tmpl = AllTemplates.First<BindableCodeTemplate>(
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
