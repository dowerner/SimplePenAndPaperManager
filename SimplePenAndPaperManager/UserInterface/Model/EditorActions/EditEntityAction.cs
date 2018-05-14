using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;
using SimplePenAndPaperManager.UserInterface.View.Controls;
using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class EditEntityAction : BaseAction
    {
        public IMapEntity OldData { get; set; }
        public IMapEntity NewData { get; set; }
        public IVisualElement AffectedElement { get; set; }

        public EditEntityAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }

        public override void Do()
        {
            OldData = AffectedElement.SourceEntity.Copy();
            OldData.X = AffectedElement.SourceEntity.X;
            OldData.Y = AffectedElement.SourceEntity.Y;

            if (NewData != null) MoveData(NewData);
            else
            {
                if (AffectedElement is VisualTextMarker) TextMarkerInputBox.SetMarkerData((VisualTextMarker)AffectedElement);
            }
        }

        public override void Undo()
        {
            NewData = AffectedElement.SourceEntity.Copy();
            NewData.X = AffectedElement.SourceEntity.X;
            NewData.Y = AffectedElement.SourceEntity.Y;

            MoveData(OldData);
        }

        private void MoveData(IMapEntity source)
        {
            AffectedElement.X = source.X;
            AffectedElement.Y = source.Y;
            AffectedElement.Orientation = source.Orientation;
            AffectedElement.Name = source.Name;

            if (AffectedElement is VisualTextMarker)
            {
                ((VisualTextMarker)AffectedElement).Text = ((ITextMarkerEntity)source).Text;
            }
        }
    }
}
