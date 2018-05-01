using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public abstract class BaseBuilding : BaseMapEntity, IBuildingEntity
    {
        public List<IFloorEntity> Floors { get; set; }
    }
}
