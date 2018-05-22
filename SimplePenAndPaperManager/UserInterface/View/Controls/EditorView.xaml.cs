using SimplePenAndPaperManager.MapEditor;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions;
using SimplePenAndPaperManager.UserInterface.View.States;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private IDataModel _vm;

        public EditorView()
        {
            InitializeComponent();

            TerrainMap.UseCustomCursor = true;
            TerrainMap.Cursor = Cursors.Pen;

            DataContextChanged += EditorView_DataContextChanged;
        }

        private void EditorView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _vm = (IDataModel)DataContext;
            _vm.PropertyChanged += Instance_PropertyChanged;
            Gizmo.TransformationChanged += Gizmo_TransformationChanged;
        }

        private void TerrainMap_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) HandleEscape();
        }

        private void Gizmo_TransformationChanged(object sender, TransformationEvent transformationEvent)
        {
            switch (transformationEvent)
            {
                case TransformationEvent.TranslationStarted:
                    TranslateAction translation = new TranslateAction(_vm.SelectedEntities);
                    translation.TransformStartPoint = _vm.SelectionLocation;
                    DataModel.Instance.CurrentAction = translation;
                    break;
                case TransformationEvent.TranslationEnded:
                    DataModel.Instance.UndoStack.Push(DataModel.Instance.CurrentAction);
                    DataModel.Instance.CurrentAction = null;
                    break;
                case TransformationEvent.RotationStarted:
                    RotateAction rotation = new RotateAction(_vm.SelectedEntities);
                    mouseHandlingMode = MouseHandlingMode.RotateObject;
                    rotation.StartRotation = _vm.GizmoOrientation;
                    rotation.PivotPoint = _vm.SelectionLocation;
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
            if ((e.PropertyName == "GizmoDragX" && _vm.GizmoDragX)
                || (e.PropertyName == "GizmoDragY" && _vm.GizmoDragY))
                mouseHandlingMode = MouseHandlingMode.DragObject;

            if (e.PropertyName == "IsCreatingRectangle" && DataModel.Instance.IsCreatingRectangle)
            {
                mouseHandlingMode = MouseHandlingMode.CreateRectangle;
            }

            if (e.PropertyName == "IsCreatingPolygon" && DataModel.Instance.IsCreatingPolygon)
            {
                mouseHandlingMode = MouseHandlingMode.CreatePolygon;
            }

            if(e.PropertyName == "IsCreatingTextMarker" && DataModel.Instance.IsCreatingTextMarker)
            {
                mouseHandlingMode = MouseHandlingMode.CreateTextMarker;
                zoomAndPanControl.Cursor = Cursors.Pen;
            }

            if(e.PropertyName == "SelectionLocation" && mouseHandlingMode != MouseHandlingMode.DragObject)
            {
                Point canvasPoint = FromZoomControlToCanvasCoordinates(_vm.SelectionLocation.MeterToPx());
                _vm.GizmoX = Utils.PxToMeter(canvasPoint.X - Gizmo.Width / 2);
                _vm.GizmoY = Utils.PxToMeter(canvasPoint.Y - Gizmo.Height / 2);
            }

            // update selected entities postion
            if((e.PropertyName == "GizmoX" || e.PropertyName == "GizmoY") && mouseHandlingMode == MouseHandlingMode.DragObject)
            {
                TranslateAction translation = (TranslateAction)DataModel.Instance.CurrentAction;
                Point pxPoint = FromCanvasToZoomControlCoordinates(new Point(Utils.MeterToPx(_vm.GizmoX) + Gizmo.Width / 2,
                                                                             Utils.MeterToPx(_vm.GizmoY) + Gizmo.Height / 2));
                translation.TransformEndPoint = pxPoint.PxToMeter();
                translation.Do();
            }

            // update selected entities rotation
            if(e.PropertyName == "GizmoOrientation" && mouseHandlingMode == MouseHandlingMode.RotateObject)
            {
                RotateAction rotation = (RotateAction)DataModel.Instance.CurrentAction;
                rotation.EndRotation = _vm.GizmoOrientation;
                rotation.Do();
            }

            #region terrain
            if (e.PropertyName == "CurrentMap") ExpandContent();

            if (e.PropertyName == "TerrainBrushSize" || e.PropertyName == "ContentScale")
            {
                TerrainMap.DefaultDrawingAttributes.Width = Utils.MeterToPx(DataModel.Instance.TerrainBrushSize);
                TerrainMap.DefaultDrawingAttributes.Height = Utils.MeterToPx(DataModel.Instance.TerrainBrushSize);

                TerrainEllipse.Width = Utils.MeterToPx(DataModel.Instance.TerrainBrushSize) * _vm.ContentScale;
                TerrainEllipse.Height = Utils.MeterToPx(DataModel.Instance.TerrainBrushSize) * _vm.ContentScale;
                TerrainEllipse.Margin = new Thickness(-TerrainEllipse.Width / 2, -TerrainEllipse.Height / 2, 0, 0);
                TerrainRectangle.Width = Utils.MeterToPx(DataModel.Instance.TerrainBrushSize) * _vm.ContentScale;
                TerrainRectangle.Height = Utils.MeterToPx(DataModel.Instance.TerrainBrushSize) * _vm.ContentScale;
                TerrainRectangle.Margin = new Thickness(-TerrainRectangle.Width / 2, -TerrainRectangle.Height / 2, 0, 0);
            }
            else if (e.PropertyName == "TerrainBrush")
            {
                switch (DataModel.Instance.TerrainBrush)
                {
                    case TerrainBrush.Circle:
                        TerrainMap.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Ellipse;
                        break;
                    case TerrainBrush.Rectangle:
                        TerrainMap.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Rectangle;
                        break;
                }
            }
            else if (e.PropertyName == "Terrain")
            {
                switch (DataModel.Instance.Terrain)
                {
                    case FloorMaterial.Asphalt:
                        TerrainMap.DefaultDrawingAttributes.Color = Colors.LightGray;
                        break;
                    case FloorMaterial.Grass:
                        TerrainMap.DefaultDrawingAttributes.Color = Colors.OliveDrab;
                        break;
                    case FloorMaterial.Stone:
                        TerrainMap.DefaultDrawingAttributes.Color = Colors.Gray;
                        break;
                    case FloorMaterial.Water:
                        TerrainMap.DefaultDrawingAttributes.Color = Colors.CornflowerBlue;
                        break;
                    case FloorMaterial.Wood:
                        TerrainMap.DefaultDrawingAttributes.Color = Colors.DarkGoldenrod;
                        break;
                    case FloorMaterial.Sand:
                        TerrainMap.DefaultDrawingAttributes.Color = Colors.PaleGoldenrod;
                        break;
                    default:
                        TerrainMap.DefaultDrawingAttributes.Color = Colors.White;
                        break;
                }
            }
            #endregion
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
            _vm.SelectedEntities.Clear();
            if (DataModel.Instance.CurrentAction != null) DataModel.Instance.CurrentAction.Undo();
            if(DataModel.Instance.NewRectangleBuilding != null)
            {
                _vm.MapEntities.Remove(DataModel.Instance.NewRectangleBuilding);
                DataModel.Instance.NewRectangleBuilding = null;
            }
            _vm.InTerrainEditingMode = false;
            mouseHandlingMode = MouseHandlingMode.None;
        }
        #endregion

        /// <summary>
        /// Expand the content area to fit the rectangles.
        /// </summary>
        private void ExpandContent()
        { 
            _vm.ContentWidth = Utils.MeterToPx(_vm.MapWidth);
            _vm.ContentHeight = Utils.MeterToPx(_vm.MapHeight);
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

            if ((mouseHandlingMode == MouseHandlingMode.CreateRectangle || mouseHandlingMode == MouseHandlingMode.CreatePolygon))
            {
                DataModel.Instance.StartBuilding();
                zoomAndPanControl.CaptureMouse();
                e.Handled = true;
                return;
            }
            if(mouseHandlingMode == MouseHandlingMode.CreateTextMarker)
            {
                zoomAndPanControl.CaptureMouse();
                e.Handled = true;
                return;
            }

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
                if(mouseHandlingMode == MouseHandlingMode.CreateRectangle)
                {
                    // mouse rectangle location to center of shape
                    VisualRectangularBuilding building = DataModel.Instance.NewRectangleBuilding;
                    Point diagonal = building.C.Sub(building.A);
                    building.X += diagonal.X / 2;
                    building.Y += diagonal.Y / 2;
                    building.A = building.A.Sub(diagonal.Mult(0.5));
                    building.C = building.C.Sub(diagonal.Mult(0.5));
                    building.CreateFloorFromDimensions(DataModel.Instance.CurrentMap);

                    // When releasing during the creation of a rectangle
                    CreateRectangleAction createAction = new CreateRectangleAction(null) { Building = DataModel.Instance.NewRectangleBuilding };
                    DataModel.Instance.UndoStack.Push(createAction);
                    _vm.SelectedEntities.Clear();
                    DataModel.Instance.NewRectangleBuilding.IsSelected = true;
                    DataModel.Instance.NewRectangleBuilding = null;
                }
                if(mouseHandlingMode == MouseHandlingMode.CreatePolygon)
                {
                    // don't stop if polygon being is created -> this is done via doubleclick
                    return;
                }
                if(mouseHandlingMode == MouseHandlingMode.CreateTextMarker)
                {
                    // place text marker
                    CreateTextMarkerAction action = new CreateTextMarkerAction(null);
                    action.Marker = new VisualTextMarker(new TextMarker()
                    {
                        X = Utils.PxToMeter(_vm.MousePosition.X),
                        Y = Utils.PxToMeter(_vm.MousePosition.Y),
                        Text = "Text",
                        Name = Constants.DefaultMarkerName,
                        Id = DataModel.Instance.CurrentMap.GetNewId()
                    });
                    TextMarkerInputBox.SetMarkerData(action.Marker);
                    action.Marker.Name = Constants.DefaultMarkerName + "_" + action.Marker.Text.Replace(' ', '_');
                    action.Do();
                    zoomAndPanControl.Cursor = Cursors.Arrow;
                    DataModel.Instance.UndoStack.Push(action);
                }

                zoomAndPanControl.ReleaseMouseCapture();
                mouseHandlingMode = MouseHandlingMode.None;
                DataModel.Instance.IsCreatingRectangle = false;
                _vm.GizmoDragX = false;
                _vm.GizmoDragY = false;
                DataModel.Instance.NewRectangleBuilding = null;
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

                if (_vm.GizmoDragX) _vm.GizmoX = Utils.PxToMeter(canvasPoint.X - (Gizmo.X + Gizmo.Width / 2));
                if (_vm.GizmoDragY) _vm.GizmoY = Utils.PxToMeter(canvasPoint.Y - (Gizmo.Y + Gizmo.Height / 2));

                e.Handled = true;
            }
            else if(mouseHandlingMode == MouseHandlingMode.CreateRectangle && DataModel.Instance.NewRectangleBuilding != null)
            {
                // Update rectangle that is being created
                Point point = e.GetPosition(content).PxToMeter();
                Point start = DataModel.Instance.BuildingStartLocation;

                DataModel.Instance.NewRectangleBuilding.C = point.Sub(start);

                // update building in map
                _vm.MapEntities.Remove(DataModel.Instance.NewRectangleBuilding);
                _vm.MapEntities.Add(DataModel.Instance.NewRectangleBuilding);
            }
            else if(mouseHandlingMode == MouseHandlingMode.CreatePolygon && DataModel.Instance.CurrentPolygonWall != null)
            {
                // Update polygon wall which is currently being placed
                Point point = e.GetPosition(content).PxToMeter();
                DataModel.Instance.CurrentPolygonWall.X2 = point.X;
                DataModel.Instance.CurrentPolygonWall.Y2 = point.Y;
            }
            _vm.MousePosition = e.GetPosition(content);
            DataModel.Instance.CanvasPosition = FromZoomControlToCanvasCoordinates(_vm.MousePosition);
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
            if (zoomAndPanControl.ContentViewportWidth > _vm.ContentWidth)
            {
                xContentOffset -= (zoomAndPanControl.ContentViewportWidth - _vm.ContentWidth) / 2;
            }

            // check if Viewport is higher than the content and calculate the offst that has to be subtracted in this case
            if (zoomAndPanControl.ContentViewportHeight > _vm.ContentHeight)
            {
                yContentOffset -= (zoomAndPanControl.ContentViewportHeight - _vm.ContentHeight) / 2;
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
            if (zoomAndPanControl.ContentViewportWidth > _vm.ContentWidth)
            {
                xContentOffset -= (zoomAndPanControl.ContentViewportWidth - _vm.ContentWidth) / 2;
            }

            // check if Viewport is higher than the content and calculate the offst that has to be subtracted in this case
            if (zoomAndPanControl.ContentViewportHeight > _vm.ContentHeight)
            {
                yContentOffset -= (zoomAndPanControl.ContentViewportHeight - _vm.ContentHeight) / 2;
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
            if(mouseHandlingMode == MouseHandlingMode.CreatePolygon)
            {
                CreatePolygonAction action = new CreatePolygonAction(null);
                action.Do();
                DataModel.Instance.UndoStack.Push(action);
                DataModel.Instance.CurrentPolygonWall = null;   // remove current polygon building wall (is now replaced with complete building)
                DataModel.Instance.NewPolygonalBuildingWalls = null;    // remove wall collection
                DataModel.Instance.IsCreatingPolygon = false;           // polygon creation completed
                mouseHandlingMode = MouseHandlingMode.None;
                zoomAndPanControl.ReleaseMouseCapture();
                e.Handled = true;

                return;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
            {
                Point doubleClickPoint = e.GetPosition(content);
                zoomAndPanControl.AnimatedSnapTo(doubleClickPoint);
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
            {
                _vm.SelectedEntities.Clear();
            }

            FrameworkElement selectedObject = e.OriginalSource as FrameworkElement;
            ((IVisualElement)selectedObject.DataContext).IsSelected = true;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _vm.LastSelected = (IVisualElement)((FrameworkElement)e.OriginalSource).DataContext;
        }
    }
}
