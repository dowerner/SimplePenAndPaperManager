using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Runtime.Serialization;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;

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
    public class PolygonBuilding : BaseBuilding, IPolygonMapEntity
    {
        public override IMapEntity Copy(bool copyLocation = false)
        {
            PolygonBuilding copy = new PolygonBuilding() { Corners = new List<Point2D>() };
            foreach (Point2D corner in Corners) copy.Corners.Add(corner);
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
