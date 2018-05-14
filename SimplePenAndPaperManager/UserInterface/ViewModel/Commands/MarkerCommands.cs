using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class MarkerCommands
    {
        public static CreateMarkerCommand CreateMarkerCommand
        {
            get
            {
                if (_createMarkerCommand == null) _createMarkerCommand = new CreateMarkerCommand();
                return _createMarkerCommand;
            }
        }
        private static CreateMarkerCommand _createMarkerCommand;
    }

    public class CreateMarkerCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DataModel.Instance.InTerrainEditingMode = false;
            DataModel.Instance.IsCreatingTextMarker = true;
        }
    }
}
