using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Windows.Media;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public abstract class PolygonElement : BaseVisualElement
    {
        public PointCollection Corners
        {
            get { return _corners; }
            set
            {
                _corners = value;
                OnPropertyChanged("Corners");
            }
        }
        protected PointCollection _corners;

        public PolygonElement(IMapEntity mapEntity) : base(mapEntity)
        {
        }
    }
}
