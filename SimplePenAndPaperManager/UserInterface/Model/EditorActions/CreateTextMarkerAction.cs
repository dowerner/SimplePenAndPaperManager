using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreateTextMarkerAction : BaseAction
    {
        public VisualTextMarker Marker { get; set; }

        public override void Do()
        {
            DataModel.Instance.MapEntities.Add(Marker);
            DataModel.Instance.SelectedEntities.Clear();
            Marker.IsSelected = true;
        }

        public override void Undo()
        {
            Marker.IsSelected = false;
            DataModel.Instance.MapEntities.Remove(Marker);
        }

        public CreateTextMarkerAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }
    }
}
