using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualDoor : BaseVisualElement
    {
        private IDoorEntity _door;

        public VisualDoor(IDoorEntity mapEntity) : base(mapEntity)
        {
            DisplayWhilePlacing = true;
            AttachToWall = true;
            _door = mapEntity;
            Color = Colors.Gray;
        }

        public override IVisualElement Copy()
        {
            return new VisualDoor((IDoorEntity)_door.Copy());
        }
    }
}
