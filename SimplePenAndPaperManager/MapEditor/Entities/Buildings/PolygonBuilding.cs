using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class PolygonBuilding : BaseBuilding, IPolygonMapEntity
    {
        public List<Point2D> Corners { get; set; }
    }
}
