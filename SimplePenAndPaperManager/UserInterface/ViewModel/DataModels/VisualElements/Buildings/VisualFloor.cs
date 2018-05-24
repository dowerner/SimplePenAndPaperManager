using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.ObjectModel;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.ComponentModel;
using System.Windows.Data;
using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Characters.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualFloor : BaseVisualElement
    {
        public ICollectionView CharacterView { get; set; }
        public ICollectionView ItemView { get; set; }
        public ICollectionView WallsView { get; set; }
        public ICollectionView DoorsView { get; set; }
        public ICollectionView WindowsView { get; set; }
        public ICollectionView MarkersView { get; set; }

        public ObservableCollection<IVisualElement> MapEntities
        {
            get { return _mapEntities; }
            set
            {
                _mapEntities = value;

                CharacterView = new CollectionViewSource() { Source = _mapEntities }.View;
                CharacterView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is ICharacterEntity; };
                ItemView = new CollectionViewSource() { Source = _mapEntities }.View;
                ItemView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IItemEntity; };
                WallsView = new CollectionViewSource() { Source = _mapEntities }.View;
                WallsView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IWallEntity; };
                DoorsView = new CollectionViewSource() { Source = _mapEntities }.View;
                DoorsView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IDoorEntity; };
                WindowsView = new CollectionViewSource() { Source = _mapEntities }.View;
                WindowsView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IWindowEntity; };
                MarkersView = new CollectionViewSource() { Source = _mapEntities }.View;
                MarkersView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IMarkerEntity; };

                fillToSource();
                OnPropertyChanged("MapEntities");
            }
        }
        private ObservableCollection<IVisualElement> _mapEntities;

        private IFloorEntity _sourceFloor;

        public VisualFloor(IFloorEntity mapEntity) : base(mapEntity)
        {
            MapEntities = new ObservableCollection<IVisualElement>();
            _mapEntities.CollectionChanged += _mapEntities_CollectionChanged;
            _sourceFloor = mapEntity;
            fillFromSource();
        }

        private void _mapEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            fillToSource();
        }

        public override IVisualElement Copy()
        {
            return new VisualFloor((IFloorEntity)_sourceFloor.Copy());
        }

        private void fillFromSource()
        {
            _mapEntities.Clear();

            // fill in walls
            foreach (IWallEntity wall in _sourceFloor.Walls)
            {
                _mapEntities.Add(new WallElement(wall));
                foreach (IDoorEntity door in wall.Doors) _mapEntities.Add(null);    //TODO: Create visual door 
                foreach (IDoorEntity door in wall.Windows) _mapEntities.Add(null);    //TODO: Create visual window
            }

            // add other entities
            foreach (IMapEntity entity in _sourceFloor.Entities) _mapEntities.Add(VisualElementHelper.CreateFromMapEntity(entity));
        }

        private void fillToSource()
        {
            foreach(IVisualElement entity in MapEntities)
            {
                if(entity.SourceEntity is IWallEntity)
                {
                    if (!_sourceFloor.Walls.Contains((IWallEntity)entity.SourceEntity)) _sourceFloor.Walls.Add((IWallEntity)entity.SourceEntity);
                }
                else if(!(entity.SourceEntity is IDoorEntity) && !(entity.SourceEntity is IWindowEntity))
                {
                    if (!_sourceFloor.Entities.Contains(entity.SourceEntity)) _sourceFloor.Entities.Add(entity.SourceEntity);
                }
            }
        }
    }
}
