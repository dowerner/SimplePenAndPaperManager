using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using System.Collections.ObjectModel;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface
{
    public delegate void FloorWasSelectedHandler(IVisualBuilding building);

    public interface IVisualBuilding : IDataModel, IVisualElement
    {
        event FloorWasSelectedHandler FloorWasSelected;
        ObservableCollection<VisualFloor> Floors { get; set; }
        VisualFloor CurrentFloor { get; set; }
        void CreateFloorFromDimensions();
        void FireFloorSelectionEvent();
    }
}
