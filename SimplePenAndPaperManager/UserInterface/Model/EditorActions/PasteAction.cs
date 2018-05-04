using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.Generic;
using System.Windows;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class PasteAction : BaseAction
    {
        public List<IVisualElement> PastedEntities { get; set; }
        public List<Point> EntityOffsets { get; set; }
        public Point PasteLocation { get; set; }

        public override void Do()
        {
            DataModel.Instance.SelectedEntities.Clear();
            foreach(IVisualElement entity in AffectedEntities)
            {
                double offsetX = entity.X - DataModel.Instance.CopyLocation.X;
                double offsetY = entity.Y - DataModel.Instance.CopyLocation.Y;

                IVisualElement copy = entity.Copy();
                copy.X = PasteLocation.X + offsetX;
                copy.Y = PasteLocation.Y + offsetY;

                DataModel.Instance.MapEntities.Add(copy);
                PastedEntities.Add(copy);

                DataModel.Instance.SelectedEntities.Add(copy);
                copy.IsSelected = true;
            }
        }

        public override void Undo()
        {
            foreach (IVisualElement copy in PastedEntities)
            {
                DataModel.Instance.SelectedEntities.Remove(copy);
                DataModel.Instance.MapEntities.Remove(copy);
            }

            PastedEntities.Clear();
        }

        public PasteAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
            PastedEntities = new List<IVisualElement>();
        }
    }
}
