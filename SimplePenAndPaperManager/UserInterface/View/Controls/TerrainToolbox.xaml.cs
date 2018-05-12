using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System.Windows;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.View.Controls
{
    /// <summary>
    /// Interaction logic for TerrainToolbox.xaml
    /// </summary>
    public partial class TerrainToolbox : Window
    {
        public TerrainToolbox()
        {
            InitializeComponent();
            Closing += TerrainToolbox_Closing;
            KeyDown += TerrainToolbox_KeyDown;
        }

        private void TerrainToolbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape) DataModel.Instance.InTerrainEditingMode = false;
        }

        private void TerrainToolbox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataModel.Instance.InTerrainEditingMode = false;
        }
    }
}
