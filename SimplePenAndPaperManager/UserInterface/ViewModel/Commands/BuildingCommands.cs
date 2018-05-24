using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class BuildingCommands
    {
        public static CreateRecangularBuildingCommand CreateRecangularBuildingCommand
        {
            get
            {
                if (_createRecangularBuildingCommand == null) _createRecangularBuildingCommand = new CreateRecangularBuildingCommand();
                return _createRecangularBuildingCommand;
            }
        }
        private static CreateRecangularBuildingCommand _createRecangularBuildingCommand;

        public static CreatePolygonalBuildingCommand CreatePolygonalBuildingCommand
        {
            get
            {
                if (_createPolygonalBuildingCommand == null) _createPolygonalBuildingCommand = new CreatePolygonalBuildingCommand();
                return _createPolygonalBuildingCommand;
            }
        }
        private static CreatePolygonalBuildingCommand _createPolygonalBuildingCommand;
    }

    public class CreateRecangularBuildingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            GlobalManagement.Instance.IsCreatingRectangularBuilding = true;
            GlobalManagement.Instance.InTerrainEditingMode = false;
        }
    }

    public class CreatePolygonalBuildingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            GlobalManagement.Instance.IsCreatingPolygonalBuilding = true;
            GlobalManagement.Instance.InTerrainEditingMode = false;
        }
    }
}
