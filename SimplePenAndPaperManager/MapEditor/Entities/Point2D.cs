using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using System.Runtime.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities
{
    [DataContract]
    public struct Point2D
    {
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }
    }
}
