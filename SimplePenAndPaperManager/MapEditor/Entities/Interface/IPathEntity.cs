namespace SimplePenAndPaperManager.MapEditor.Entities.Interface
{
    public interface IPathEntity : IMapEntity
    {
        double X1 { get; set; }
        double Y1 { get; set; }
        double X2 { get; set; }
        double Y2 { get; set; }
    }
}
