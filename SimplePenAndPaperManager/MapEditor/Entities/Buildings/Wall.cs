using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System;
using System.Xml.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [Serializable]
    public class Wall : BaseMapEntity, IWallEntity
    {
        [XmlArrayItem("ListOfDoors")]
        public List<IDoorEntity> Doors { get; set; }
        public BuildingMaterial Material { get; set; }
        public bool IsOuterWall { get; set; }
        public double Thickness { get; set; }
        public double Length { get; set; }
        [XmlArrayItem("ListOfWindows")]
        public List<IWindowEntity> Windows { get; set; }

        public Wall()
        {
            Doors = new List<IDoorEntity>();
            Windows = new List<IWindowEntity>();
        }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            Wall copy = new Wall() { Thickness = Thickness, Material = Material };
            copy.Doors = new List<IDoorEntity>();
            copy.Windows = new List<IWindowEntity>();
            foreach (IDoorEntity door in Doors) copy.Doors.Add((IDoorEntity)door.Copy(copyLocation));
            foreach (IWindowEntity window in Windows) copy.Windows.Add((IWindowEntity)window.Copy(copyLocation));
            copy.Length = Length;
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
