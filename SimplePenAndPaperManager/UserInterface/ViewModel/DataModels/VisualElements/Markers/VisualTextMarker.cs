using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers
{
    public class VisualTextMarker : BaseVisualElement
    {
        public string Text
        {
            get { return _markerSource.Text; }
            set
            {
                _markerSource.Text = value;
                OnPropertyChanged("Text");
            }
        }

        protected ITextMarkerEntity _markerSource;

        public VisualTextMarker(ITextMarkerEntity mapEntity) : base(mapEntity)
        {
            _markerSource = mapEntity;
        }

        public override BaseVisualElement Copy()
        {
            return new VisualTextMarker((ITextMarkerEntity)_markerSource.Copy());
        }
    }
}
