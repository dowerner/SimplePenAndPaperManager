using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class DeleteAction : BaseAction
    {
        public override void Do()
        {
            foreach(IVisualElement entity in AffectedEntities)
            {
                DataModel.Instance.SelectedEntities.Remove(entity);
                DataModel.Instance.MapEntities.Remove(entity);
            }
        }

        public override void Undo()
        {
            foreach (IVisualElement entity in AffectedEntities)
            {
                DataModel.Instance.MapEntities.Add(entity);
            }
        }

        public DeleteAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }
    }
}
