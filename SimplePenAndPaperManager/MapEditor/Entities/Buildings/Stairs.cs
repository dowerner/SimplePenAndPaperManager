using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using System.Runtime.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [DataContract]
    public class Stairs : BaseMapEntity, IStairsEntity
    {
        [DataMember]
        public BuildingMaterial Material { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            Stairs copy = new Stairs() { Material = Material };
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
