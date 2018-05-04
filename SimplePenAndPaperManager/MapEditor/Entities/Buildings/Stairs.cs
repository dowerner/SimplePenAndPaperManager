using System;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class Stairs : BaseMapEntity, IStairsEntity
    {
        public BuildingMaterial Material { get; set; }

        public override IMapEntity Copy()
        {
            Stairs copy = new Stairs() { Material = Material };
            CopyFillInBaseProperties(copy);
            return copy;
        }
    }
}
