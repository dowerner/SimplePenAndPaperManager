using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Windows.Media;
using System.Windows;
using SimplePenAndPaperManager.MathTools;

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

        public WallElement AttachedWall
        {
            get { return _attachedWall; }
            set
            {
                if (_attachedWall != null)
                {
                    _attachedWall.PropertyChanged -= AttachedWall_PropertyChanged;
                    if (SourceEntity is IDoorEntity) _attachedWall.Doors.Remove(this);
                }

                _attachedWall = value;
                if (_attachedWall == null) return;
                if (SourceEntity is IDoorEntity) _attachedWall.Doors.Add(this);
                _attachedWall.PropertyChanged += AttachedWall_PropertyChanged;
                _pointOnWall = new Point(X - _attachedWall.X, Y - _attachedWall.Y).Rotate(-Orientation);
            }
        }
        private WallElement _attachedWall;
        private Point _pointOnWall;

        private void AttachedWall_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "X" || e.PropertyName == "Y" || e.PropertyName == "Orientation")
            {
                Orientation = _attachedWall.Orientation;
                Point newPosition = new Point(_attachedWall.X, _attachedWall.Y).Add(_pointOnWall.Rotate(Orientation));
                X = newPosition.X;
                Y = newPosition.Y;
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
