using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.Model;
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
                _x = x + _polygon.X;
                return _x;
            }
            set
            {
                double angle = _polygon.Orientation * Math.PI / 180;
                double x = Math.Cos(angle) * (value - _polygon.X) + Math.Sin(angle) * (_y - _polygon.Y);

                Point newPoint = new Point(x, _polygon.Corners[_cornerIndex].Y);

                if (_polygon is VisualRectangularBuilding)
                {
                    switch (_cornerIndex)
                    {
                        case 0:
                            ((VisualRectangularBuilding)_polygon).A = newPoint;
                            break;
                        case 1:
                            ((VisualRectangularBuilding)_polygon).B = newPoint;
                            break;
                        case 2:
                            ((VisualRectangularBuilding)_polygon).C = newPoint;
                            break;
                        case 3:
                            ((VisualRectangularBuilding)_polygon).D = newPoint;
                            break;
                    }
                }
                else
                {
                    _polygon.Corners[_cornerIndex] = newPoint;
                }
                
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
                _y = y + _polygon.Y;
                return _y;
            }
            set
            {
                double angle = _polygon.Orientation * Math.PI / 180;
                double y = -Math.Sin(angle) * (_x - _polygon.X) + Math.Cos(angle) * (value - _polygon.Y);

                Point newPoint = new Point(_polygon.Corners[_cornerIndex].X, y);

                if (_polygon is VisualRectangularBuilding)
                {
                    switch (_cornerIndex)
                    {
                        case 0:
                            ((VisualRectangularBuilding)_polygon).A = newPoint;
                            break;
                        case 1:
                            ((VisualRectangularBuilding)_polygon).B = newPoint;
                            break;
                        case 2:
                            ((VisualRectangularBuilding)_polygon).C = newPoint;
                            break;
                        case 3:
                            ((VisualRectangularBuilding)_polygon).D = newPoint;
                            break;
                    }
                }
                else
                {
                    _polygon.Corners[_cornerIndex] = newPoint;
                }

                _polygon.Corners[_cornerIndex] = newPoint;
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
                OnPropertyChanged("X");
                OnPropertyChanged("XOffset");
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
                OnPropertyChanged("Y");
                OnPropertyChanged("YOffset");
            }
        }
        private double _height;

        public double XOffset
        {
            get { return -_width / 2; }
        }

        public double YOffset
        {
            get { return -_height / 2; }
        }

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
            Width = Constants.ManipulatorDiameter;
            Height = Constants.ManipulatorDiameter;
            _polygon = polygon;
            _polygon.PropertyChanged += _polygon_PropertyChanged;
            _cornerIndex = index;
        }

        private void _polygon_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y" || e.PropertyName == "A" || e.PropertyName == "B" || e.PropertyName == "C" || e.PropertyName == "D")
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
