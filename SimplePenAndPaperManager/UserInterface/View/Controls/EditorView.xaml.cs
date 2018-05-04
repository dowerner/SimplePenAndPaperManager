﻿using SimplePenAndPaperManager.UserInterface.Model.EditorActions;
using SimplePenAndPaperManager.UserInterface.View.States;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using ZoomAndPan;

namespace SimplePenAndPaperManager.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for EditorView.xaml
    /// </summary>
    public partial class EditorView : UserControl
    {
        /// <summary>
        /// Specifies the current state of the mouse handling logic.
        /// </summary>
        private MouseHandlingMode mouseHandlingMode = MouseHandlingMode.None;

        /// <summary>
        /// The point that was clicked relative to the ZoomAndPanControl.
        /// </summary>
        private Point origZoomAndPanControlMouseDownPoint;

        /// <summary>
        /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
        /// </summary>
        private Point origContentMouseDownPoint;

        /// <summary>
        /// Records which mouse button clicked during mouse dragging.
        /// </summary>
        private MouseButton mouseButtonDown;

        /// <summary>
        /// Saves the previous zoom rectangle, pressing the backspace key jumps back to this zoom rectangle.
        /// </summary>
        private Rect prevZoomRect;

        /// <summary>
        /// Save the previous content scale, pressing the backspace key jumps back to this scale.
        /// </summary>
        private double prevZoomScale;

        /// <summary>
        /// Set to 'true' when the previous zoom rect is saved.
        /// </summary>
        private bool prevZoomRectSet = false;

        public EditorView()
        {
            InitializeComponent();
            DataModel.Instance.PropertyChanged += Instance_PropertyChanged;
            Gizmo.TransformationChanged += Gizmo_TransformationChanged;
        }

        private void Gizmo_TransformationChanged(object sender, TransformationEvent transformationEvent)
        {
            switch (transformationEvent)
            {
                case TransformationEvent.TranslationStarted:
                    TranslateAction translation = new TranslateAction(DataModel.Instance.SelectedEntities);
                    translation.TransformStartPoint = DataModel.Instance.SelectionLocation;
                    DataModel.Instance.CurrentAction = translation;
                    break;
                case TransformationEvent.TranslationEnded:
                    DataModel.Instance.UndoStack.Push(DataModel.Instance.CurrentAction);
                    DataModel.Instance.CurrentAction = null;
                    break;
                case TransformationEvent.RotationStarted:
                    RotateAction rotation = new RotateAction(DataModel.Instance.SelectedEntities);
                    mouseHandlingMode = MouseHandlingMode.RotateObject;
                    rotation.StartRotation = DataModel.Instance.GizmoOrientation;
                    rotation.PivotPoint = DataModel.Instance.SelectionLocation;
                    DataModel.Instance.CurrentAction = rotation;
                    break;
                case TransformationEvent.RotationEnded:
                    mouseHandlingMode = MouseHandlingMode.None;
                    DataModel.Instance.UndoStack.Push(DataModel.Instance.CurrentAction);
                    DataModel.Instance.CurrentAction = null;
                    break;
            }
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "GizmoDragX" && DataModel.Instance.GizmoDragX) 
                || (e.PropertyName == "GizmoDragY" && DataModel.Instance.GizmoDragY))
                mouseHandlingMode = MouseHandlingMode.DragObject;

            if(e.PropertyName == "SelectionLocation" && mouseHandlingMode != MouseHandlingMode.DragObject)
            {
                Point canvasPoint = FromZoomControlToCanvasCoordinates(DataModel.Instance.SelectionLocation);
                DataModel.Instance.GizmoX = canvasPoint.X;
                DataModel.Instance.GizmoY = canvasPoint.Y;
            }

            // update selected entities postion
            if((e.PropertyName == "GizmoX" || e.PropertyName == "GizmoY") && mouseHandlingMode == MouseHandlingMode.DragObject)
            {
                TranslateAction translation = (TranslateAction)DataModel.Instance.CurrentAction;
                translation.TransformEndPoint = FromCanvasToZoomControlCoordinates(new Point(DataModel.Instance.GizmoX, DataModel.Instance.GizmoY));
                translation.Do();
            }

            // update selected entities rotation
            if(e.PropertyName == "GizmoOrientation" && mouseHandlingMode == MouseHandlingMode.RotateObject)
            {
                RotateAction rotation = (RotateAction)DataModel.Instance.CurrentAction;
                rotation.EndRotation = DataModel.Instance.GizmoOrientation;
                rotation.Do();
            }
        }

        /// <summary>
        /// Event raised when the Window has loaded.
        /// </summary>
        private void EditorView_Loaded(object sender, RoutedEventArgs e)
        {
            OverviewWindow overviewWindow = new OverviewWindow();
            Window parent = Window.GetWindow(this);
            overviewWindow.Left = parent.Left;
            overviewWindow.Top = parent.Top + Height + 5;
            overviewWindow.Owner = parent;
            overviewWindow.Show();

            KeyDown += EditorView_KeyDown;

            ExpandContent();
        }

        private void EditorView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    HandleEscape();
                    break;
            }
        }

        #region Key Handling
        private void HandleEscape()
        {
            DataModel.Instance.SelectedEntities.Clear();
            if (DataModel.Instance.CurrentAction != null) DataModel.Instance.CurrentAction.Undo();
            mouseHandlingMode = MouseHandlingMode.None;
        }
        #endregion

        /// <summary>
        /// Expand the content area to fit the rectangles.
        /// </summary>
        private void ExpandContent()
        {
            DataModel.Instance.ContentWidth = 4000;
            DataModel.Instance.ContentHeight = 4000;
        }

        /// <summary>
        /// Event raised on mouse down in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            content.Focus();
            Keyboard.Focus(content);

            mouseButtonDown = e.ChangedButton;
            origZoomAndPanControlMouseDownPoint = e.GetPosition(zoomAndPanControl);
            origContentMouseDownPoint = e.GetPosition(content);

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 &&
                (e.ChangedButton == MouseButton.Left ||
                 e.ChangedButton == MouseButton.Right))
            {
                // Shift + left- or right-down initiates zooming mode.
                mouseHandlingMode = MouseHandlingMode.Zooming;
            }
            else if (mouseButtonDown == MouseButton.Left)
            {
                // Just a plain old left-down initiates panning mode.
                mouseHandlingMode = MouseHandlingMode.Panning;
            }

            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                // Capture the mouse so that we eventually receive the mouse up event.
                zoomAndPanControl.CaptureMouse();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse up in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.None)
            {
                if (mouseHandlingMode == MouseHandlingMode.Zooming)
                {
                    if (mouseButtonDown == MouseButton.Left)
                    {
                        // Shift + left-click zooms in on the content.
                        ZoomIn(origContentMouseDownPoint);
                    }
                    else if (mouseButtonDown == MouseButton.Right)
                    {
                        // Shift + left-click zooms out from the content.
                        ZoomOut(origContentMouseDownPoint);
                    }
                }
                else if (mouseHandlingMode == MouseHandlingMode.DragZooming)
                {
                    // When drag-zooming has finished we zoom in on the rectangle that was highlighted by the user.
                    ApplyDragZoomRect();
                }
                if(mouseHandlingMode == MouseHandlingMode.DragObject)
                {
                    // When releasing dragged objects
                    Gizmo_TransformationChanged(Gizmo, TransformationEvent.TranslationEnded);
                }

                zoomAndPanControl.ReleaseMouseCapture();
                mouseHandlingMode = MouseHandlingMode.None;
                DataModel.Instance.GizmoDragX = false;
                DataModel.Instance.GizmoDragY = false;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse move in the ZoomAndPanControl.
        /// </summary>
        private void zoomAndPanControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseHandlingMode == MouseHandlingMode.Panning)
            {
                //
                // The user is left-dragging the mouse.
                // Pan the viewport by the appropriate amount.
                //
                Point curContentMousePoint = e.GetPosition(content);
                Vector dragOffset = curContentMousePoint - origContentMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= dragOffset.X;
                zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.Zooming)
            {
                Point curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
                Vector dragOffset = curZoomAndPanControlMousePoint - origZoomAndPanControlMouseDownPoint;
                double dragThreshold = 10;
                if (mouseButtonDown == MouseButton.Left &&
                    (Math.Abs(dragOffset.X) > dragThreshold ||
                     Math.Abs(dragOffset.Y) > dragThreshold))
                {
                    //
                    // When Shift + left-down zooming mode and the user drags beyond the drag threshold,
                    // initiate drag zooming mode where the user can drag out a rectangle to select the area
                    // to zoom in on.
                    //
                    mouseHandlingMode = MouseHandlingMode.DragZooming;
                    Point curContentMousePoint = e.GetPosition(content);
                    InitDragZoomRect(origContentMouseDownPoint, curContentMousePoint);
                }

                e.Handled = true;
            }
            else if (mouseHandlingMode == MouseHandlingMode.DragZooming)
            {
                //
                // When in drag zooming mode continously update the position of the rectangle
                // that the user is dragging out.
                //
                Point curContentMousePoint = e.GetPosition(content);
                SetDragZoomRect(origContentMouseDownPoint, curContentMousePoint);

                e.Handled = true;
            }
            else if(mouseHandlingMode == MouseHandlingMode.DragObject)
            {
                //
                // Update position of transformation gizmo on overlay canvas to drag an object around
                //
                Point canvasPoint = FromZoomControlToCanvasCoordinates(e.GetPosition(content));

                if (DataModel.Instance.GizmoDragX) DataModel.Instance.GizmoX = canvasPoint.X - (Gizmo.X + Gizmo.Width / 2);
                if (DataModel.Instance.GizmoDragY) DataModel.Instance.GizmoY = canvasPoint.Y - (Gizmo.Y + Gizmo.Height / 2);

                e.Handled = true;
            }
            DataModel.Instance.MousePosition = e.GetPosition(content);
        }

        #region Math Tools
        /// <summary>
        /// Transform point from zoom control content to overlay canvas coordinates
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point FromZoomControlToCanvasCoordinates(Point point)
        {
            double xContentOffset = zoomAndPanControl.ContentOffsetX;   // get x content offset
            double yContentOffset = zoomAndPanControl.ContentOffsetY;   // get y content offset

            // check if Viewport is wider than the content and calculate the offst that has to be subtracted in this case
            if (zoomAndPanControl.ContentViewportWidth > DataModel.Instance.ContentWidth)
            {
                xContentOffset -= (zoomAndPanControl.ContentViewportWidth - DataModel.Instance.ContentWidth) / 2;
            }

            // check if Viewport is higher than the content and calculate the offst that has to be subtracted in this case
            if (zoomAndPanControl.ContentViewportHeight > DataModel.Instance.ContentHeight)
            {
                yContentOffset -= (zoomAndPanControl.ContentViewportHeight - DataModel.Instance.ContentHeight) / 2;
            }

            // return point in canvas coordinate system
            return new Point((point.X - xContentOffset) * zoomAndPanControl.ContentScale,
                             (point.Y - yContentOffset) * zoomAndPanControl.ContentScale);
        }

        /// <summary>
        /// Transform point from overlay canvas to zoom control coordinates
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point FromCanvasToZoomControlCoordinates(Point point)
        {
            double xContentOffset = zoomAndPanControl.ContentOffsetX;   // get x content offset
            double yContentOffset = zoomAndPanControl.ContentOffsetY;   // get y content offset

            // check if Viewport is wider than the content and calculate the offst that has to be subtracted in this case
            if (zoomAndPanControl.ContentViewportWidth > DataModel.Instance.ContentWidth)
            {
                xContentOffset -= (zoomAndPanControl.ContentViewportWidth - DataModel.Instance.ContentWidth) / 2;
            }

            // check if Viewport is higher than the content and calculate the offst that has to be subtracted in this case
            if (zoomAndPanControl.ContentViewportHeight > DataModel.Instance.ContentHeight)
            {
                yContentOffset -= (zoomAndPanControl.ContentViewportHeight - DataModel.Instance.ContentHeight) / 2;
            }

            // return point in zoom control coordinate system
            return new Point(point.X / zoomAndPanControl.ContentScale + xContentOffset,
                             point.Y / zoomAndPanControl.ContentScale + yContentOffset);
        }
        #endregion

        /// <summary>
        /// Event raised by rotating the mouse wheel
        /// </summary>
        private void zoomAndPanControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                Point curContentMousePoint = e.GetPosition(content);
                ZoomIn(curContentMousePoint);
            }
            else if (e.Delta < 0)
            {
                Point curContentMousePoint = e.GetPosition(content);
                ZoomOut(curContentMousePoint);
            }
        }

        /// <summary>
        /// The 'ZoomIn' command (bound to the plus key) was executed.
        /// </summary>
        private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomIn(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
        }

        /// <summary>
        /// The 'ZoomOut' command (bound to the minus key) was executed.
        /// </summary>
        private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomOut(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
        }

        /// <summary>
        /// The 'JumpBackToPrevZoom' command was executed.
        /// </summary>
        private void JumpBackToPrevZoom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            JumpBackToPrevZoom();
        }

        /// <summary>
        /// Determines whether the 'JumpBackToPrevZoom' command can be executed.
        /// </summary>
        private void JumpBackToPrevZoom_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = prevZoomRectSet;
        }

        /// <summary>
        /// The 'Fill' command was executed.
        /// </summary>
        private void Fill_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            zoomAndPanControl.AnimatedScaleToFit();
        }

        /// <summary>
        /// The 'OneHundredPercent' command was executed.
        /// </summary>
        private void OneHundredPercent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            zoomAndPanControl.AnimatedZoomTo(1.0);
        }

        /// <summary>
        /// Jump back to the previous zoom level.
        /// </summary>
        private void JumpBackToPrevZoom()
        {
            zoomAndPanControl.AnimatedZoomTo(prevZoomScale, prevZoomRect);

            ClearPrevZoomRect();
        }

        /// <summary>
        /// Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomOut(Point contentZoomCenter)
        {
            zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale - 0.1, contentZoomCenter);
        }

        /// <summary>
        /// Zoom the viewport in, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomIn(Point contentZoomCenter)
        {
            zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale + 0.1, contentZoomCenter);
        }

        /// <summary>
        /// Initialise the rectangle that the use is dragging out.
        /// </summary>
        private void InitDragZoomRect(Point pt1, Point pt2)
        {
            SetDragZoomRect(pt1, pt2);

            dragZoomCanvas.Visibility = Visibility.Visible;
            dragZoomBorder.Opacity = 0.5;
        }

        /// <summary>
        /// Update the position and size of the rectangle that user is dragging out.
        /// </summary>
        private void SetDragZoomRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            //
            // Deterine x,y,width and height of the rect inverting the points if necessary.
            // 

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            //
            // Update the coordinates of the rectangle that is being dragged out by the user.
            // The we offset and rescale to convert from content coordinates.
            //
            Canvas.SetLeft(dragZoomBorder, x);
            Canvas.SetTop(dragZoomBorder, y);
            dragZoomBorder.Width = width;
            dragZoomBorder.Height = height;
        }

        /// <summary>
        /// When the user has finished dragging out the rectangle the zoom operation is applied.
        /// </summary>
        private void ApplyDragZoomRect()
        {
            //
            // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
            //
            SavePrevZoomRect();

            //
            // Retreive the rectangle that the user draggged out and zoom in on it.
            //
            double contentX = Canvas.GetLeft(dragZoomBorder);
            double contentY = Canvas.GetTop(dragZoomBorder);
            double contentWidth = dragZoomBorder.Width;
            double contentHeight = dragZoomBorder.Height;
            zoomAndPanControl.AnimatedZoomTo(new Rect(contentX, contentY, contentWidth, contentHeight));

            FadeOutDragZoomRect();
        }

        //
        // Fade out the drag zoom rectangle.
        //
        private void FadeOutDragZoomRect()
        {
            AnimationHelper.StartAnimation(dragZoomBorder, Border.OpacityProperty, 0.0, 0.1,
                delegate (object sender, EventArgs e)
                {
                    dragZoomCanvas.Visibility = Visibility.Collapsed;
                });
        }

        //
        // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
        //
        private void SavePrevZoomRect()
        {
            prevZoomRect = new Rect(zoomAndPanControl.ContentOffsetX, zoomAndPanControl.ContentOffsetY, zoomAndPanControl.ContentViewportWidth, zoomAndPanControl.ContentViewportHeight);
            prevZoomScale = zoomAndPanControl.ContentScale;
            prevZoomRectSet = true;
        }

        /// <summary>
        /// Clear the memory of the previous zoom level.
        /// </summary>
        private void ClearPrevZoomRect()
        {
            prevZoomRectSet = false;
        }

        /// <summary>
        /// Event raised when the user has double clicked in the zoom and pan control.
        /// </summary>
        private void zoomAndPanControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
            {
                Point doubleClickPoint = e.GetPosition(content);
                zoomAndPanControl.AnimatedSnapTo(doubleClickPoint);
            }
        }

        /// <summary>
        /// Event raised when a mouse button is released over a Rectangle.
        /// </summary>
        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseHandlingMode != MouseHandlingMode.DraggingRectangles)
            {
                //
                // We are not in rectangle dragging mode.
                //
                return;
            }

            mouseHandlingMode = MouseHandlingMode.None;

            Rectangle rectangle = (Rectangle)sender;
            rectangle.ReleaseMouseCapture();

            e.Handled = true;
        }
    }
}