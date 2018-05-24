using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualPolygonalBuilding : BaseDataModel, IVisualBuilding
    {
        private IBuildingEntity _buildingSource;
        protected VisualPolygon _polygonBuildingSource;

        public VisualPolygonalBuilding(IBuildingEntity mapEntity) : base()
        {
            _polygonBuildingSource = new VisualPolygon(mapEntity);
            _polygonBuildingSource.PropertyChanged += _polygonBuildingSource_PropertyChanged;

            // setup
            _buildingSource = mapEntity;
            Floors = new ObservableCollection<VisualFloor>();

            // fill floors
            FillFromSource();
            if (Floors.Count > 0) CurrentFloor = Floors[0];

            Floors.CollectionChanged += Floors_CollectionChanged;
        }

        private void _polygonBuildingSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #region Help Methods
        private void EntityChanged(object sender, PropertyChangedEventArgs e)
        {
            IVisualElement entity = (IVisualElement)sender;

            if (e.PropertyName == "IsSelected")
            {
                if (entity.IsSelected && !SelectedEntities.Contains(entity)) SelectedEntities.Add(entity);
                else if (!entity.IsSelected && SelectedEntities.Contains(entity)) SelectedEntities.Remove(entity);
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

        public void CreateFloorFromDimensions()
        {
            Floor floorSource = new Floor()
            {
                Id = 0,
                Name = "Floor",
            };

            VisualFloor floor = new VisualFloor(floorSource);

            SetBoundingDimensions();

            int i = 0;
            bool finished = false;
            Point offset = new Point(MapWidth / 2, MapHeight / 2);

            while(!finished)
            {
                Wall wall = new Wall() { Id = 0, Thickness = Constants.DefaultOutsideWallThickness, Name = "OuterWall" };
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
                floor.MapEntities.Add(visualWall);
            }
            Floors.Add(floor);

            if (CurrentFloor == null) CurrentFloor = floor;
        }

        public VisualFloor CurrentFloor
        {
            get { return _currentFloor; }
            set
            {
                if (_currentFloor != null)
                {
                    foreach (IVisualElement entity in _currentFloor.MapEntities) entity.PropertyChanged -= EntityChanged;
                    _currentFloor.MapEntities.CollectionChanged -= MapEntities_CollectionChanged;
                }
                _currentFloor = value;
                OnPropertyChanged("CurrentFloor");
                if (_currentFloor == null) return;
                SetBoundingDimensions();

                _currentFloor.MapEntities.CollectionChanged += MapEntities_CollectionChanged;
                foreach (IVisualElement entity in _currentFloor.MapEntities) entity.PropertyChanged += EntityChanged;
            }
        }
        private VisualFloor _currentFloor;

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

        public PointCollection Corners
        {
            get { return _polygonBuildingSource.Corners; }
            set { _polygonBuildingSource.Corners = value; }
        }

        public Color Color
        {
            get { return _polygonBuildingSource.Color; }
            set { _polygonBuildingSource.Color = value; }
        }

        public Color StrokeColor
        {
            get { return _polygonBuildingSource.StrokeColor; }
            set { _polygonBuildingSource.StrokeColor = value; }
        }

        public double CenterX
        {
            get { return _polygonBuildingSource.CenterX; }
            set { _polygonBuildingSource.CenterX = value; }
        }

        public double CenterY
        {
            get { return _polygonBuildingSource.CenterY; }
            set { _polygonBuildingSource.CenterY = value; }
        }

        public double BoundingWidth
        {
            get { return _polygonBuildingSource.BoundingWidth; }
            set { _polygonBuildingSource.BoundingWidth = value; }
        }

        public double BoundingHeight
        {
            get { return _polygonBuildingSource.BoundingHeight; }
            set { _polygonBuildingSource.BoundingHeight = value; }
        }

        public double X
        {
            get { return _polygonBuildingSource.X; }
            set { _polygonBuildingSource.X = value; }
        }

        public double Y
        {
            get { return _polygonBuildingSource.Y; }
            set { _polygonBuildingSource.Y = value; }
        }

        public double Orientation
        {
            get { return _polygonBuildingSource.Orientation; }
            set { _polygonBuildingSource.Orientation = value; }
        }

        public int Id
        {
            get { return _polygonBuildingSource.Id; }
            set { _polygonBuildingSource.Id = value; }
        }

        public string Name
        {
            get { return _polygonBuildingSource.Name; }
            set { _polygonBuildingSource.Name = Name; }
        }

        public bool IsSelected
        {
            get { return _polygonBuildingSource.IsSelected; }
            set { _polygonBuildingSource.IsSelected = value; }
        }

        public IMapEntity SourceEntity
        {
            get { return _polygonBuildingSource.SourceEntity; }
            set { _polygonBuildingSource.SourceEntity = value; }
        }
        #endregion

        public virtual IVisualElement Copy()
        {
            VisualPolygonalBuilding copy = new VisualPolygonalBuilding((IBuildingEntity)_buildingSource.Copy());
            copy.CurrentFloor = copy.Floors[Floors.IndexOf(CurrentFloor)];
            return copy;
        }
    }
}
