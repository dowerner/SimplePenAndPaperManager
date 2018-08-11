using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using System.Runtime.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [KnownType(typeof(Stairs))]
    [KnownType(typeof(Window))]
    [KnownType(typeof(TextMarker))]
    [KnownType(typeof(Point2D))]
    [KnownType(typeof(Door))]
    [KnownType(typeof(Floor))]
    [KnownType(typeof(RectangularBuilding))]
    [KnownType(typeof(PolygonBuilding))]
    [DataContract]
    public class RectangularBuilding : BaseBuilding, IRectangularBuildingEntity
    {
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public double Width { get; set; }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            RectangularBuilding copy = new RectangularBuilding() { Width = Width, Height = Height };
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
