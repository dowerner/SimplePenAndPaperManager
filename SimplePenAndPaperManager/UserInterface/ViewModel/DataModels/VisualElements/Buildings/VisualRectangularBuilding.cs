using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Windows;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualRectangularBuilding : VisualPolygonalBuilding
    {
        public Point A
        {
            get { return Corners[0]; }
            set
            {
                Corners[0] = value;

                // update other corners to keep rectangle
                Corners[1] = new Point(Corners[2].X, Corners[0].Y);
                Corners[3] = new Point(Corners[0].X, Corners[2].Y);

                OnPropertyChanged("Corners");
                UpdateDimensions();
            }
        }

        public Point B
        {
            get { return Corners[1]; }
            set
            {
                Corners[1] = value;

                // update other corners to keep rectangle
                Corners[0] = new Point(Corners[3].X, Corners[1].Y);
                Corners[2] = new Point(Corners[1].X, Corners[3].Y);

                OnPropertyChanged("Corners");
                UpdateDimensions();
            }
        }

        public Point C
        {
            get { return Corners[2]; }
            set
            {
                Corners[2] = value;

                // update other corners to keep rectangle
                Corners[1] = new Point(Corners[2].X, Corners[0].Y);
                Corners[3] = new Point(Corners[0].X, Corners[2].Y);

                OnPropertyChanged("Corners");
                UpdateDimensions();
            }
        }

        public Point D
        {
            get { return Corners[3]; }
            set
            {
                Corners[3] = value;

                // update other corners to keep rectangle
                Corners[0] = new Point(Corners[3].X, Corners[1].Y);
                Corners[2] = new Point(Corners[1].X, Corners[3].Y);

                OnPropertyChanged("Corners");
                UpdateDimensions();
            }
        }

        public double Width
        {
            get { return _rectangleSource.Width; }
            set
            {
                _rectangleSource.Width = value;
                OnPropertyChanged("Width");
                UpdateCorners();
            }
        }

        public double Height
        {
            get { return _rectangleSource.Height; }
            set
            {
                _rectangleSource.Height = value;
                OnPropertyChanged("Height");
                UpdateCorners();
            }
        }

        private void UpdateCorners()
        {
            Corners[0] = new Point(-Width / 2, -Height / 2);
            Corners[1] = new Point(Width / 2, -Height / 2);
            Corners[2] = new Point(Width / 2, Height / 2);
            Corners[3] = new Point(-Width / 2, Height / 2);
            OnPropertyChanged("Corners");
        }

        private void UpdateDimensions()
        {
            _rectangleSource.Width = B.X - A.X;
            _rectangleSource.Height = D.Y - A.Y;
            OnPropertyChanged("Width");
            OnPropertyChanged("Height");
        }

        private IRectangularBuildingEntity _rectangleSource;

        public VisualRectangularBuilding(IRectangularBuildingEntity mapEntity) : base(mapEntity)
        {
            _rectangleSource = mapEntity;
            _corners = new PointCollection();
            for (int i = 0; i < 4; i++) _corners.Add(new Point());
            UpdateCorners();
        }

        public override BaseVisualElement Copy()
        {
            return new VisualRectangularBuilding((IRectangularBuildingEntity)_rectangleSource.Copy());
        }
    }
}
