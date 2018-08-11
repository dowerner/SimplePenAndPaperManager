using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Ink;

namespace SimplePenAndPaperManager.MapEditor.Entities
{
    [KnownType(typeof(Stairs))]
    [KnownType(typeof(Window))]
    [KnownType(typeof(Wall))]
    [KnownType(typeof(TextMarker))]
    [KnownType(typeof(Point2D))]
    [KnownType(typeof(Door))]
    [KnownType(typeof(Floor))]
    [KnownType(typeof(RectangularBuilding))]
    [KnownType(typeof(PolygonBuilding))]
    [DataContract]
    public class Map
    {
        public int GetNewId()
        {
            _newId++;
            return _newId;
        }
        private int _newId;

        [DataMember]
        public double Width { get; set; }

        [DataMember]
        public double Height { get; set; }

        [DataMember]
        public List<IMapEntity> Entities { get; set; }

        [DataMember]
        public StrokeCollection Terrain { get; set; }

        public Map()
        {
            _newId = -1;
        }
    }
}
