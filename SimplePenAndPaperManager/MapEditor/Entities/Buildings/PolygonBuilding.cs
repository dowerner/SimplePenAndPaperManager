using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class PolygonBuilding : BaseBuilding, IPolygonMapEntity
    {
        public List<Point2D> Corners { get; set; }

        public override IMapEntity Copy()
        {
            PolygonBuilding copy = new PolygonBuilding() { Corners = new List<Point2D>() };
            foreach (Point2D corner in Corners) copy.Corners.Add(corner);
            CopyFillInBaseProperties(copy);
            return copy;
        }
    }
}
