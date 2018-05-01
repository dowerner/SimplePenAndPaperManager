using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Interface
{
    public interface IPolygonMapEntity : IMapEntity
    {
        List<Point2D> Corners { get; set; }
    }
}
