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
    public class TemplateCollection : BindableBase
    {
        [XmlIgnore]
        public List<BindableCodeTemplate> AllTemplates
        {
            get 
            {
               return new List<BindableCodeTemplate>()
                    {
                        FirstPointTemplate,
                        Globals_Template ,
                        Header_Template ,
                        LastPointTemplate,
                        MainFilenameTemplate,
                        MainProgramTemplate,
                        PathEndTemplate, 
                        PathStartTemplate ,
                        ProgramEndTemplate,
                        RA_Point_Template ,
                        SubEndTemplate,
                        SubFilenameTemplate,
                        SubStartTemplate,
                        XY_Point_Template 
                    };
            }
           
        }

        BindableCodeTemplate _headerTemplate;
        public BindableCodeTemplate Header_Template
        {
            get { return _headerTemplate; }
            set { SetProperty(ref _headerTemplate, value); }
        }

        BindableCodeTemplate _globalsTemplate;
        public BindableCodeTemplate Globals_Template
        {
            get { return _globalsTemplate; }
            set { SetProperty(ref _globalsTemplate, value); }
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

        BindableCodeTemplate _subCallTemplate;
        public BindableCodeTemplate SubCallTemplate
        {
            get { return _subCallTemplate; }
            set { SetProperty(ref _subCallTemplate, value); }
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

        BindableCodeTemplate _mainProgramTemplate;
        public BindableCodeTemplate MainProgramTemplate
        {
            get { return _mainProgramTemplate; }
            set { SetProperty(ref _mainProgramTemplate, value); }
        }
   
        BindableCodeTemplate _pathNameTemplate;
        public BindableCodeTemplate PathNameTemplate
        {
            get { return _pathNameTemplate; }
            set { SetProperty(ref _pathNameTemplate, value); }
        }

        BindableCodeTemplate _firstpointTemplate;
        public BindableCodeTemplate FirstPointTemplate
        {
            get { return _firstpointTemplate; }
            set { SetProperty(ref _firstpointTemplate, value); }
        }

        BindableCodeTemplate _lastpointTemplate;
        public BindableCodeTemplate LastPointTemplate
        {
            get { return _lastpointTemplate; }
            set { SetProperty(ref _lastpointTemplate, value); }
        }

        public TemplateCollection()
        {
           Header_Template = new BindableCodeTemplate(SettingsNames.HEADER_TEMPLATE,
                                            DefaultSettings.HEADER_TEMPLATE_FORMAT);
           Globals_Template = new BindableCodeTemplate(SettingsNames.GLOBALS_TEMPLATE,
                                            DefaultSettings.GLOBALS_TEMPLATE_FORMAT);
           XY_Point_Template = new BindableCodeTemplate(SettingsNames.XY_POINT_TEMPLATE,
                                             DefaultSettings.XY_POINT_TEMPLATE_FORMAT);
           RA_Point_Template = new BindableCodeTemplate(SettingsNames.RA_POINT_TEMPLATE,
                                            DefaultSettings.RA_POINT_TEMPLATE_FORMAT);         
 
            SubStartTemplate = new BindableCodeTemplate(SettingsNames.SUB_START_TEMPLATE,
                                            DefaultSettings.SUB_START_TEMPLATE_FORMAT);       
           SubEndTemplate = new BindableCodeTemplate(SettingsNames.SUB_END_TEMPLATE,
                                            DefaultSettings.SUB_END_TEMPLATE_FORMAT);
           SubCallTemplate = new BindableCodeTemplate(SettingsNames.SUB_CALL_TEMPLATE,
                                          DefaultSettings.SUB_CALL_TEMPLATE_FORMAT);    

           PathStartTemplate = new BindableCodeTemplate(SettingsNames.PATH_START_TEMPLATE,
                                            DefaultSettings.PATH_START_TEMPLATE_FORMAT);         
            PathEndTemplate = new BindableCodeTemplate(SettingsNames.PROGRAM_END_TEMPLATE,
                                            DefaultSettings.PATH_END_TEMPLATE_FORMAT);
           MainProgramTemplate= new BindableCodeTemplate(SettingsNames.MAIN_BODY_TEMPLATE,
                                          DefaultSettings.MAIN_PROGRAM_BODY_TEMPLATE_FORMAT);
            ProgramEndTemplate = new BindableCodeTemplate(SettingsNames.PROGRAM_END_TEMPLATE,
                                        DefaultSettings.PROGRAM_END_TEMPLATE_FORMAT);
            SubFilenameTemplate= new BindableCodeTemplate(SettingsNames.SUB_FILE_NAME_TEMPLATE,
                                         DefaultSettings.SUB_FILE_NAME_TEMPLATE_FORMAT);
             MainFilenameTemplate= new BindableCodeTemplate(SettingsNames.MAIN_FILE_NAME_TEMPLATE,
                                         DefaultSettings.MAIN_FILE_NAME_TEMPLATE_FORMAT);
           FirstPointTemplate= new BindableCodeTemplate(SettingsNames.FIRST_POINT_TEMPLATE,
                                         DefaultSettings.FIRST_POINT_TEMPLATE_FORMAT);
            LastPointTemplate= new BindableCodeTemplate(SettingsNames.LAST_POINT_TEMPLATE,
                                        DefaultSettings.LAST_POINT_TEMPLATE_FORMAT);
            
        }

       
    }

  }
