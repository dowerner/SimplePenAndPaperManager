using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Collections.Generic;
using System.Windows.Ink;

namespace SimplePenAndPaperManager.MapEditor.Entities
{
    public class Map
    {
        public List<IMapEntity> Entities { get; set; }
        public StrokeCollection Terrain { get; set; }
    }
}
