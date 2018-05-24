﻿using SimplePenAndPaperManager.UserInterface.ViewModel.Commands;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Collections.Specialized;

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

        public virtual double MapWidth { get; set; }
        public virtual double MapHeight { get; set; }

        public virtual ObservableCollection<IVisualElement> MapEntities { get; set; }

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

        public IVisualElement LastSelected { get; set; }
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

            // commands
            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);
            CopyCommand = new CopyCommand(this);
            PasteCommand = new PasteCommand(this);
            DeleteCommand = new DeleteCommand(this);
            DeselectAllCommand = new DeselectAllCommand(this);
            EditEntityCommand = new EditEntityCommand(this);
            CreateMarkerCommand = new CreateMarkerCommand(this);
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