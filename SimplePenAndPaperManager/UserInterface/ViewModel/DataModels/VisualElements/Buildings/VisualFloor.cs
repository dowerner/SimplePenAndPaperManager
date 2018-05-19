using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.ObjectModel;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualFloor : BaseVisualElement
    {
        public ObservableCollection<IVisualElement> MapEntities
        {
            get { return _mapEntities; }
            set
            {
                _mapEntities = value;
                fillToSource();
                OnPropertyChanged("MapEntities");
            }
        }
        private ObservableCollection<IVisualElement> _mapEntities;

        private IFloorEntity _sourceFloor;

        public VisualFloor(IFloorEntity mapEntity) : base(mapEntity)
        {
            _mapEntities = new ObservableCollection<IVisualElement>();
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
