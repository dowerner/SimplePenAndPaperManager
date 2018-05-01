using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.ComponentModel;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public abstract class BaseVisualElement : INotifyPropertyChanged
    {
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }
        private Color _color;

        public double CenterX
        {
            get { return _centerX; }
            set
            {
                _centerX = value;
                OnPropertyChanged("CenterX");
            }
        }
        private double _centerX;

        public double CenterY
        {
            get { return _centerY; }
            set
            {
                _centerY = value;
                OnPropertyChanged("CenterY");
            }
        }
        private double _centerY;

        public double X
        {
            get { return _source.X; }
            set
            {
                _source.X = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return _source.Y; }
            set
            {
                _source.Y = value;
                OnPropertyChanged("Y");
            }
        }

        public double Orientation
        {
            get { return _source.Orientation; }
            set
            {
                _source.Orientation = value;
                OnPropertyChanged("Orientation");
            }
        }

        public int Id
        {
            get { return _source.Id; }
            set
            {
                _source.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return _source.Name; }
            set
            {
                _source.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        private bool _isSelected;

        protected IMapEntity _source;

        public BaseVisualElement(IMapEntity mapEntity)
        {
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
