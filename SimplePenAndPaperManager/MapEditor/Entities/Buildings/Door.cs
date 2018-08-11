using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Runtime.Serialization;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [DataContract]
    public class Door : BaseMapEntity, IDoorEntity
    {
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Thickness { get; set; }
        [DataMember]
        public List<IKeyEntity> Keys { get; set; }
        [DataMember]
        public bool Locked { get; set; }
        [DataMember]
        public BuildingMaterial Material { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            Door copy = new Door() { Locked = Locked, Material = Material, Keys = Keys };   // note that the keys aren't copied but the same keys can open the door which makes sense for a door
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
