using System.Collections.ObjectModel;
using System.Windows;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualRectangularBuilding : VisualRectangle, IVisualBuilding
    {
        public VisualRectangularBuilding(IRectangularMapEntity mapEntity) : base(mapEntity)
        {}

        public ObservableCollection<IVisualElement> SelectedEntities { get; set; }

        public Point SelectionLocation { get; set; }

        public VisualFloor CurrentFloor
        {
            get { return _currentFloor; }
            set
            {
                _currentFloor = value;
                OnPropertyChanged("CurrentFloor");
            }
        }
        private VisualFloor _currentFloor;

        public double MapWidth
        {
            get { return _currentFloor.BoundingWidth; }
            set { _currentFloor.BoundingWidth = value; }
        }

        public double MapHeight
        {
            get { return _currentFloor.BoundingHeight; }
            set { _currentFloor.BoundingHeight = value; }
        }

        public ObservableCollection<VisualFloor> Floors
        {
            get { return _floors; }
            set
            {
                _floors = value;
                OnPropertyChanged("Floors");
            }
        }
        private ObservableCollection<VisualFloor> _floors;

        #region DataModel Implementation
        public ObservableCollection<IVisualElement> MapEntities
        {
            get { return _currentFloor.MapEntities; }
            set
            {
                _currentFloor.MapEntities = value;
                OnPropertyChanged("MapEntities");
            }
        }

        public double ContentScale
        {
            get { return _contentScale; }
            set
            {
                _contentScale = value;
                OnPropertyChanged("ContentScale");
            }
        }
        private double _contentScale;

        public double ContentOffsetX
        {
            get { return _contentOffsetX; }
            set
            {
                _contentOffsetX = value;
                OnPropertyChanged("ContentOffsetX");
            }
        }
        private double _contentOffsetX;

        public double ContentOffsetY
        {
            get { return _contentOffsetY; }
            set
            {
                _contentOffsetY = value;
                OnPropertyChanged("ContentOffsetY");
            }
        }
        private double _contentOffsetY;

        public double ContentViewportWidth
        {
            get { return _contentViewportWidth; }
            set
            {
                _contentViewportWidth = value;
                OnPropertyChanged("ContentViewportWidth");
            }
        }
        private double _contentViewportWidth;

        public double ContentViewportHeight
        {
            get { return _contentViewportHeight; }
            set
            {
                _contentViewportHeight = value;
                OnPropertyChanged("ContentViewportHeight");
            }
        }
        private double _contentViewportHeight;

        public double ContentWidth
        {
            get { return _contentWidth; }
            set
            {
                _contentWidth = value;
                OnPropertyChanged("ContentWidth");
            }
        }
        private double _contentWidth;

        public double ContentHeight
        {
            get { return _contentHeight; }
            set
            {
                _contentHeight = value;
                OnPropertyChanged("ContentHeight");
            }
        }
        private double _contentHeight;

        public double GizmoOrientation
        {
            get { return _gizmoOrientation; }
            set
            {
                _gizmoOrientation = value;
                OnPropertyChanged("GizmoOrientation");
            }
        }
        private double _gizmoOrientation;

        public bool EntitiesSelected
        {
            get { return _entitiesSelected; }
        }
        private bool _entitiesSelected;

        public bool GizmoIsRotating
        {
            get { return _gizmoIsRotating; }
            set
            {
                _gizmoIsRotating = value;
                OnPropertyChanged("GizmoIsRotating");
            }
        }
        private bool _gizmoIsRotating;

        public bool GizmoDragX
        {
            get { return _gizmoDragX; }
            set
            {
                _gizmoDragX = value;
                OnPropertyChanged("GizmoDragX");
            }
        }
        private bool _gizmoDragX;

        public bool GizmoDragY
        {
            get { return _gizmoDragY; }
            set
            {
                _gizmoDragY = value;
                OnPropertyChanged("GizmoDragY");
            }
        }
        private bool _gizmoDragY;

        public double GizmoX
        {
            get { return _gizmoX; }
            set
            {
                _gizmoX = value;
                OnPropertyChanged("GizmoX");
            }
        }
        private double _gizmoX;

        public double GizmoY
        {
            get { return _gizmoY; }
            set
            {
                _gizmoY = value;
                OnPropertyChanged("GizmoY");
            }
        }
        private double _gizmoY;

        public Point MousePosition
        {
            get { return _mousePosition; }
            set
            {
                _mousePosition = value;
                OnPropertyChanged("MousePosition");
            }
        }
        private Point _mousePosition;

        /// <summary>
        /// Can only be false since there is no terrain inside of a building.
        /// </summary>
        public bool InTerrainEditingMode
        {
            get { return false; }
            set { } // cannot be set
        }

        /// <summary>
        /// Can only be false since there is no terrain inside of a building.
        /// </summary>
        public bool ShowTerrainEllipse
        {
            get { return false; }
        }

        /// <summary>
        /// Can only be false since there is no terrain inside of a building.
        /// </summary>
        public bool ShowTerrainRectangle
        {
            get { return false; }
        }
        #endregion

        public override IVisualElement Copy()
        {
            return new VisualRectangularBuilding((IRectangularMapEntity)_rectangleSource.Copy());
        }
    }
}
