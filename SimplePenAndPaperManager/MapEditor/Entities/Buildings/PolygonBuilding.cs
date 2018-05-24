using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
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
