using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualRectangularBuilding : VisualRectangle
    {
        public VisualRectangularBuilding(IRectangularMapEntity mapEntity) : base(mapEntity)
        {}

        public override IVisualElement Copy()
        {
            return new VisualRectangularBuilding((IRectangularMapEntity)_rectangleSource.Copy());
        }
    }
}
