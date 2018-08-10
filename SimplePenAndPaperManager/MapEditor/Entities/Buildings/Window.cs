using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System;
using System.Xml.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [Serializable]
    public class Window : BaseMapEntity, IWindowEntity
    {
        [XmlArrayItem("ListOfKeys")]
        public List<IKeyEntity> Keys { get; set; }
        public bool Locked { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            Window copy = new Window() { Locked = Locked, Keys = Keys };    // the key list is the original list rather than a copy since the same keys would still fit a copy of the window
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
