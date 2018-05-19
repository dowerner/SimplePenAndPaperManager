using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface
{
    public interface IFloorEntity : IMapEntity
    {
        List<IWallEntity> Walls { get; set; }
        List<IMapEntity> Entities { get; set; }
    }
}
