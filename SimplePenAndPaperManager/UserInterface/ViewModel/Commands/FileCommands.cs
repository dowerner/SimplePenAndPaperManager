using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System;
using System.Windows;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class OpenMapCommand : SimpleCommand
    {
        private DataModel _vm;

        public OpenMapCommand(DataModel vm, bool executionEnabled = true) : base(null, executionEnabled)
        {
            _vm = vm;
            _action = OpenMap;
        }

        private void OpenMap()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = Constants.DefaultMapName; // Default file name
            dlg.DefaultExt = Constants.SaveFileExtension; // Default file extension
            dlg.Filter = Constants.SaveFileExtensionMapFilter; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Open map
                try
                {
                    _vm.CurrentMap = FileDirector.Instance.LoadFromXml<Map>(dlg.FileName);
                }
                catch(Exception e)
                {
                    MessageBox.Show($"An error occured during the loading procedure of the map. ({e.Message}).", "Map Open Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class SaveMapAsCommand : SimpleCommand
    {
        private DataModel _vm;

        public SaveMapAsCommand(DataModel vm, bool executionEnabled = true) : base(null, executionEnabled)
        {
            _vm = vm;
            _action = SaveMap;
        }

        private void SaveMap()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = Constants.DefaultMapName; // Default file name
            dlg.DefaultExt = Constants.SaveFileExtension; // Default file extension
            dlg.Filter = Constants.SaveFileExtensionMapFilter; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save map
                try
                {
                    FileDirector.Instance.SaveToXml<Map>(dlg.FileName, _vm.CurrentMap);
                }
                catch(Exception e)
                {
                    MessageBox.Show($"An error occured during the save procedure of the map. ({e.Message}).", "Map Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }                
            }
        }
    }

    public class SaveMapCommand : SimpleCommand
    {
        private DataModel _vm;

        public SaveMapCommand(DataModel vm, bool executionEnabled = true) : base(null, executionEnabled)
        {
            _vm = vm;
            _action = SaveMap;
        }

        private void SaveMap()
        {
            // Save map
            try
            {
                FileDirector.Instance.SaveToXml<Map>(_vm.CurrentMapPath, _vm.CurrentMap);
            }
            catch(Exception e)
            {
                MessageBox.Show($"An error occured during the save procedure of the map. ({e.Message}).", "Map Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
