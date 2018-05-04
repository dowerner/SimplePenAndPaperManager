using System;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class RectangularBuilding : BaseBuilding, IRectangularMapEntity
    {
        public double Height { get; set; }
        public double Width { get; set; }

        public override IMapEntity Copy()
        {
            RectangularBuilding copy = new RectangularBuilding() { Width = Width, Height = Height };
            CopyFillInBaseProperties(copy);
            return copy;
        }
    }
}
