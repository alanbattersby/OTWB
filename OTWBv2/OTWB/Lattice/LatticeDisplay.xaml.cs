using OTWB.Common;
using OTWB.Coordinates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OTWB.Lattice
{
    public sealed partial class LatticeDisplay : UserControl
    {
        enum MOVEDPOINT {START,MIDDLE,END};
        const int lwidth = 3;

        CartesianGrid _grid;
        string lastHilightName;

        Brush normalBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);
        Brush hiliteBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        string namefmt = "NAME{0}";

        Point pointerPosition;

        MOVEDPOINT movepoint;

        public LatticeDisplay()
        {
            this.InitializeComponent();
            lastHilightName = string.Empty;
            //LatticeCanvas.Width = LatticeClip.Width;
            //LatticeCanvas.Height = LatticeClip.Height;
        }

        LatticeData _lat;
        public LatticeData Lattice
        {
            get { return _lat; }
            set
            {
                _lat = value;
                if (_lat == null) return;
                _lat.PropertyChanged += _lat_PropertyChanged;
                _lat.LineChanged += _lat_LineChanged;
                _lat.LineRemoved += _lat_LineRemoved;
                Range x = new Range(0,1,_lat.Columns - 1);
                Range y = new Range(0,1,_lat.Rows - 1);
                _grid = new CartesianGrid(x,y);
                _grid.Name = "LATTICE_GRID";
               
                Show();
            }
        }

        void _lat_LineRemoved(object sender, Line2D ll)
        {
            UIElement toremove = null;
            foreach (UIElement uie in LatticeCanvas.Children)
            {
                if ((uie is Shape) && (uie as Shape).Name.StartsWith(ll.Name))
                {
                    toremove = uie;
                    break;
                }
            }

            if (toremove != null)
                LatticeCanvas.Children.Remove(toremove);
        }

        void _lat_LineChanged(object sender, Line2D ll)
        {
            UIElement toremove = null;
            foreach (UIElement uie in LatticeCanvas.Children)
            {
                if ((uie is Shape) && (uie as Shape).Name.StartsWith(ll.Name))
                {
                    toremove = uie;
                    break;
                }
            }

            if (toremove != null)
                LatticeCanvas.Children.Remove(toremove);

            LatticeCanvas.Children.Add(ReCreateLineWithIndex(ll));
        }

        Line ReCreateLineWithIndex(Line2D ll)
        {
            Tuple<double, double> t = ScaleFactors;
            double st = lwidth / Math.Min(t.Item1, t.Item2);
            Line l = ll.Visual;
            AttachPointerEvents(ref l);
            l.Stroke = (ll.Name == lastHilightName) ? hiliteBrush : normalBrush;
            l.StrokeThickness = st;
            return l;
        }

        Line CreateLine(Line2D ll)
        {
            Line l = ll.Visual;
            AttachPointerEvents(ref l);
            l.Stroke = (ll.Name == lastHilightName) ? hiliteBrush : normalBrush;
            return l;
        }

        void AttachPointerEvents(ref Line l)
        {
            l.PointerPressed += l_PointerPressed;
            l.PointerMoved += l_PointerMoved;
            l.PointerReleased += l_PointerReleased;
            l.PointerCanceled += l_PointerCanceled;
            l.PointerCaptureLost += l_PointerCaptureLost;
        }

        void DetachPointerEvents(ref Line l)
        {
            l.PointerPressed -= l_PointerPressed;
            l.PointerMoved -= l_PointerMoved;
            l.PointerReleased -= l_PointerReleased;
            l.PointerCanceled -= l_PointerCanceled;
            l.PointerCaptureLost -= l_PointerCaptureLost;
        }

        void l_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            l.ReleasePointerCapture(e.Pointer);
            
            Hilight(l.Name, false);
            lastHilightName = string.Empty;
        }

        double distance(double x1, double y1, double x2, double y2)
        {
            double xdiff = x2 - x1;
            double ydiff = y2 - y1;
            return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }

        void l_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            l.ReleasePointerCapture(e.Pointer);
            Hilight(l.Name, false);
            lastHilightName = string.Empty;

        }

        void l_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            l.ReleasePointerCapture(e.Pointer);
            Hilight(l.Name, false);
            lastHilightName = string.Empty;

            Line2D ll = _lat.Lines.First<Line2D>(lline =>lline.Name == l.Name);
            double lc = _lat.Columns - 1;
            double lr = _lat.Rows - 1;

            if (l.X1 < 0)
                ll.X1 = 0;
            else
                ll.X1 = Math.Min(Math.Round(l.X1),lc );
            if (l.Y1 < 0)
                l.Y1 = 0;
            else
                ll.Y1 = Math.Min(Math.Round(l.Y1),lr );
            if (l.X2 < 0)
                l.X2 = 0;
            else
                ll.X2 = Math.Min(Math.Round(l.X2), lc);
            if (l.Y2 < 0)
                l.Y2 = 0;
            else
                ll.Y2 = Math.Min(Math.Round(l.Y2), lr);
            
        }

        void l_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            if (l.PointerCaptures != null && l.PointerCaptures.Count == 1)
            {
                PointerPoint p = e.GetCurrentPoint(LatticeCanvas);
                Tuple<double, double> t = ScaleFactors;
                double x = p.Position.X; // t.Item1;
                double y = p.Position.Y; // t.Item2;
                if (movepoint == MOVEDPOINT.START)
                {
                    l.X1 += x - pointerPosition.X;
                    l.Y1 += y - pointerPosition.Y;
               
                }
                else if (movepoint == MOVEDPOINT.END)
                {
                    l.X2 += x - pointerPosition.X;
                    l.Y2 += y - pointerPosition.Y;
                }
                else  // move both
                {
                    l.X1 += x - pointerPosition.X;
                    l.Y1 += y - pointerPosition.Y;
                    l.X2 += x - pointerPosition.X;
                    l.Y2 += y - pointerPosition.Y;
                }
               
                
                pointerPosition.X = x;
                pointerPosition.Y = y;
            }
        }

        void l_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            Point pntrposn = e.GetCurrentPoint(LatticeCanvas).Position;
            if ( distance(l.X1, l.Y1, pntrposn.X, pntrposn.Y) < 0.2 )
                movepoint = MOVEDPOINT.START;
            else if ( distance(l.X2,l.Y2, pntrposn.X, pntrposn.Y) < 0.2)
                movepoint = MOVEDPOINT.END;
            else
                movepoint = MOVEDPOINT.MIDDLE;
            
            Tuple<double, double> t = ScaleFactors;
            l.CapturePointer(e.Pointer);
            pointerPosition = new Point();
            pointerPosition.X = pntrposn.X; // t.Item1;
            pointerPosition.Y = pntrposn.Y; // t.Item2;
            Hilight(l.Name, true);
        }

        void _lat_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Columns")
                _grid.Dim1 = new Range(0, 1, _lat.Columns - 1);
            if (e.PropertyName == "Rows")
                _grid.Dim2 = new Range(0, 1, _lat.Rows - 1);
            _grid.Update();
            Show();       
        }

        public void Show()
        {
            Clear();
            Tuple<double,double> t = ScaleFactors;
            double st = 1 / Math.Min(t.Item1, t.Item2);
            if (double.IsInfinity(st) || double.IsNaN(st)) return;
            ChangeScaleFactorTo(t);
            Shape s = _grid.Outline;
            s.StrokeThickness = st;
            LatticeCanvas.Children.Add(s);
            foreach (Line2D ll in _lat.Lines)
            {
                Line l = CreateLine(ll);             
                l.StrokeThickness = lwidth * st;
                LatticeCanvas.Children.Add(l);
            }
        }

        public void Clear()
        {
            this.LatticeCanvas.Children.Clear();
        }

        private Tuple<double,double>  ScaleFactors
        {
            get
            {
                double width = (this.LatticeBorder.ActualWidth <= 0)
                                   ? LatticeBorder.MinWidth : LatticeBorder.ActualWidth;
                width -= LatticeBorder.BorderThickness.Left + LatticeBorder.BorderThickness.Right
                           + LatticeBorder.Margin.Left + LatticeBorder.Margin.Right;


                double height = (LatticeBorder.ActualHeight <= 0)
                              ? LatticeBorder.MinHeight : LatticeBorder.ActualHeight;
                height -= LatticeBorder.BorderThickness.Top + LatticeBorder.BorderThickness.Bottom
                        + LatticeBorder.Margin.Top + LatticeBorder.Margin.Bottom;

                return new Tuple<double,double>
                    (width / _grid.Dim1.End, height / _grid.Dim2.End);

            }
        }

        public void ChangeScaleFactorTo(Tuple<double,double> sfs)
        {
            try
            {

                if (sfs.Item1 > 0 && sfs.Item2 > 0 )
                {
                    (LatticeCanvas.RenderTransform as CompositeTransform).ScaleX = sfs.Item1;
                    (LatticeCanvas.RenderTransform as CompositeTransform).ScaleY = sfs.Item2;
                    foreach (Shape uie in LatticeCanvas.Children)
                    {
                        if (!(uie is Polygon))
                            uie.StrokeThickness = 1 / Math.Min(sfs.Item1, sfs.Item2);
                    }
                }
            }
            catch 
            {

            }
        }

        private void LatticeCanvas_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            int delta = e.GetCurrentPoint(null).Properties.MouseWheelDelta;
            //Debug.WriteLine("moved");
            double inc = (delta > 0) ? 0.2 : -0.2;
          
            (LatticeCanvas.RenderTransform as CompositeTransform).ScaleX += inc;
            (LatticeCanvas.RenderTransform as CompositeTransform).ScaleY += inc;
        }

        void setLineHilight(string name, bool state)
        {
            try
            {
                if (name == string.Empty)
                {
                    foreach (UIElement uie in LatticeCanvas.Children)
                    {
                        if (uie is Line)
                            (uie as Line).Stroke = normalBrush;
                    }
                }
                else
                {
                    Line l = (Line)LatticeCanvas.Children.FirstOrDefault(x => (x as Shape).Name == name);
                    if (l != null)
                        l.Stroke = (state) ? hiliteBrush : normalBrush;
                }
            }
            catch (System.InvalidOperationException)
            {

            }
        }

        
        public void Hilight(string name,bool state)
        {
            if (lastHilightName != string.Empty)
            {
                setLineHilight(lastHilightName, false);
            }
            if (name != string.Empty)
            {
                setLineHilight(name, state);
                lastHilightName = name;
            }
            else
                lastHilightName = string.Empty;
        }

       
    }
}
