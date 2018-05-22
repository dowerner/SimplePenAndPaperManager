using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualPolygonalBuilding : VisualPolygon, IVisualBuilding
    {
        private IBuildingEntity _buildingSource;

        public VisualPolygonalBuilding(IBuildingEntity mapEntity) : base(mapEntity)
        {
            SelectedEntities = new ObservableCollection<IVisualElement>();
            SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
            ContentScale = 1;

            _buildingSource = mapEntity;
            Floors = new ObservableCollection<VisualFloor>();

            // fill floors
            FillFromSource();
            if (Floors.Count > 0) CurrentFloor = Floors[0];

            Floors.CollectionChanged += Floors_CollectionChanged;
        }

        #region Help Methods
        private void EntityChanged(object sender, PropertyChangedEventArgs e)
        {
            IVisualElement entity = (IVisualElement)sender;

            if (e.PropertyName == "IsSelected")
            {
                if (entity.IsSelected && !_selectedEntities.Contains(entity)) _selectedEntities.Add(entity);
                else if (!entity.IsSelected && _selectedEntities.Contains(entity)) _selectedEntities.Remove(entity);
            }
        }

        private void MapEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetBoundingDimensions();

            // listen for entity changes
            if (e.NewItems != null)
            {
                foreach (IVisualElement element in e.NewItems) element.PropertyChanged += EntityChanged;
            }
            if (e.OldItems != null)
            {
                foreach (IVisualElement element in e.OldItems) element.PropertyChanged -= EntityChanged;
            }
        }

        private void SelectedEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // handle items where selection was removed
            foreach (IVisualElement element in MapEntities)
            {
                if (!SelectedEntities.Contains(element)) element.IsSelected = false;
            }

            // set selection position
            _selectionLocation = new Point(0, 0);

            foreach (IVisualElement element in SelectedEntities)
            {
                _selectionLocation.X += element.X;
                _selectionLocation.Y += element.Y;
            }
            _selectionLocation.X /= SelectedEntities.Count;
            _selectionLocation.Y /= SelectedEntities.Count;

            OnPropertyChanged("EntitiesSelected");
            OnPropertyChanged("SelectionLocation");

            // set correct gizmo orientation
            if (_selectedEntities.Count == 1 && !GizmoIsRotating) GizmoOrientation = _selectedEntities[0].Orientation;
            else if (!GizmoIsRotating) GizmoOrientation = 0;
        }

        private void Floors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FillToSource();
        }

        private void FillFromSource()
        {
            foreach (IFloorEntity floor in _buildingSource.Floors)
            {
                Floors.Add((VisualFloor)VisualElementHelper.CreateFromMapEntity(floor));
            }
        }

        private void FillToSource()
        {
            foreach (VisualFloor floor in Floors)
            {
                if (!_buildingSource.Floors.Contains((IFloorEntity)floor.SourceEntity)) _buildingSource.Floors.Add((IFloorEntity)floor.SourceEntity);
            }
        }

        private void SetBoundingDimensions()
        {
            double x_max = Constants.StartMax, y_max = Constants.StartMax;
            double x_min = Constants.StartMin, y_min = Constants.StartMin;

            foreach(Point corner in Corners)
            {
                if (corner.X > x_max) x_max = corner.X;
                if (corner.X < x_min) x_min = corner.X;
                if (corner.Y > y_max) y_max = corner.Y;
                if (corner.Y < y_min) y_min = corner.Y;
            }

            double width = x_max - x_min;
            double height = y_max - y_min;

            MapWidth = Constants.BuildingBoundExtension * width;
            MapHeight = Constants.BuildingBoundExtension * height;
        }
        #endregion

        public void CreateFloorFromDimensions(Map map)
        {
            Floor floorSource = new Floor()
            {
                Id = map.GetNewId(),
                Name = "Floor",
            };

            VisualFloor floor = new VisualFloor(floorSource);

            SetBoundingDimensions();

            PointCollection points = new PointCollection();
            points.Add(Corners[0]);

            int i = 0;
            bool finished = false;
            Point offset = new Point(MapWidth / 2, MapHeight / 2);

            while(!finished)
            {
                Wall wall = new Wall() { Id = map.GetNewId(), Thickness = Constants.DefaultOutsideWallThickness, Name = "OuterWall" };
                WallElement visualWall = new WallElement(wall);
                visualWall.X1 = Corners[i].X + offset.X;
                visualWall.Y1 = Corners[i].Y + offset.Y;

                if (i == Corners.Count - 1)
                {
                    visualWall.X2 = Corners[0].X + offset.X;
                    visualWall.Y2 = Corners[0].Y + offset.Y;
                    finished = true;
                }
                else
                {
                    visualWall.X2 = Corners[i + 1].X + offset.X;
                    visualWall.Y2 = Corners[i + 1].Y + offset.Y;
                    i++;
                }
                floor.MapEntities.Add(VisualElementHelper.CreateFromMapEntity(wall));
                ((IFloorEntity)floor.SourceEntity).Walls.Add(wall);
            }

            Floors.Add(floor);
            ((IBuildingEntity)SourceEntity).Floors.Add((IFloorEntity)floor.SourceEntity);

            if (CurrentFloor == null) CurrentFloor = floor;
        }

        public ObservableCollection<IVisualElement> SelectedEntities
        {
            get { return _selectedEntities; }
            set
            {
                _selectedEntities = value;
                OnPropertyChanged("SelectedEntities");
            }
        }
        private ObservableCollection<IVisualElement> _selectedEntities;

        public Point SelectionLocation
        {
            get { return _selectionLocation; }
            set
            {
                _selectionLocation = value;
                OnPropertyChanged("SelectionLocation");
            }
        }
        private Point _selectionLocation;

        public IVisualElement LastSelected { get; set; }

        public VisualFloor CurrentFloor
        {
            get { return _currentFloor; }
            set
            {
                if (_currentFloor != null) _currentFloor.MapEntities.CollectionChanged -= MapEntities_CollectionChanged;
                _currentFloor = value;
                _currentFloor.MapEntities.CollectionChanged += MapEntities_CollectionChanged;
                OnPropertyChanged("CurrentFloor");
            }
        }
        private VisualFloor _currentFloor;

        public double MapWidth
        {
            get { return _mapWidth; }
            set
            {
                _mapWidth = value;
                OnPropertyChanged("MapWidth");
            }
        }
        private double _mapWidth;

        public double MapHeight
        {
            get { return _mapHeight; }
            set
            {
                _mapHeight = value;
                OnPropertyChanged("MapHeight");
            }
        }
        private double _mapHeight;

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
        public ICollectionView CharacterView { get { return CurrentFloor.CharacterView; } set { CurrentFloor.CharacterView = value; } }
        public ICollectionView ItemView { get { return CurrentFloor.ItemView; } set { CurrentFloor.ItemView = value; } }
        public ICollectionView WallsView { get { return CurrentFloor.WallsView; } set { CurrentFloor.WallsView = value; } }
        public ICollectionView DoorsView { get { return CurrentFloor.DoorsView; } set { CurrentFloor.DoorsView = value; } }
        public ICollectionView WindowsView { get { return CurrentFloor.WindowsView; } set { CurrentFloor.WindowsView = value; } }
        public ICollectionView MarkersView { get { return CurrentFloor.MarkersView; } set { CurrentFloor.MarkersView = value; } }

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

        public override BaseVisualElement Copy()
        {
            return new VisualPolygonalBuilding((IBuildingEntity)_polygonSource.Copy());
        }
    }
}
