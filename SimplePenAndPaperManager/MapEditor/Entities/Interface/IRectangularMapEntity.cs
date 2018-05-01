namespace SimplePenAndPaperManager.MapEditor.Entities.Interface
{
    public interface IRectangularMapEntity : IMapEntity
    {
        double Width { get; set; }
        double Height { get; set; }
    }
}
