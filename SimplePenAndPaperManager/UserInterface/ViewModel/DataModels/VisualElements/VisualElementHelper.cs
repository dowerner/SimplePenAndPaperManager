using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public static class VisualElementHelper
    {
        public static IVisualElement CreateFromMapEntity(IMapEntity mapEntity)
        {
            if (mapEntity is RectangularBuilding) return new VisualRectangularBuilding((IRectangularMapEntity)mapEntity);
            if (mapEntity is PolygonBuilding) return new VisualPolygonalBuilding((IPolygonMapEntity)mapEntity);
            if (mapEntity is TextMarker) return new VisualTextMarker((ITextMarkerEntity)mapEntity);

            return null;
        }
    }
}