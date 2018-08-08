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
        }

        public override void Undo()
        {
            foreach (IVisualElement entity in AffectedEntities)
            {
                _context.MapEntities.Add(entity);
            }
        }

        public DeleteAction(ObservableCollection<IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
        }
    }
}
