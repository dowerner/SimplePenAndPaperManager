using System;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;

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

        public override IVisualElement Copy()
        {
            return new RectangleElement((IRectangularMapEntity)_rectangleSource.Copy());
        }

        private IRectangularMapEntity _rectangleSource;

        public RectangleElement(IRectangularMapEntity mapEntity) : base(mapEntity)
        {
            _rectangleSource = mapEntity;
        }
    }
}
