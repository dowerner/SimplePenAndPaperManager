using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.Model;
using System.Runtime.Serialization;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings;

namespace SimplePenAndPaperManager.MapEditor.Entities.Markers
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
    public class TextMarker : BaseMapEntity, ITextMarkerEntity
    {
        public string Text { get; set; }

        public TextMarker()
        {
            Name = Constants.DefaultMarkerName;
        }

        public override IMapEntity Copy(bool copyLocation = false)
        {
            TextMarker copy = new TextMarker() { Text = Text };
            CopyFillInBaseProperties(copy, copyLocation);
            return copy;
        }
    }
}
