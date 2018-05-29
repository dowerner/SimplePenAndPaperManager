using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface
{
    public interface IWallEntity : IMapEntity
    {
        bool IsOuterWall { get; set; }
        double Thickness { get; set; }
        double Length { get; set; }
        BuildingMaterial Material { get; set; }
        List<IWindowEntity> Windows { get; set; }
        List<IDoorEntity> Doors { get; set; }
    }
}
