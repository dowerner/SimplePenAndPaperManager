using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Windows;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDataModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new DataModel();
            DataContext = _vm;
        }

        private void ListBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IVisualElement selectedElement = (IVisualElement)((FrameworkElement)sender).DataContext;

            if(!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                _vm.SelectedEntities.Clear();
            }
            selectedElement.IsSelected = !selectedElement.IsSelected;
        }
    }
}
