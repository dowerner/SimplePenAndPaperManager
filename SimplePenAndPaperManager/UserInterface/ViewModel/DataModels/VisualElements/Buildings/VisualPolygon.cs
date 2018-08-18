using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualPolygon : PolygonElement
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

        protected IPolygonMapEntity _polygonSource;

        public VisualPolygon(IPolygonMapEntity mapEntity) : base(mapEntity)
        {
            _polygonSource = mapEntity;
            fillPointsFromSource();
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

        public void UpdateCenter()
        {
            Point offset = new Point(0, 0);
            foreach (Point point in Corners) offset = offset.Add(point);
            offset = offset.Mult(1.0 / Corners.Count);

            PointCollection newCorners = new PointCollection();
            foreach (Point point in Corners) newCorners.Add(point.Sub(offset));
            Corners = newCorners;

            offset = offset.Rotate(Orientation);

            X += offset.X;
            Y += offset.Y;
        }

        public override IVisualElement Copy()
        {
            return new VisualPolygon((IPolygonMapEntity)_polygonSource.Copy());
        }
    }
}
