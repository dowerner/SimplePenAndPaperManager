using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreateRectangleAction : BaseAction
    {
        public VisualRectangularBuilding Building { get; set; }

        public override void Do()
        {
            _context.MapEntities.Add(Building);
            _context.SelectedEntities.Clear();
            Building.IsSelected = true;
        }

        public override void Undo()
        {
            _context.MapEntities.Remove(Building);
            if (_context.SelectedEntities.Contains(Building))
            {
                _context.SelectedEntities.Remove(Building);
            }
        }

        public CreateRectangleAction(ObservableCollection<IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
        }
    }
}
