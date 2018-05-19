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

        public override IMapEntity Copy()
        {
            IMapEntity copy = new Floor();
            CopyFillInBaseProperties(copy);
            return copy;
        }
    }
}
