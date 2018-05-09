using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreateRectangleAction : BaseAction
    {
        public RectangleElement CreatedElement { get; set; }

        public override void Do()
        {
            DataModel.Instance.MapEntities.Add(CreatedElement);
            DataModel.Instance.SelectedEntities.Clear();
            DataModel.Instance.SelectedEntities.Add(CreatedElement);
        }

        public override void Undo()
        {
            DataModel.Instance.MapEntities.Remove(CreatedElement);
            if (DataModel.Instance.SelectedEntities.Contains(CreatedElement))
            {
                DataModel.Instance.SelectedEntities.Remove(CreatedElement);
            }
        }

        public CreateRectangleAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }
    }
}
