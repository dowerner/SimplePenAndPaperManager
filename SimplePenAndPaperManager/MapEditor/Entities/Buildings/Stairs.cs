using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class Stairs : BaseMapEntity, IStairsEntity
    {
        public BuildingMaterial Material { get; set; }
    }
}
