using SimplePenAndPaperManager.UserInterface.ViewModel.Commands;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Data;
using SimplePenAndPaperManager.MapEditor.Entities.Characters.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Vegetation.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels
{
    public abstract class BaseDataModel : IDataModel
    {
        public UndoCommand UndoCommand { get; private set; }
        public RedoCommand RedoCommand { get; private set; }
        public CopyCommand CopyCommand { get; private set; }
        public PasteCommand PasteCommand { get; private set; }
        public DeleteCommand DeleteCommand { get; private set; }
        public DeselectAllCommand DeselectAllCommand { get; private set; }
        public EditEntityCommand EditEntityCommand { get; private set; }
        public CreateMarkerCommand CreateMarkerCommand { get; private set; }
        public ShowManipulationPoints ShowManipulationPoints { get; private set; }
        public CreateDoorCommand CreateDoorCommand { get; private set; }

        public IVisualWallAttachable CurrentWallAttachable { get; set; }

        public bool IsCreatingWallAttachement
        {
            get { return _isCreatingWallAttachement; }
            set
            {
                _isCreatingWallAttachement = value;
                OnPropertyChanged(nameof(IsCreatingWallAttachement));
            }
        }
        private bool _isCreatingWallAttachement;

        public bool SelectionIsBuilding
        {
            get { return LastSelected is IVisualBuilding; }
        }

        public virtual double MapWidth
        {
            get { return _mapWidth; }
            set
            {
                _mapWidth = value;
                OnPropertyChanged("MapWidth");
            }
        }
        private double _mapWidth;

        public virtual double MapHeight
        {
            get { return _mapHeight; }
            set
            {
                _mapHeight = value;
                OnPropertyChanged("MapHeight");
            }
        }
        private double _mapHeight;

        public ICollectionView CharacterView { get; set; }
        public ICollectionView ItemView { get; set; }
        public ICollectionView BuildingsView { get; set; }
        public ICollectionView WallsView { get; set; }
        public ICollectionView DoorsView { get; set; }
        public ICollectionView WindowsView { get; set; }
        public ICollectionView VegetationView { get; set; }
        public ICollectionView MarkersView { get; set; }

        public virtual ObservableCollection<IVisualElement> MapEntities
        {
            get { return _mapEntities; }
            set
            {
                _mapEntities = value;

                CharacterView = new CollectionViewSource() { Source = _mapEntities }.View;
                CharacterView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is ICharacterEntity; };
                ItemView = new CollectionViewSource() { Source = _mapEntities }.View;
                ItemView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IItemEntity; };
                BuildingsView = new CollectionViewSource() { Source = _mapEntities }.View;
                BuildingsView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IBuildingEntity && !(item is VisualCornerManipulator); };
                WallsView = new CollectionViewSource() { Source = _mapEntities }.View;
                WallsView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IWallEntity; };
                DoorsView = new CollectionViewSource() { Source = _mapEntities }.View;
                DoorsView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IDoorEntity; };
                WindowsView = new CollectionViewSource() { Source = _mapEntities }.View;
                WindowsView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IWindowEntity; };
                VegetationView = new CollectionViewSource() { Source = _mapEntities }.View;
                VegetationView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IVegetationEntity; };
                MarkersView = new CollectionViewSource() { Source = _mapEntities }.View;
                MarkersView.Filter = delegate (object item) { return ((IVisualElement)item).SourceEntity is IMarkerEntity; };

                OnPropertyChanged("MapEntities");
            }
        }
        private ObservableCollection<IVisualElement> _mapEntities;

        public ObservableCollection<IVisualElement> SelectedEntities
        {
            get { return _selectedEntities; }
            set
            {
                _selectedEntities = value;
                OnPropertyChanged("SelectedEntities");
            }
        }
        private ObservableCollection<IVisualElement> _selectedEntities;

        public ObservableCollection<VisualCornerManipulator> CornerManipulators { get; set; }

        public Point SelectionLocation
        {
            get { return _selectionLocation; }
            set
            {
                _selectionLocation = value;
                OnPropertyChanged("SelectionLocation");
            }
        }
        private Point _selectionLocation;

        public Point CopyLocation { get; set; }

        public bool IsCreatingTextMarker
        {
            get { return _isCreatingTextMarker; }
            set
            {
                _isCreatingTextMarker = value;
                OnPropertyChanged("IsCreatingTextMarker");
            }
        }
        private bool _isCreatingTextMarker;

        ///
        /// The current scale at which the content is being viewed.
        /// 
        public double ContentScale
        {
            get { return _contentScale; }
            set
            {
                _contentScale = value;
                OnPropertyChanged("ContentScale");
            }
        }
        private double _contentScale;

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetX
        {
            get { return _contentOffsetX; }
            set
            {
                _contentOffsetX = value;
                OnPropertyChanged("ContentOffsetX");
                OnPropertyChanged("SelectionLocation");
            }
        }
        private double _contentOffsetX;

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetY
        {
            get { return _contentOffsetY; }
            set
            {
                _contentOffsetY = value;
                OnPropertyChanged("ContentOffsetY");
                OnPropertyChanged("SelectionLocation");
            }
        }
        private double _contentOffsetY;

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportWidth
        {
            get { return _contentViewportWidth; }
            set
            {
                _contentViewportWidth = value;
                OnPropertyChanged("ContentViewportWidth");
                OnPropertyChanged("SelectionLocation");
            }
        }
        private double _contentViewportWidth;

        ///
        /// The heigth of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportHeight
        {
            get { return _contentViewportHeight; }
            set
            {
                _contentViewportHeight = value;
                OnPropertyChanged("ContentViewportHeight");
                OnPropertyChanged("SelectionLocation");
            }
        }
        private double _contentViewportHeight;

        ///
        /// The width of the content (in content coordinates).
        /// 
        public double ContentWidth
        {
            get { return _contentWidth; }
            set
            {
                _contentWidth = value;
                OnPropertyChanged("ContentWidth");
                OnPropertyChanged("SelectionLocation");
            }
        }
        private double _contentWidth;

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        public double ContentHeight
        {
            get { return _contentHeight; }
            set
            {
                _contentHeight = value;
                OnPropertyChanged("ContentHeight");
                OnPropertyChanged("SelectionLocation");
            }
        }
        private double _contentHeight;

        public double GizmoOrientation
        {
            get { return _gizmoOrientation; }
            set
            {
                _gizmoOrientation = value;
                OnPropertyChanged("GizmoOrientation");
            }
        }
        private double _gizmoOrientation;

        public bool EntitiesSelected { get { return SelectedEntities.Count > 0; } }

        public bool GizmoIsRotating
        {
            get { return _gizmoIsRotating; }
            set
            {
                _gizmoIsRotating = value;
                OnPropertyChanged("GizmoIsRotating");
            }
        }
        private bool _gizmoIsRotating;

        public bool GizmoDragX
        {
            get { return _gizmoDragX; }
            set
            {
                _gizmoDragX = value;
                OnPropertyChanged("GizmoDragX");
            }
        }
        private bool _gizmoDragX;

        public bool GizmoDragY
        {
            get { return _gizmoDragY; }
            set
            {
                _gizmoDragY = value;
                OnPropertyChanged("GizmoDragY");
            }
        }
        private bool _gizmoDragY;

        public double GizmoX
        {
            get { return _gizmoX; }
            set
            {
                _gizmoX = value;
                OnPropertyChanged("GizmoX");
            }
        }
        private double _gizmoX;

        public double GizmoY
        {
            get { return _gizmoY; }
            set
            {
                _gizmoY = value;
                OnPropertyChanged("GizmoY");
            }
        }
        private double _gizmoY;

        public IVisualElement LastSelected
        {
            get { return _lastSelected; }
            set
            {
                _lastSelected = value;
                OnPropertyChanged("LastSelected");
                OnPropertyChanged("SelectionIsBuilding");
            }
        }
        private IVisualElement _lastSelected;

        public Point MousePosition
        {
            get { return _mousePosition; }
            set
            {
                _mousePosition = value;
                OnPropertyChanged("MousePosition");
            }
        }
        private Point _mousePosition;

        public BaseDataModel()
        {
            // setup
            ContentScale = 1;
            SelectedEntities = new ObservableCollection<IVisualElement>();
            SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
            CornerManipulators = new ObservableCollection<VisualCornerManipulator>();
            MapEntities = new ObservableCollection<IVisualElement>();

            // commands
            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);
            CopyCommand = new CopyCommand(this);
            PasteCommand = new PasteCommand(this);
            DeleteCommand = new DeleteCommand(this);
            DeselectAllCommand = new DeselectAllCommand(this);
            EditEntityCommand = new EditEntityCommand(this);
            CreateMarkerCommand = new CreateMarkerCommand(this);
            ShowManipulationPoints = new ShowManipulationPoints(this);
            CreateDoorCommand = new CreateDoorCommand(this);
        }

        private void SelectedEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // handle items where selection was removed
            foreach (IVisualElement element in MapEntities)
            {
                if (!SelectedEntities.Contains(element)) element.IsSelected = false;
            }

            // set selection position
            _selectionLocation = new Point(0, 0);

            foreach (IVisualElement element in SelectedEntities)
            {
                _selectionLocation.X += element.X;
                _selectionLocation.Y += element.Y;
            }
            _selectionLocation.X /= SelectedEntities.Count;
            _selectionLocation.Y /= SelectedEntities.Count;

            OnPropertyChanged("EntitiesSelected");
            OnPropertyChanged("SelectionLocation");

            // set correct gizmo orientation
            if (_selectedEntities.Count == 1 && !GizmoIsRotating) GizmoOrientation = _selectedEntities[0].Orientation;
            else if (!GizmoIsRotating) GizmoOrientation = 0;

            // update rotation capabilities
            AllowRotation = true;
            foreach (IVisualElement entity in SelectedEntities) if (entity is VisualCornerManipulator) AllowRotation = false;
        }

        public bool AllowRotation
        {
            get { return _allowRotation; }
            set
            {
                _allowRotation = value;
                OnPropertyChanged("AllowRotation");
            }
        }
        private bool _allowRotation;

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
