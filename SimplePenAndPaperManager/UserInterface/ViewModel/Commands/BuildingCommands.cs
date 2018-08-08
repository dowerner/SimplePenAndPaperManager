using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
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

    public class CreateDoorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IDataModel _model;

        public CreateDoorCommand(IDataModel dataModel)
        {
            _model = dataModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            GlobalManagement.Instance.InTerrainEditingMode = false;
            _model.IsCreatingWallAttachement = true;
            if (_model.CurrentWallAttachable != null) _model.MapEntities.Remove(_model.CurrentWallAttachable);
            _model.CurrentWallAttachable = new VisualDoor(new Door() { Width=Constants.DefaultDoorWidth, Thickness=Constants.DefaultOutsideWallThickness, Name=Constants.DefaultDoorName });
            _model.MapEntities.Add(_model.CurrentWallAttachable);
        }
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
