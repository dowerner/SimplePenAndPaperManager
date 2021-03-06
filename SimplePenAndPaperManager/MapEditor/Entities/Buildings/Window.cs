﻿using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [DataContract]
    public class Window : BaseMapEntity, IWindowEntity
    {
        [DataMember]
        public List<IKeyEntity> Keys { get; set; }
        [DataMember]
        public bool Locked { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            Window copy = new Window() { Locked = Locked, Keys = Keys };    // the key list is the original list rather than a copy since the same keys would still fit a copy of the window
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
