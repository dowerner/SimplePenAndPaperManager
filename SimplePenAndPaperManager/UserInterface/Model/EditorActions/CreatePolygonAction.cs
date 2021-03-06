﻿using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreatePolygonAction : BaseAction
    {
        public VisualPolygonalBuilding Building { get; set; }
        public List<WallElement> ConstructionWalls { get; set; }

        public override void Do()
        {
            _context.SelectedEntities.Clear();

            if(Building == null)
            {
                // Create polygon points from line collection
                List<Point2D> points = new List<Point2D>();
                double x = 0;   // sum of x coordinates
                double y = 0;   // sum of y coordinates
                foreach (WallElement wall in ((DataModel)_context).NewPolygonalBuildingWalls)
                {
                    Point2D point1 = new Point2D() { X = wall.X1, Y = wall.Y1 };
                    Point2D point2 = new Point2D() { X = wall.X2, Y = wall.Y2 };
                    if (points.AddIfNoPointPresent(point1)) // add first point if no point is at the location
                    {
                        x += point1.X;
                        y += point1.Y;
                    }
                    if (points.AddIfNoPointPresent(point2)) // add second point if no point is at the location
                    {
                        x += point2.X;
                        y += point2.Y;
                    }
                    ((DataModel)_context).MapEntities.Remove(wall);    // remove wall from the world
                }

                x /= (points.Count);    // calculate x coordinate of the polygon center
                y /= (points.Count);    // calculate y coordinate of the polygon center

                for (int i = 0; i < points.Count; i++)  // move corner points to local coordinate system of the polygon
                {
                    points[i] = new Point2D() { X = points[i].X - x, Y = points[i].Y - y };
                }

                // create polygon element
                Building = new VisualPolygonalBuilding(new PolygonBuilding() { Corners = points, X = x, Y = y, Id = ((DataModel)_context).CurrentMap.GetNewId(), Name = Constants.DefaultHouseName });
                Building.CreateFloorFromDimensions();
            }            

            ((DataModel)(_context)).MapEntities.Add(Building);   // add polygon to the world
            Building.IsSelected = true;
        }

        public override void Undo()
        {
            if (_context.SelectedEntities.Contains(Building))
            {
                _context.SelectedEntities.Remove(Building);
            }
            _context.MapEntities.Remove(Building);
        }

        public CreatePolygonAction(ObservableCollection<ViewModel.DataModels.VisualElements.Interface.IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
        }
    }
}
