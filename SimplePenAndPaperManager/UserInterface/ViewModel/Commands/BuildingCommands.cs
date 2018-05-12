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
            return !DataModel.Instance.InTerrainEditingMode;
        }

        public void Execute(object parameter)
        {
            DataModel.Instance.IsCreatingRectangle = true;
        }

        public CreateRecangularBuildingCommand()
        {
            DataModel.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InTerrainEditingMode") CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class CreatePolygonalBuildingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !DataModel.Instance.InTerrainEditingMode;
        }

        public void Execute(object parameter)
        {
            DataModel.Instance.IsCreatingPolygon = true;
        }

        public CreatePolygonalBuildingCommand()
        {
            DataModel.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InTerrainEditingMode") CanExecuteChanged?.Invoke(this, null);
        }
    }
}
