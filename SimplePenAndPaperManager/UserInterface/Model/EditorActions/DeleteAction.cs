using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class DeleteAction : BaseAction
    {
        public override void Do()
        {
            foreach(IVisualElement entity in AffectedEntities)
            {
                _context.SelectedEntities.Remove(entity);
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
