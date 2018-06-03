using SimplePenAndPaperManager.UserInterface.ViewModel.Commands;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements;
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
        ObservableCollection<VisualCornerManipulator> CornerManipulators { get; set; }
        Point SelectionLocation { get; set; }
        Point CopyLocation { get; set; }
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
        bool AllowRotation { get; set; }
        bool SelectionIsBuilding { get; }
        double GizmoX { get; set; }
        double GizmoY { get; set; }
        Point MousePosition { get; set; }
        IVisualElement LastSelected { get; set; }

        #region Build Properties
        bool IsCreatingTextMarker { get; set; }
        #endregion

        #region Commands
        UndoCommand UndoCommand { get; }
        RedoCommand RedoCommand { get; }
        CopyCommand CopyCommand { get; }
        PasteCommand PasteCommand { get; }
        DeleteCommand DeleteCommand { get; }
        DeselectAllCommand DeselectAllCommand { get; }
        EditEntityCommand EditEntityCommand { get; }
        CreateMarkerCommand CreateMarkerCommand { get; }
        ShowManipulationPoints ShowManipulationPoints { get; }
        #endregion
    }
}
