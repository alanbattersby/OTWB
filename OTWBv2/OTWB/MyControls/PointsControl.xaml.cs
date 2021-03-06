﻿using OTWB.Coordinates;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OTWB.MyControls
{
    public sealed partial class PointsControl : UserControl
    {
        public string Title { get; set; }
        public PointsControl()
        {
            this.InitializeComponent();
        }

        public int SelectedPath
        {
            get { return this.PathCombo.SelectedIndex; }
            set { this.PathCombo.SelectedIndex = value; }
        }

        private void PathCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is List<PathFragment>)
                PointsView.DataContext = (this.DataContext as List<PathFragment>)[PathCombo.SelectedIndex];
            PathCount.DataContext = PointsView.DataContext;
        }
    }
}
