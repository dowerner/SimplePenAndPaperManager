using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Windows;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for BuildingEditor.xaml
    /// </summary>
    public partial class BuildingEditor : Window
    {
        public BuildingEditor()
        {
            InitializeComponent();
            Owner = GlobalManagement.Instance.MainWindow;
        }

        public void SaveAndClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        public static void OpenBuildingMap(IDataModel visualElement)
        {
            BuildingEditor editor = new BuildingEditor();
            editor.DataContext = visualElement;
            editor.Show();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IVisualElement selectedElement = (IVisualElement)((FrameworkElement)sender).DataContext;
            IVisualBuilding building = (IVisualBuilding)DataContext;

            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                building.SelectedEntities.Clear();
                foreach (VisualFloor floor in building.Floors) floor.IsSelected = false;
            }
            selectedElement.IsSelected = !selectedElement.IsSelected;

            if (selectedElement is VisualFloor)
            {
                building.CurrentFloor = (VisualFloor)selectedElement;
                building.FireFloorSelectionEvent();
            }
        }

        private void TreeView_LostFocus(object sender, RoutedEventArgs e)
        {
            // remove selection from floors to prevent accidental removal
            foreach (VisualFloor floor in ((IVisualBuilding)DataContext).Floors) floor.IsSelected = false;
        }
    }
}
