using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface
{
    interface IWallEntity : IMapEntity
    {
        double Thickness { get; set; }
        BuildingMaterial Material { get; set; }
        List<IWindowEntity> Windows { get; set; }
        List<IDoorEntity> Doors { get; set; }
    }
}
