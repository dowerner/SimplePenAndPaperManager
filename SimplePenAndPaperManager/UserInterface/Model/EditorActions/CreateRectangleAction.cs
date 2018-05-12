using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreateRectangleAction : BaseAction
    {
        public VisualRectangle Building { get; set; }

        public override void Do()
        {
            DataModel.Instance.MapEntities.Add(Building);
            DataModel.Instance.SelectedEntities.Clear();
            Building.IsSelected = true;
        }

        public override void Undo()
        {
            DataModel.Instance.MapEntities.Remove(Building);
            if (DataModel.Instance.SelectedEntities.Contains(Building))
            {
                DataModel.Instance.SelectedEntities.Remove(Building);
            }
        }

        public CreateRectangleAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }
    }
}
