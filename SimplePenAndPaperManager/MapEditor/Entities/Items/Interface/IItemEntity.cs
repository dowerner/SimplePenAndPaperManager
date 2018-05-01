using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Items.Interface
{
    public interface IItemEntity : IMapEntity
    {
        double Value { get; set; }
        double Weight { get; set; }
        string SymbolPath { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}
