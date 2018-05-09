using System;
using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.MathTools;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreatePolygonAction : BaseAction
    {
        public PolygonElement Building { get; set; }
        public List<WallElement> ConstructionWalls { get; set; }

        public override void Do()
        {
            DataModel.Instance.SelectedEntities.Clear();

            if(Building == null)
            {
                // Create polygon points from line collection
                List<Point2D> points = new List<Point2D>();
                double x = 0;   // sum of x coordinates
                double y = 0;   // sum of y coordinates
                foreach (WallElement wall in DataModel.Instance.NewPolygonalBuildingWalls)
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
                    DataModel.Instance.MapEntities.Remove(wall);    // remove wall from the world
                }

                x /= (points.Count);    // calculate x coordinate of the polygon center
                y /= (points.Count);    // calculate y coordinate of the polygon center

                for (int i = 0; i < points.Count; i++)  // move corner points to local coordinate system of the polygon
                {
                    points[i] = new Point2D() { X = points[i].X - x, Y = points[i].Y - y };
                }

                // create polygon element
                Building = new PolygonElement(new MapEditor.Entities.Buildings.PolygonBuilding() { Corners = points, X = x, Y = y });
            }            

            DataModel.Instance.MapEntities.Add(Building);   // add polygon to the world
            Building.IsSelected = true;
        }

        public override void Undo()
        {
            if (DataModel.Instance.SelectedEntities.Contains(Building))
            {
                DataModel.Instance.SelectedEntities.Remove(Building);
            }
            DataModel.Instance.MapEntities.Remove(Building);
        }

        public CreatePolygonAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }
    }
}
