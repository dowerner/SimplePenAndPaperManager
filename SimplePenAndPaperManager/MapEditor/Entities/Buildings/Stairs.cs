﻿using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [Serializable]
    public class Stairs : BaseMapEntity, IStairsEntity
    {
        public BuildingMaterial Material { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            Stairs copy = new Stairs() { Material = Material };
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
