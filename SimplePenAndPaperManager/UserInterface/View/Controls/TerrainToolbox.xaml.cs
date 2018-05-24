using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System.ComponentModel;
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
            if(e.Key == Key.Escape) GlobalManagement.Instance.InTerrainEditingMode = false;
        }

        private void TerrainToolbox_Closing(object sender, CancelEventArgs e)
        {
            GlobalManagement.Instance.InTerrainEditingMode = false;
        }
    }
}
