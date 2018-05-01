using System;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class Wall : BaseMapEntity, IWallEntity
    {
        public List<IDoorEntity> Doors { get; set; }
        public BuildingMaterial Material { get; set; }
        public double Thickness { get; set; }
        public List<IWindowEntity> Windows { get; set; }
    }
}
