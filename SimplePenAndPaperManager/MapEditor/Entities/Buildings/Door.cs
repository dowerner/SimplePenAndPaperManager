using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System;
using System.Xml.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [Serializable]
    public class Door : BaseMapEntity, IDoorEntity
    {
        public double Width { get; set; }
        public double Thickness { get; set; }

        [XmlArrayItem("ListOfKeys")]
        public List<IKeyEntity> Keys { get; set; }
        public bool Locked { get; set; }
        public BuildingMaterial Material { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            Door copy = new Door() { Locked = Locked, Material = Material, Keys = Keys };   // note that the keys aren't copied but the same keys can open the door which makes sense for a door
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
