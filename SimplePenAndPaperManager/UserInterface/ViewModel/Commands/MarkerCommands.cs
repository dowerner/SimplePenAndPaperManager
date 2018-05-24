using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class CreateMarkerCommand : BaseCommand
    {
        public CreateMarkerCommand(IDataModel context) : base(context)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            GlobalManagement.Instance.InTerrainEditingMode = false;
            _context.IsCreatingTextMarker = true;
        }
    }
}
