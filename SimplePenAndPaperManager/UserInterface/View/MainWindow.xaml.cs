using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
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
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IVisualElement selectedElement = (IVisualElement)((FrameworkElement)sender).DataContext;


            if(!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                DataModel.Instance.SelectedEntities.Clear();
            }
            selectedElement.IsSelected = !selectedElement.IsSelected;
        }
    }
}
