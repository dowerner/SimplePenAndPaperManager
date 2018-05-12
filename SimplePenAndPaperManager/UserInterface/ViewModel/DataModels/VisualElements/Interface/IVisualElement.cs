using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.ComponentModel;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface
{
    public interface IVisualElement : INotifyPropertyChanged
    {
        Color Color { get; set; }
        double CenterX { get; set; }
        double CenterY { get; set; }
        double BoundingWidth { get; set; }
        double BoundingHeight { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Orientation { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        bool IsSelected { get; set; }
        IMapEntity SourceEntity { get; set; }
        IVisualElement Copy();
    }
}
