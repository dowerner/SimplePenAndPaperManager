using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreateTextMarkerAction : BaseAction
    {
        public VisualTextMarker Marker { get; set; }

        public override void Do()
        {
            _context.MapEntities.Add(Marker);
            _context.SelectedEntities.Clear();
            Marker.IsSelected = true;
        }

        public override void Undo()
        {
            Marker.IsSelected = false;
            _context.MapEntities.Remove(Marker);
        }

        public CreateTextMarkerAction(ObservableCollection<IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
        }
    }
}
