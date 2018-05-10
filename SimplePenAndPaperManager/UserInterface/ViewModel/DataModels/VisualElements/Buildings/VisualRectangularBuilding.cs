using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Windows;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualRectangularBuilding : PolygonElement
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
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
                UpdateCorners();
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
                UpdateCorners();
            }
        }
        private double _height;

        private IRectangularMapEntity _rectangleSource;

        public VisualRectangularBuilding(IRectangularMapEntity mapEntity) : base(mapEntity)
        {
            _rectangleSource = mapEntity;
            _corners = new PointCollection();
            for (int i = 0; i < 4; i++) _corners.Add(new Point());
        }

        public override IVisualElement Copy()
        {
            return new VisualRectangularBuilding((IRectangularMapEntity)_rectangleSource);
        }

        private void UpdateCorners()
        {
            A = new Point(-Width / 2, -Height / 2);
            B = new Point(Width / 2, -Height / 2);
            C = new Point(Width / 2, Height / 2);
            D = new Point(-Width / 2, Height / 2);
        }

        private void UpdateDimensions()
        {
            _width = B.X - A.X;
            _height = D.Y - A.Y;
            OnPropertyChanged("Width");
            OnPropertyChanged("Height");
        }
    }
}
