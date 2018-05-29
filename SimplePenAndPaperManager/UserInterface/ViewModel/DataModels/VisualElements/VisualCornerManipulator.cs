using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System;
using System.Windows;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public class VisualCornerManipulator : BaseVisualElement
    {
        public override double X
        {
            get
            {
                double angle = _polygon.Orientation * Math.PI / 180;
                double x = Math.Cos(angle) * _polygon.Corners[_cornerIndex].X - Math.Sin(angle) * _polygon.Corners[_cornerIndex].Y;
                _x = x + _polygon.X - Width / 2;
                return _x;
            }
            set
            {
                double angle = _polygon.Orientation * Math.PI / 180;
                double x = Math.Cos(angle) * (value - _polygon.X + Width / 2) + Math.Sin(angle) * (_y - _polygon.Y + Height / 2);

                _polygon.Corners[_cornerIndex] = new Point(x, _polygon.Corners[_cornerIndex].Y);
                _polygon.Corners = _polygon.Corners;
                _polygon.UpdateAllFloorDimensions();
                OnPropertyChanged("X");
            }
        }
        private double _x;

        public override double Y
        {
            get
            {
                double angle = _polygon.Orientation * Math.PI / 180;
                double y = Math.Sin(angle) * _polygon.Corners[_cornerIndex].X + Math.Cos(angle) * _polygon.Corners[_cornerIndex].Y;
                _y = y + _polygon.Y - Height / 2;
                return _y;
            }
            set
            {
                double angle = _polygon.Orientation * Math.PI / 180;
                double y = -Math.Sin(angle) * (_x - _polygon.X + Width / 2) + Math.Cos(angle) * (value - _polygon.Y + Height / 2);

                _polygon.Corners[_cornerIndex] = new Point(_polygon.Corners[_cornerIndex].X, y);
                _polygon.Corners = _polygon.Corners;
                _polygon.UpdateAllFloorDimensions();
                OnPropertyChanged("Y");
            }
        }
        private double _y;

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        private double _width;

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }
        private double _height;

        public int CornerIndex
        {
            get { return _cornerIndex; }
            set
            {
                _cornerIndex = value;
                OnPropertyChanged("CornerIndex");
            }
        }
        private int _cornerIndex;

        public VisualPolygonalBuilding Polygon
        {
            get { return _polygon; }
            set
            {
                _polygon = value;
                OnPropertyChanged("Polygon");
            }
        }
        private VisualPolygonalBuilding _polygon;

        public VisualCornerManipulator(IMapEntity mapEntity, VisualPolygonalBuilding polygon, int index) : base(mapEntity)
        {
            Color = Colors.Blue;
            Width = 1;
            Height = 1;
            _polygon = polygon;
            _polygon.PropertyChanged += _polygon_PropertyChanged;
            _cornerIndex = index;
        }

        private void _polygon_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
            {
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// Copy is not allowed
        /// </summary>
        /// <returns></returns>
        public override IVisualElement Copy()
        {
            return null;
        }
    }
}
