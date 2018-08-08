using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings
{
    public class VisualDoor : BaseVisualElement, IVisualWallAttachable
    {
        private IDoorEntity _door;

        public double Width
        {
            get { return _door.Width; }
            set
            {
                _door.Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public double Thickness
        {
            get { return _door.Thickness; }
            set
            {
                _door.Thickness = value;
                OnPropertyChanged(nameof(Thickness));
            }
        }

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
