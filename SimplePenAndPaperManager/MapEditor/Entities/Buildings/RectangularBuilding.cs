using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class RectangularBuilding : BaseBuilding, IRectangularBuildingEntity
    {
        public double Height { get; set; }
        public double Width { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            RectangularBuilding copy = new RectangularBuilding() { Width = Width, Height = Height };
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
