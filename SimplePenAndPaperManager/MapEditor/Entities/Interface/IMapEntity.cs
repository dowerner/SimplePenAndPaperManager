namespace SimplePenAndPaperManager.MapEditor.Entities.Interface
{
    public interface IMapEntity
    {
        int Id { get; set; }
        string Name { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Orientation { get; set; }
        IMapEntity Copy(bool copyLocation = false);
    }
}
