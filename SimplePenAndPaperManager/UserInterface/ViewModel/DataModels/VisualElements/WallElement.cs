using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.ComponentModel;
using System.Windows;
using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.Model;
using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public class WallElement : BaseVisualElement
    {
        public List<IWindowEntity> Windows { get; set; } 
        public ObservableCollection<VisualDoor> Doors { get; set; }

        public bool IsOuterWall
        {
            get { return _wallEnity.IsOuterWall; }
            set { _wallEnity.IsOuterWall = value; }
        }

        public double Thickness
        {
            get { return _wallEnity.Thickness; }
            set
            {
                _wallEnity.Thickness = value;
                OnPropertyChanged("Thickness");
            }
        }

        public double Length
        {
            get { return _wallEnity.Length; }
            set
            {
                _wallEnity.Length = value;
                CalculatePositionAndSize(X, Y);
                OnPropertyChanged("X1");
                OnPropertyChanged("Y1");
                OnPropertyChanged("X2");
                OnPropertyChanged("Y2");
                OnPropertyChanged("Length");
            }
        }

        public override double X
        {
            get { return _wallEnity.X; }
            set
            {
                _wallEnity.X = value;
                CalculatePositionAndSize(X, Y);
                OnPropertyChanged("X1");
                OnPropertyChanged("Y1");
                OnPropertyChanged("X2");
                OnPropertyChanged("Y2");
                OnPropertyChanged("X");
            }
        }

        public override double Y
        {
            get { return _wallEnity.Y; }
            set
            {
                _wallEnity.Y = value;
                CalculatePositionAndSize(X, Y);
                OnPropertyChanged("X1");
                OnPropertyChanged("Y1");
                OnPropertyChanged("X2");
                OnPropertyChanged("Y2");
                OnPropertyChanged("Y");
            }
        }

        public BuildingMaterial Material
        {
            get { return _wallEnity.Material; }
            set
            {
                _wallEnity.Material = value;
                OnPropertyChanged("Material");
            }
        }

        public double X1
        {
            get { return _x1; }
            set
            {
                _x1 = value;
                CalculatePositionAndSize(_x1, _y1, _x2, _y2);
                OnPropertyChanged("X1");
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
                OnPropertyChanged("Orientation");
                OnPropertyChanged("Length");
            }
        }
        private double _x1;

        public double Y1
        {
            get { return _y1; }
            set
            {
                _y1 = value;
                CalculatePositionAndSize(_x1, _y1, _x2, _y2);
                OnPropertyChanged("Y1");
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
                OnPropertyChanged("Orientation");
                OnPropertyChanged("Length");
            }
        }
        private double _y1;

        public double X2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                CalculatePositionAndSize(_x1, _y1, _x2, _y2);
                OnPropertyChanged("X2");
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
                OnPropertyChanged("Orientation");
                OnPropertyChanged("Length");
            }
        }
        private double _x2;

        public double Y2
        {
            get { return _y2; }
            set
            {
                _y2 = value;
                CalculatePositionAndSize(_x1, _y1, _x2, _y2);
                OnPropertyChanged("Y2");
                OnPropertyChanged("X");
                OnPropertyChanged("Y");
                OnPropertyChanged("Orientation");
                OnPropertyChanged("Length");
            }
        }
        private double _y2;

        public override IVisualElement Copy()
        {
            return new WallElement((IWallEntity)_wallEnity.Copy());
        }

        private IWallEntity _wallEnity;

        public WallElement(IWallEntity mapEntity) : base(mapEntity)
        {
            _wallEnity = mapEntity;
            Windows = new List<IWindowEntity>();
            Doors = new ObservableCollection<VisualDoor>();
            CalculatePositionAndSize(X, Y);
            StrokeColor = Colors.Black;
            Thickness = Constants.DefaultOutsideWallThickness;
            PropertyChanged += WallElement_PropertyChanged;
            Doors.CollectionChanged += Doors_CollectionChanged;
        }

        private void Doors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null) foreach (VisualDoor door in e.NewItems) _wallEnity.Doors.Add((IDoorEntity)door.SourceEntity);
            if(e.OldItems != null) foreach (VisualDoor door in e.OldItems) _wallEnity.Doors.Remove((IDoorEntity)door.SourceEntity);
        }

        public bool Contains(Point point, double thresholdDistance=Constants.ThresholdDistanceForWallAttachables)
        {
            return Utils.RectangleContains(point, new Point(X, Y), Length, Thickness + thresholdDistance, Orientation);
        }

        public Point ClosestPointOnWall(Point point)
        {
            Point centerOffset = new Point(0, -Thickness / 2).Rotate(Orientation);
            Point a = new Point(X1, Y1);
            Point ab = new Point(X2 - X1, Y2 - Y1);
            Point ap = point.Sub(a);
            double length = Math.Sqrt(ab.X * ab.X + ab.Y * ab.Y);
            Point vec = ab.Mult(1 / length);
            double dp = ap.Dot(ab) / (length * length);
            return centerOffset.Add(a.Add(ab.Mult(dp)));
        }

        private void WallElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Orientation")
            {
                CalculatePositionAndSize(X, Y);
                OnPropertyChanged("X1");
                OnPropertyChanged("Y1");
                OnPropertyChanged("X2");
                OnPropertyChanged("Y2");
            }
        }

        #region Help Functions
        private void CalculatePositionAndSize(double x, double y)
        {
            double angle = Orientation * Math.PI / 180;
            _x1 = x - Math.Cos(angle) * Length / 2;
            _y1 = y - Math.Sin(angle) * Length / 2;
            _x2 = x + Math.Cos(angle) * Length / 2;
            _y2 = y + Math.Sin(angle) * Length / 2;
        }

        private void CalculatePositionAndSize(double x1, double y1, double x2, double y2)
        {
            _wallEnity.Orientation = Math.Atan2(y2 - y1, x2 - x1) * 180 / Math.PI;
            _wallEnity.Length = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            double dx = (x2 - x1) / 2;
            double dy = (y2 - y1) / 2;

            _wallEnity.X = x1 + dx;
            _wallEnity.Y = y1 + dy;
        }
        #endregion
    }
}
