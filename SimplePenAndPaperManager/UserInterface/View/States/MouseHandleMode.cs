﻿namespace SimplePenAndPaperManager.UserInterface.View.States
{
    /// <summary>
    /// Defines the current state of the mouse handling logic.
    /// </summary>
    public enum MouseHandlingMode
    {
        /// <summary>
        /// Not in any special mode.
        /// </summary>
        None,

        /// <summary>
        /// The user is left-dragging rectangles with the mouse.
        /// </summary>
        DraggingRectangles,

        /// <summary>
        /// The user is left-mouse-button-dragging to pan the viewport.
        /// </summary>
        Panning,

        /// <summary>
        /// The user is holding down shift and left-clicking or right-clicking to zoom in or out.
        /// </summary>
        Zooming,

        /// <summary>
        /// The user is holding down shift and left-mouse-button-dragging to select a region to zoom to.
        /// </summary>
        DragZooming,

        /// <summary>
        /// The user is dragging an object via gizmo.
        /// </summary>
        DragObject,

        /// <summary>
        /// The user is rotating an object via gizmo.
        /// </summary>
        RotateObject,

        /// <summary>
        /// The user is dragging the lower right corner of a newly created rectangle.
        /// </summary>
        CreateRectangle,

        /// <summary>
        /// The user is creating a polygon.
        /// </summary>
        CreatePolygon,

        CreateTextMarker,

        CreateWallAttachable,

        CreateWall
    }
}
