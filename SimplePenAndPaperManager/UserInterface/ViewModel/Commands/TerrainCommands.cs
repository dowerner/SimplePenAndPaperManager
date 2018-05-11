﻿using SimplePenAndPaperManager.MapEditor;
using SimplePenAndPaperManager.UserInterface.View.Controls;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class TerrainCommands
    {
        public static EditTerrainCommand EditTerrainCommand
        {
            get
            {
                if (_editTerrainCommand == null) _editTerrainCommand = new EditTerrainCommand();
                return _editTerrainCommand;
            }
        }
        private static EditTerrainCommand _editTerrainCommand;

        public static SetTerrainMaterialCommand SetTerrainMaterialCommand
        {
            get
            {
                if (_setTerrainMaterialCommand == null) _setTerrainMaterialCommand = new SetTerrainMaterialCommand();
                return _setTerrainMaterialCommand;
            }
        }
        private static SetTerrainMaterialCommand _setTerrainMaterialCommand;
    }

    public class EditTerrainCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !DataModel.Instance.InTerrainEditingMode;
        }

        public void Execute(object parameter)
        {
            DataModel.Instance.InTerrainEditingMode = true;
            TerrainToolbox toolbox = new TerrainToolbox();
            toolbox.Owner = Application.Current.MainWindow;
            toolbox.Show();
        }

        public EditTerrainCommand()
        {
            DataModel.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InTerrainEditingMode") CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class SetTerrainMaterialCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DataModel.Instance.Terrain = (FloorMaterial)parameter;
        }
    }
}
