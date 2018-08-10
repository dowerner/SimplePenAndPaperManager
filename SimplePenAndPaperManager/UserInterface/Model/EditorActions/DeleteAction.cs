using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System.Linq;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class DeleteAction : BaseAction
    {
        public VisualFloor RemovedFloor { get; set; }
        public int RemovedFloorIndex { get; set; }

        public override void Do()
        {
            foreach(IVisualElement entity in AffectedEntities)
            {
                _context.SelectedEntities.Remove(entity);

                // remove all building shape manipulation helpers
                var items = _context.MapEntities.Where(item => item is VisualCornerManipulator).ToList();
                for (int i = 0; i < items.Count; i++) _context.MapEntities.Remove(items[i]);
                if (_context is DataModel)
                {
                    ((DataModel)_context).CurrentCorners = null;
                }

                if(entity is WallElement)
                {
                    foreach (VisualDoor door in ((WallElement)entity).Doors) _context.MapEntities.Remove(door);
                    //TODO: Windows
                }
                _context.MapEntities.Remove(entity);
            }

            // handle floor removal
            if (_context is IVisualBuilding)
            {
                IVisualBuilding building = (IVisualBuilding)_context;
                VisualFloor floor = building.CurrentFloor;
                if (floor != null && floor.IsSelected)
                {
                    RemovedFloorIndex = building.Floors.IndexOf(floor);
                    building.Floors.Remove(floor);
                    RemovedFloor = floor;
                    if (building.Floors.Count > 0) building.CurrentFloor = building.Floors[0];
                    else building.CurrentFloor = null;
                }
            }
        }

        public override void Undo()
        {
            foreach (IVisualElement entity in AffectedEntities)
            {
                _context.MapEntities.Add(entity);
            }

            if(_context is IVisualBuilding && RemovedFloor != null)
            {
                IVisualBuilding building = (IVisualBuilding)_context;
                building.Floors.Insert(RemovedFloorIndex, RemovedFloor);
            }
        }

        public DeleteAction(ObservableCollection<IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
        }
    }
}
