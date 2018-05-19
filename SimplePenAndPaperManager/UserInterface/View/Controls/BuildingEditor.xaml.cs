using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
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

            if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                // use local selected!
                //DataModel.Instance.SelectedEntities.Clear();
            }
            selectedElement.IsSelected = !selectedElement.IsSelected;
        }
    }
}
