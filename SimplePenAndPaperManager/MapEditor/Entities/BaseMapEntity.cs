using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities
{
    public abstract class BaseMapEntity : IMapEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Orientation { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
