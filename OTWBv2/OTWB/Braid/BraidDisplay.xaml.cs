using OTWB.Common;
using System;
using System.Collections.Generic;
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

namespace OTWB.Braid
{
    public sealed partial class BraidDisplay : UserControl
    {
        enum MOVEDPOINT { START, END };
        const int lwidth = 3;

        CartesianGrid _grid;
        string lastHilightName;

        Brush normalBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);
        Brush hiliteBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        string namefmt = "BraidLine{0}_{1}";

        Point pointerPosition;

        MOVEDPOINT movepoint;

        public BraidDisplay()
        {
            this.InitializeComponent();
            lastHilightName = string.Empty;
        }

        BraidData _braid;
        public BraidData Braid
        {
            get { return _braid; }
            set
            {
                _braid = value;
                if (_braid == null) return;
                _braid.PropertyChanged += _braid_PropertyChanged;
                int cols = Math.Max(_braid.Perms.Count,5);
                Range x = new Range(0, 1, cols);
                int rows = Math.Max(_braid.NumStrands, 5); 
                Range y = new Range(0,1,rows);
                _grid = new CartesianGrid(x, y);
                _grid.Name = "Braid_GRID";

                Show();
            }
        }

        private void _braid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Show();
        }

        public void Hilight(int indx)
        {
            foreach (Shape uie in BraidCanvas.Children)
            {
                if (uie.Name.StartsWith("BraidLine"))
                    if (StrandOf(uie) == indx)
                        uie.Stroke = hiliteBrush;
                    else
                        uie.Stroke = normalBrush;
            }
        }

        public void UnHilight()
        {
            foreach (Shape uie in BraidCanvas.Children)
            {
                if ((uie is Line) && (uie.Name.StartsWith("BraidLine")))
                    uie.Stroke = normalBrush;
            }
        }

        public void Show()
        {
            Clear();
            Tuple<double, double> t = ScaleFactors;
            double st = 1 / Math.Min(t.Item1, t.Item2);
            if (double.IsInfinity(st) || double.IsNaN(st)) return;
            ChangeScaleFactorTo(t);
            Shape s = _grid.Outline;
            s.StrokeThickness = st;
            BraidCanvas.Children.Add(s);
            
            for (int col = 0; col < _braid.Perms.Count; col++)
            {
                Permutation perm = _braid.Perms[col];
                for (int row = 0 ; row < _braid.NumStrands; row++)
                {
                    Line l = new Line();
                    l.Name = string.Format(namefmt, col, row);
                    l.X1 = col;
                    l.Y1 = row;
                    l.X2 = col + 1;
                    l.Y2 = perm.PermOf(row);
                    l.Stroke = normalBrush;
                    l.StrokeThickness = 2 * st;
                    l.Tag = new Tuple<int,int>(col,row);   // id of permutation 
                    AttachPointerEvents(ref l);
                    BraidCanvas.Children.Add(l);
                 }
            }
            
        }

        int PermOf(Shape l)
        {
            return (l.Tag as Tuple<int, int>).Item1;
        }

        int StrandOf(Shape l)
        {
            return (l.Tag as Tuple<int, int>).Item2;
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

        public void Clear()
        {
            this.BraidCanvas.Children.Clear();
        }

        private Tuple<double, double> ScaleFactors
        {
            get
            {
                double width = (this.BraidBorder.ActualWidth <= 0)
                                   ? BraidBorder.MinWidth : BraidBorder.ActualWidth;
                width -= BraidBorder.BorderThickness.Left + BraidBorder.BorderThickness.Right
                           + BraidBorder.Margin.Left + BraidBorder.Margin.Right;


                double height = (BraidBorder.ActualHeight <= 0)
                              ? BraidBorder.MinHeight : BraidBorder.ActualHeight;
                height -= BraidBorder.BorderThickness.Top + BraidBorder.BorderThickness.Bottom
                        + BraidBorder.Margin.Top + BraidBorder.Margin.Bottom;

                return new Tuple<double, double>
                    (width / _grid.Dim1.End, height / _grid.Dim2.End);

            }
        }

        public void ChangeScaleFactorTo(Tuple<double, double> sfs)
        {
            try
            {

                if (sfs.Item1 > 0 && sfs.Item2 > 0)
                {
                    (BraidCanvas.RenderTransform as CompositeTransform).ScaleX = sfs.Item1;
                    (BraidCanvas.RenderTransform as CompositeTransform).ScaleY = sfs.Item2;
                    foreach (Shape uie in BraidCanvas.Children)
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

        private void BraidCanvas_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            int delta = e.GetCurrentPoint(null).Properties.MouseWheelDelta;
            //Debug.WriteLine("moved");
            double inc = (delta > 0) ? 0.2 : -0.2;

            (BraidCanvas.RenderTransform as CompositeTransform).ScaleX += inc;
            (BraidCanvas.RenderTransform as CompositeTransform).ScaleY += inc;
        }

        public void Hilight(string name, bool state)
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
        void setLineHilight(string name, bool state)
        {
            try
            {
                if (name == string.Empty)
                {
                    foreach (UIElement uie in BraidCanvas.Children)
                    {
                        if (uie is Line)
                            (uie as Line).Stroke = normalBrush;
                    }
                }
                else
                {
                    Line l = (Line)BraidCanvas.Children.FirstOrDefault(x => (x as Shape).Name == name);
                    if (l != null)
                        l.Stroke = (state) ? hiliteBrush : normalBrush;
                }
            }
            catch (System.InvalidOperationException)
            {

            }
        }

        double distance(double x1, double y1, double x2, double y2)
        {
            double xdiff = x2 - x1;
            double ydiff = y2 - y1;
            return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }

        void l_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            l.ReleasePointerCapture(e.Pointer);

            Hilight(l.Name, false);
            lastHilightName = string.Empty;
        }

        void l_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            if (l.PointerCaptures != null && l.PointerCaptures.Count == 1)
            {
                PointerPoint p = e.GetCurrentPoint(BraidCanvas);
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
              
                pointerPosition.X = x;
                pointerPosition.Y = y;
            }
        }

        void l_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            Point pntrposn = e.GetCurrentPoint(BraidCanvas).Position;
            movepoint = MOVEDPOINT.END;
                     
            Tuple<double, double> t = ScaleFactors;
            l.CapturePointer(e.Pointer);
            pointerPosition = new Point();
            pointerPosition.X = Math.Round(pntrposn.X); // t.Item1;
            pointerPosition.Y = Math.Round(pntrposn.Y); // t.Item2;
            Hilight(l.Name, true);
        }

        void l_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            l.X2 = Math.Round(l.X2);
            l.Y2 = Math.Round(l.Y2);
            l.ReleasePointerCapture(e.Pointer);
            Hilight(l.Name, false);
            lastHilightName = string.Empty;
            int permindx = PermOf(l);
            int strand = StrandOf(l);

            Permutation perm = Braid.Perms[permindx];
            if (movepoint == MOVEDPOINT.END)
            {
                int newstrand = (int)l.Y2;
                perm.SetPermOf(strand, newstrand);
            }

            Show();
        }

        void l_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            Line l = (sender as Line);
            l.ReleasePointerCapture(e.Pointer);
            Hilight(l.Name, false);
            lastHilightName = string.Empty;

        }
    }
}
