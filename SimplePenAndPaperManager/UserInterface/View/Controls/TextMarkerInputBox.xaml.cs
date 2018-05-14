using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;
using System.Windows;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for TextMarkerInputBox.xaml
    /// </summary>
    public partial class TextMarkerInputBox : Window
    {
        private VisualTextMarker _marker;

        public TextMarkerInputBox(VisualTextMarker marker)
        {
            InitializeComponent();

            _marker = marker;
            DataContext = _marker;

            Loaded += TextMarkerInputBox_Loaded;
        }

        private void TextMarkerInputBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextInput.Focus();
            TextInput.SelectAll();
        }

        public void SaveAndClose(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        public static void SetMarkerData(VisualTextMarker marker)
        {
            TextMarkerInputBox inputBox = new TextMarkerInputBox(marker);
            inputBox.ShowDialog();
        }
    }
}
