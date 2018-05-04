﻿using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public abstract class BaseBuilding : BaseMapEntity, IBuildingEntity
    {
        public List<IFloorEntity> Floors { get; set; }

        public BaseBuilding()
        {
            Floors = new List<IFloorEntity>();
        }

        protected override void CopyFillInBaseProperties(IMapEntity copy)
        {
            base.CopyFillInBaseProperties(copy);

            IBuildingEntity copiedBuilding = (IBuildingEntity)copy;
            foreach (IFloorEntity floor in Floors) copiedBuilding.Floors.Add((IFloorEntity)floor.Copy());
        }
    }
}