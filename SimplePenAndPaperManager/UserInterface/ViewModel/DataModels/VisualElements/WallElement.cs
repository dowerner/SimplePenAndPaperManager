using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using System;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public class WallElement : BaseVisualElement
    {
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

        public new double X
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

        public new double Y
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
            Thickness = 1;
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
