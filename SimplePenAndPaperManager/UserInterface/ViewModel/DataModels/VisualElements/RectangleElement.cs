using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public class RectangleElement : BaseVisualElement
    {
        public double Width
        {
            get { return _rectangleSource.Width; }
            set
            {
                _rectangleSource.Width = value;
                OnPropertyChanged("Widht");
            }
        }

        public double Height
        {
            get { return _rectangleSource.Height; }
            set
            {
                _rectangleSource.Height = value;
                OnPropertyChanged("Height");
            }
        }

        private IRectangularMapEntity _rectangleSource;

        public RectangleElement(IRectangularMapEntity mapEntity) : base(mapEntity)
        {
            _rectangleSource = mapEntity;
        }
    }
}
