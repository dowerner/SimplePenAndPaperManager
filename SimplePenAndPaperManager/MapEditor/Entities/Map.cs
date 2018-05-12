using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Collections.Generic;
using System.Windows.Ink;

namespace SimplePenAndPaperManager.MapEditor.Entities
{
    public class Map
    {
        public int GetNewId()
        {
            _newId++;
            return _newId;
        }
        private int _newId;

        public double Width { get; set; }
        public double Height { get; set; }
        public List<IMapEntity> Entities { get; set; }
        public StrokeCollection Terrain { get; set; }

        public Map()
        {
            _newId = -1;
        }
    }
}
