using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface
{
    public interface IStairsEntity : IMapEntity
    {
        BuildingMaterial Material { get; set; }
    }
}
