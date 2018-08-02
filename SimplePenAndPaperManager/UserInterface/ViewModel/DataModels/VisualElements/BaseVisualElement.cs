using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public abstract class BaseVisualElement : IVisualElement
    {
        public bool AttachToWall
        {
            get { return _attachToWall; }
            set
            {
                _attachToWall = value;
                OnPropertyChanged(nameof(AttachToWall));
            }
        }
        private bool _attachToWall;

        public bool DisplayWhilePlacing
        {
            get { return _displayWhilePlacing; }
            set
            {
                _displayWhilePlacing = value;
                OnPropertyChanged(nameof(DisplayWhilePlacing));
            }
        }
        private bool _displayWhilePlacing;

        public double BoundingWidth
        {
            get { return _boundingWidth; }
            set
            {
                _boundingWidth = value;
                OnPropertyChanged(nameof(BoundingWidth));
            }
        }
        protected double _boundingWidth;

        public double BoundingHeight
        {
            get { return _boundingHeight; }
            set
            {
                _boundingHeight = value;
                OnPropertyChanged(nameof(BoundingHeight));
            }
        }
        protected double _boundingHeight;

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        protected Color _color;

        public double CenterX
        {
            get { return _centerX; }
            set
            {
                _centerX = value;
                OnPropertyChanged(nameof(CenterX));
            }
        }
        protected double _centerX;

        public double CenterY
        {
            get { return _centerY; }
            set
            {
                _centerY = value;
                OnPropertyChanged(nameof(CenterY));
            }
        }
        protected double _centerY;

        public virtual double X
        {
            get { return _source.X; }
            set
            {
                _source.X = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public virtual double Y
        {
            get { return _source.Y; }
            set
            {
                _source.Y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public double Orientation
        {
            get { return _source.Orientation; }
            set
            {
                _source.Orientation = value;
                OnPropertyChanged(nameof(Orientation));
            }
        }

        public int Id
        {
            get { return _source.Id; }
            set
            {
                _source.Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get { return _source.Name; }
            set
            {
                _source.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                StrokeColor = _isSelected ? Colors.Blue : Color;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        private bool _isSelected;

        public Color StrokeColor
        {
            get { return _strokeColor; }
            set
            {
                _strokeColor = value;
                OnPropertyChanged(nameof(StrokeColor));
            }
        }
        private Color _strokeColor;

        public abstract IVisualElement Copy();

        public IMapEntity SourceEntity
        {
            get { return _source; }
            set { _source = value; }
        }
        protected IMapEntity _source;

        public BaseVisualElement(IMapEntity mapEntity)
        {
            AttachToWall = false;
            DisplayWhilePlacing = false;
            _source = mapEntity;
            _color = Colors.Black;
            CenterX = 0.5;
            CenterY = 0.5;
        }


        #region INotifyPropertyChanged Members
        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// </summary>
        protected void OnPropertyChanged(string name)
        {
            CommandManager.InvalidateRequerySuggested();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
