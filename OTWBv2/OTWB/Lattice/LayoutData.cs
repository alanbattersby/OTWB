using OTWB.Common;
using OTWB.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OTWB.Lattice
{
    public class LayoutData : BindableBase
    {
        /// <summary>
        /// These properties have been moved from the lattive
        /// </summary>
        public LayoutData()
        {
            _repeatX = (int)BasicLib.GetSetting(SettingsNames.REPEAT_X);
            _repeatY = (int)BasicLib.GetSetting(SettingsNames.REPEAT_Y);
            _toolposition = (double)BasicLib.GetSetting(SettingsNames.TOOL_POSITION);
            _workPieceRadius = (double)BasicLib.GetSetting(SettingsNames.WORK_PIECE_RADIUS);
            _clipRange = new Range(0, 0.001, _toolposition);
            _width = (double)BasicLib.GetSetting(SettingsNames.WIDTH);
            _height = (double)BasicLib.GetSetting(SettingsNames.HEIGHT);
            _margin = (double)BasicLib.GetSetting(SettingsNames.MARGIN);
            _offsetX = (double)BasicLib.GetSetting(SettingsNames.OFFSET_X);
            _offsetY = (double)BasicLib.GetSetting(SettingsNames.OFFSET_Y);
            _clip = (bool)BasicLib.GetSetting(SettingsNames.CLIP);
            _hypo = (bool)BasicLib.GetSetting(SettingsNames.HYPO);
            _k = (double)BasicLib.GetSetting(SettingsNames.HYPO_K);
        }

        // Now basic data apertaining to this lattice
        // Just accessing the lattice data
        int _repeatX;
        public int RepeatX
        {
            get { return _repeatX; }
            set { SetProperty(ref _repeatX, value); }
        }

        int _repeatY;
        public int RepeatY
        {
            get { return _repeatY; }
            set { SetProperty(ref _repeatY, value); }
        }

        double _toolposition;
        public double ToolPosition
        {
            get { return _toolposition; }
            set { SetProperty(ref _toolposition, value); }
        }

        double _workPieceRadius;
        public double WorkPieceRadius
        {
            get { return _workPieceRadius; }
            set { SetProperty(ref _workPieceRadius, value); }
        }

        Range _clipRange;
        public Range ClipRange
        {
            get { return _clipRange; }
            set { SetProperty(ref _clipRange, value); }
        }

        double _height;
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        double _width;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        double _margin;
        public double Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        double _offsetX;
        public double OffsetX
        {
            get { return _offsetX; }
            set { SetProperty(ref _offsetX, value); }
        }

        double _offsetY;
        public double OffsetY
        {
            get { return _offsetY; }
            set { SetProperty(ref _offsetY, value); }
        }

        bool _clip;
        public bool Clip
        {
            get { return _clip; }
            set { SetProperty(ref _clip, value); }
        }

        bool _hypo;
        public bool Hyper
        {
            get { return _hypo; }
            set { SetProperty(ref _hypo, value); }
        }

        double _k;
        public double K
        {
            get { return _k; }
            set { SetProperty(ref _k, value); }
        }
    }
}
