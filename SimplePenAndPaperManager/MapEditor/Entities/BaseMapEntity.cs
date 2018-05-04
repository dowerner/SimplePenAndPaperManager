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

        public abstract IMapEntity Copy();

        protected virtual void CopyFillInBaseProperties(IMapEntity copy)
        {
            copy.Name = Name;
            copy.Orientation = Orientation; // copy orientation but not position as this is determined by the cursor position on pasting
        }
    }
}
