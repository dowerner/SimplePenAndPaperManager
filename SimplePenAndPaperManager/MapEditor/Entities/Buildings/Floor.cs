﻿using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [Serializable]
    public class Floor : BaseMapEntity, IFloorEntity
    {
        [XmlArrayItem("ListOfWalls")]
        public List<IWallEntity> Walls { get; set; }
        [XmlArrayItem("ListOfMapEntities")]
        public List<IMapEntity> Entities { get; set; }

        public Floor()
        {
            Walls = new List<IWallEntity>();
            Entities = new List<IMapEntity>();
        }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            IFloorEntity copy = new Floor();
            CopyFillInBaseProperties(copy, copyLocation);

            foreach (IWallEntity wall in Walls) copy.Walls.Add((IWallEntity)wall.Copy(copyLocation = true));    // walls have to be at same place relative to copied location
            foreach (IWallEntity entity in Entities) copy.Entities.Add(entity.Copy(copyLocation = true));    // entities have to be at same place relative to copied location

            return copy;
        }
    }
}
