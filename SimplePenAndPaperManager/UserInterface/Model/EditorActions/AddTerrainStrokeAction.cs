using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Windows.Ink;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class AddTerrainStrokeAction : BaseAction
    {
        public StrokeCollection Added { get; set; }

        public override void Do()
        {
            DataModel.Instance.TerrainStrokes.Add(Added);
        }

        public override void Undo()
        {
            DataModel.Instance.TerrainStrokes.Remove(Added);
        }

        public AddTerrainStrokeAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }
    }
}
