using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.Model;

namespace SimplePenAndPaperManager.MapEditor.Entities.Markers
{
    public class TextMarker : BaseMapEntity, ITextMarkerEntity
    {
        public string Text { get; set; }

        public TextMarker()
        {
            Name = Constants.DefaultMarkerName;
        }

        public override IMapEntity Copy()
        {
            TextMarker copy = new TextMarker() { Text = Text };
            CopyFillInBaseProperties(copy);
            return copy;
        }
    }
}
