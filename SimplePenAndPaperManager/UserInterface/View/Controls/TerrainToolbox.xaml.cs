using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System.Windows;

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
        }

        private void TerrainToolbox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataModel.Instance.InTerrainEditingMode = false;
        }
    }
}
