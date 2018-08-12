using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System;
using System.IO;
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
                    Map map = FileDirector.Instance.LoadFromXml<Map>(dlg.FileName);
                    string terrainPath = dlg.FileName + Constants.TerrainFileExtentions;

                    if (File.Exists(terrainPath)) map.Terrain = FileDirector.Instance.LoadTerrain(terrainPath);
                    else MessageBox.Show($"The map was loaded but the terrain file '{terrainPath}' could not be found.", "Terrain not found", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _vm.CurrentMap = map;                    

                    _vm.CurrentMapPath = dlg.FileName;
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
                    FileDirector.Instance.SaveToXml(dlg.FileName, _vm.CurrentMap);
                    FileDirector.Instance.SaveTerrain(dlg.FileName + Constants.TerrainFileExtentions, _vm.CurrentMap.Terrain);
                    _vm.CurrentMapPath = dlg.FileName;
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
                FileDirector.Instance.SaveToXml(_vm.CurrentMapPath, _vm.CurrentMap);
                FileDirector.Instance.SaveTerrain(_vm.CurrentMapPath + Constants.TerrainFileExtentions, _vm.CurrentMap.Terrain);
            }
            catch(Exception e)
            {
                MessageBox.Show($"An error occured during the save procedure of the map. ({e.Message}).", "Map Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
