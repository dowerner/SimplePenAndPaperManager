using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualPolygonalBuilding : VisualPolygon
    {
        public VisualPolygonalBuilding(IPolygonMapEntity mapEntity) : base(mapEntity)
        {}

        public override IVisualElement Copy()
        {
            return new VisualPolygonalBuilding((IPolygonMapEntity)_polygonSource.Copy());
        }
    }
}
