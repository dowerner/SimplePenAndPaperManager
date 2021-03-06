﻿using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        private IDataModel _vm;

        public OverviewWindow(IDataModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        /// <summary>
        /// Event raised when the size of the ZoomAndPanControl changes.
        /// </summary>
        private void overview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //
            // Update the scale so that the entire content fits in the window.
            //
            overview.ScaleToFit();
        }

        /// <summary>
        /// Event raised when the user drags the overview zoom rect.
        /// </summary>
        private void overviewZoomRectThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //
            // Update the position of the overview rect as the user drags it around.
            //
            double newContentOffsetX = Math.Min(Math.Max(0.0, Canvas.GetLeft(overviewZoomRectThumb) + e.HorizontalChange), _vm.ContentWidth - _vm.ContentViewportWidth);
            Canvas.SetLeft(overviewZoomRectThumb, newContentOffsetX);

            double newContentOffsetY = Math.Min(Math.Max(0.0, Canvas.GetTop(overviewZoomRectThumb) + e.VerticalChange), _vm.ContentHeight - _vm.ContentViewportHeight);
            Canvas.SetTop(overviewZoomRectThumb, newContentOffsetY);
        }

        /// <summary>
        /// Event raised on mouse down.
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //
            // Update the position of the overview rect to the point that was clicked.
            //
            Point clickedPoint = e.GetPosition(content);
            double newX = clickedPoint.X - (overviewZoomRectThumb.Width / 2);
            double newY = clickedPoint.Y - (overviewZoomRectThumb.Height / 2);
            Canvas.SetLeft(overviewZoomRectThumb, newX);
            Canvas.SetTop(overviewZoomRectThumb, newY);
        }
    }
}
