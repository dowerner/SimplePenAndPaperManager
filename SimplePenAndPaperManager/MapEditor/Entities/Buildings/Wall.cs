using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [DataContract]
    public class Wall : BaseMapEntity, IWallEntity
    {
        [DataMember]
        public List<IDoorEntity> Doors { get; set; }
        [DataMember]
        public BuildingMaterial Material { get; set; }
        [DataMember]
        public bool IsOuterWall { get; set; }
        [DataMember]
        public double Thickness { get; set; }
        [DataMember]
        public double Length { get; set; }
        [DataMember]
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
