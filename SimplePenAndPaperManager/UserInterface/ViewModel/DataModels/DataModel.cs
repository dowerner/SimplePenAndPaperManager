using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels
{
    /// <summary>
    /// A simple example of a data-model.  
    /// The purpose of this data-model is to share display data between the main window and overview window.
    /// </summary>
    public class DataModel : BaseDataModel
    {
        public DataModel() : base()
        {
            base.MapEntities = new ObservableCollection<IVisualElement>();
            MapEntities.CollectionChanged += MapEntities_CollectionChanged;

            CurrentMap = new Map() { Width = 400, Height = 400 };
        }

        private void MapEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // listen for entity changes
            if (e.NewItems != null)
            {
                foreach (IVisualElement element in e.NewItems)
                {
                    element.PropertyChanged += EntityChanged;
                    CurrentMap.Entities.Add(element.SourceEntity);

                    if (element is VisualPolygonalBuilding) CurrentCorners = ((VisualPolygonalBuilding)element).Corners;
                }
            }
            if (e.OldItems != null)
            {
                foreach (IVisualElement element in e.OldItems)
                {
                    element.PropertyChanged -= EntityChanged;
                    CurrentMap.Entities.Remove(element.SourceEntity);
                }
            }
        }
        
        private void EntityChanged(object sender, PropertyChangedEventArgs e)
        {
            VisualElements.Interface.IVisualElement entity = (VisualElements.Interface.IVisualElement)sender;

            if (e.PropertyName == "IsSelected")
            {
                if (entity.IsSelected && !SelectedEntities.Contains(entity)) SelectedEntities.Add(entity);
                else if (!entity.IsSelected && SelectedEntities.Contains(entity)) SelectedEntities.Remove(entity);
            }
        }

        #region Creation Variables
        public List<WallElement> NewPolygonalBuildingWalls { get; set; }
        public WallElement CurrentPolygonWall { get; set; }

        public PointCollection CurrentCorners
        {
            get { return _currentCorners; }
            set
            {
                _currentCorners = value;
                OnPropertyChanged("CurrentCorners");
            }
        }
        private PointCollection _currentCorners;

        public string Debug
        {
            get { return _debug; }
            set
            {
                _debug = value;
                OnPropertyChanged("Debug");
            }
        }
        private string _debug;
        #endregion

        #region Creation Methods
        private void InitiateRectangularBuilding()
        {
            GlobalManagement.Instance.BuildingStartLocation = MousePosition.PxToMeter();
            GlobalManagement.Instance.NewRectangleBuilding = new VisualRectangularBuilding(new RectangularBuilding()
            {
                X = GlobalManagement.Instance.BuildingStartLocation.X,
                Y = GlobalManagement.Instance.BuildingStartLocation.Y,
                Width = 0,
                Height = 0,
                Id = CurrentMap.GetNewId(),
                Name = Constants.DefaultHouseName
            });
            MapEntities.Add(GlobalManagement.Instance.NewRectangleBuilding);
        }

        private void InitiatePolygonalBuilding()
        {
            if (CurrentPolygonWall == null)
            {
                // save mouse start position
                GlobalManagement.Instance.BuildingStartLocation = MousePosition.PxToMeter();
                NewPolygonalBuildingWalls = new List<WallElement>();
            }
            // create wall of building
            CurrentPolygonWall = new WallElement(new Wall());
            Point meterPosition = MousePosition.PxToMeter();
            CurrentPolygonWall.X1 = meterPosition.X;
            CurrentPolygonWall.Y1 = meterPosition.Y;
            CurrentPolygonWall.X2 = meterPosition.X;
            CurrentPolygonWall.Y2 = meterPosition.Y;

            NewPolygonalBuildingWalls.Add(CurrentPolygonWall);

            // add wall to map
            MapEntities.Add(CurrentPolygonWall);
        }

        public void StartBuilding()
        {
            if (GlobalManagement.Instance.IsCreatingRectangularBuilding)
            {
                InitiateRectangularBuilding();
            }
            else if (GlobalManagement.Instance.IsCreatingPolygonalBuilding)
            {
                InitiatePolygonalBuilding();
            }
        }
        #endregion

        public Map CurrentMap
        {
            get { return _currentMap; }
            set
            {
                MapEntities.Clear();
                if (_currentMap != null) _currentMap.Terrain.StrokesChanged -= GlobalManagement.Instance.TerrainStrokes_StrokesChanged;
                _currentMap = value;

                if (_currentMap.Entities == null) _currentMap.Entities = new List<IMapEntity>();
                if (_currentMap.Terrain == null) _currentMap.Terrain = new StrokeCollection();

                GlobalManagement.Instance.TerrainStrokes = _currentMap.Terrain;
                GlobalManagement.Instance.TerrainStrokes.StrokesChanged += GlobalManagement.Instance.TerrainStrokes_StrokesChanged;

                OnPropertyChanged("CurrentMap");

                foreach (IMapEntity enity in _currentMap.Entities) MapEntities.Add(VisualElementHelper.CreateFromMapEntity(enity));
            }
        }
        private Map _currentMap;

        public override double MapWidth
        {
            get { return _currentMap.Width; }
            set { _currentMap.Height = value; }
        }

        public override double MapHeight
        {
            get { return _currentMap.Width; }
            set { _currentMap.Height = value; }
        }
    }
}
