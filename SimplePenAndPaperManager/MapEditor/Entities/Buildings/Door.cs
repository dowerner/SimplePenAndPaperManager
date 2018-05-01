using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class Door : BaseMapEntity, IDoorEntity
    {
        public List<IKeyEntity> Keys { get; set; }
        public bool Locked { get; set; }
        public BuildingMaterial Material { get; set; }
    }
}
