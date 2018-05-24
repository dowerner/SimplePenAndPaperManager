using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Windows.Ink;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class AddTerrainStrokeAction : BaseAction
    {
        public StrokeCollection Added { get; set; }

        public override void Do()
        {
            GlobalManagement.Instance.TerrainStrokes.Add(Added);
        }

        public override void Undo()
        {
            GlobalManagement.Instance.TerrainStrokes.Remove(Added);
        }

        public AddTerrainStrokeAction(ObservableCollection<IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
        }
    }
}
