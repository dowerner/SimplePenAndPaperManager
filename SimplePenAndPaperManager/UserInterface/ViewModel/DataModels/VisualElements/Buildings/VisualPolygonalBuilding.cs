using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualPolygonalBuilding : PolygonElement
    {
        public new PointCollection Corners
        {
            get { return _corners; }
            set
            {
                base.Corners = value;
                fillPointsToSource();
            }
        }

        private IPolygonMapEntity _polygonSource;

        public VisualPolygonalBuilding(IPolygonMapEntity mapEntity) : base(mapEntity)
        {
            _polygonSource = mapEntity;
            fillPointsFromSource();
        }

        public override IVisualElement Copy()
        {
            return new VisualPolygonalBuilding((IPolygonMapEntity)_polygonSource.Copy());
        }

        private void fillPointsFromSource()
        {
            if (_polygonSource.Corners == null) return;
            if (_corners != null) _corners.Changed -= _corners_Changed;
            _corners = new PointCollection();
            foreach (Point2D point in _polygonSource.Corners) _corners.Add(new Point(point.X, point.Y));
            _corners.Changed += _corners_Changed;
        }

        private void fillPointsToSource()
        {
            if (_polygonSource.Corners == null) _polygonSource.Corners = new List<Point2D>();
            _polygonSource.Corners.Clear();
            foreach (Point point in _corners) _polygonSource.Corners.Add(new Point2D() { X = point.X, Y = point.Y });
        }

        private void _corners_Changed(object sender, EventArgs e)
        {
            fillPointsToSource();
        }
    }
}
