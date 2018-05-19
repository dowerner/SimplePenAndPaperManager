using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface
{
    public interface IDataModel : INotifyPropertyChanged
    {
        double MapWidth { get; set; }
        double MapHeight { get; set; }
        ObservableCollection<IVisualElement> MapEntities { get; set; }
        ObservableCollection<IVisualElement> SelectedEntities { get; set; }
        Point SelectionLocation { get; set; }
        double ContentScale { get; set; }
        double ContentOffsetX { get; set; }
        double ContentOffsetY { get; set; }
        double ContentViewportWidth { get; set; }
        double ContentViewportHeight { get; set; }
        double ContentWidth { get; set; }
        double ContentHeight { get; set; }
        double GizmoOrientation { get; set; }
        bool EntitiesSelected { get; }
        bool GizmoIsRotating { get; set; }
        bool GizmoDragX { get; set; }
        bool GizmoDragY { get; set; }
        bool InTerrainEditingMode { get; set; }
        bool ShowTerrainEllipse { get; }
        bool ShowTerrainRectangle { get; }
        double GizmoX { get; set; }
        double GizmoY { get; set; }
        Point MousePosition { get; set; }
    }
}
