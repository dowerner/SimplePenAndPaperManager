﻿using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface
{
    public interface IBuildingEntity : IMapEntity, IPolygonMapEntity
    {
        List<IFloorEntity> Floors { get; set; }
    }
}
