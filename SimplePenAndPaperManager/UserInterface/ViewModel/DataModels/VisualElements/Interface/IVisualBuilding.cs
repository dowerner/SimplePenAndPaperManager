using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using System.Collections.ObjectModel;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface
{
    public interface IVisualBuilding : IDataModel, IVisualElement
    {
        ObservableCollection<VisualFloor> Floors { get; set; }
        VisualFloor CurrentFloor { get; set; }

    }
}
