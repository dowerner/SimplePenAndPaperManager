using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class Wall : BaseMapEntity, IWallEntity
    {
        public List<IDoorEntity> Doors { get; set; }
        public BuildingMaterial Material { get; set; }
        public double Thickness { get; set; }
        public List<IWindowEntity> Windows { get; set; }

        public override IMapEntity Copy()
        {
            Wall copy = new Wall() { Thickness = Thickness, Material = Material };
            copy.Doors = new List<IDoorEntity>();
            copy.Windows = new List<IWindowEntity>();
            foreach (IDoorEntity door in Doors) copy.Doors.Add((IDoorEntity)door.Copy());
            foreach (IWindowEntity window in Windows) copy.Windows.Add((IWindowEntity)window.Copy());
            CopyFillInBaseProperties(copy);
            return copy;
        }
    }
}
