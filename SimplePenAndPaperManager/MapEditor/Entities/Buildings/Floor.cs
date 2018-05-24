using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class Floor : BaseMapEntity, IFloorEntity
    {
        public List<IWallEntity> Walls { get; set; }
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
