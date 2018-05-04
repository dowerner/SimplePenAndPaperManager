using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels
{
    /// <summary>
    /// A simple example of a data-model.  
    /// The purpose of this data-model is to share display data between the main window and overview window.
    /// </summary>
    public class DataModel : INotifyPropertyChanged
    {
        #region Data Members

        /// <summary>
        /// The singleton instance.
        /// This is a singleton for convenience.
        /// </summary>
        private static DataModel instance = new DataModel();

        ///
        /// The current scale at which the content is being viewed.
        /// 
        private double contentScale = 1;

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetX = 0;

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetY = 0;

        ///
        /// The width of the content (in content coordinates).
        /// 
        private double contentWidth = 0;

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        private double contentHeight = 0;

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        private double contentViewportWidth = 0;

        ///
        /// The heigth of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        private double contentViewportHeight = 0;

        #endregion Data Members

        /// <summary>
        /// Retreive the singleton instance.
        /// </summary>
        public static DataModel Instance
        {
            get
            {
                return instance;
            }
        }

        public DataModel()
        {
            UndoStack = new ObservableStack<IEditorAction>();
            RedoStack = new ObservableStack<IEditorAction>();

            SelectedEntities = new ObservableCollection<IVisualElement>();
            SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
            Clipboard = new ObservableCollection<IVisualElement>();

            MapEntities = new ObservableCollection<IVisualElement>();
            MapEntities.CollectionChanged += MapEntities_CollectionChanged;
            MapEntities.Add(new VisualElements.RectangleElement(new MapEditor.Entities.Buildings.RectangularBuilding() { X = 100, Y = 400, Width = 100, Height = 100, Orientation = 20 }));
            MapEntities.Add(new VisualElements.RectangleElement(new MapEditor.Entities.Buildings.RectangularBuilding() { X = 150, Y = 600, Width = 130, Height = 100, Orientation = -34 }));

            // stress test
            //for(int i = 0; i < 3000; i++) MapEntities.Add(new VisualElements.RectangleElement(new MapEditor.Entities.Buildings.RectangularBuilding() { X = 150, Y = 600, Width = 130, Height = 100, Orientation = -34 }));

            List<MapEditor.Entities.Point2D> c = new List<MapEditor.Entities.Point2D>();
            c.Add(new MapEditor.Entities.Point2D() { X = 10, Y = 10 });
            c.Add(new MapEditor.Entities.Point2D() { X = 24, Y = 10 });
            c.Add(new MapEditor.Entities.Point2D() { X = 15, Y = 34 });
            c.Add(new MapEditor.Entities.Point2D() { X = 30, Y = 50 });
            c.Add(new MapEditor.Entities.Point2D() { X = 60, Y = 50 });

            MapEntities.Add(new VisualElements.PolygonElement(new MapEditor.Entities.Buildings.PolygonBuilding() { X = 500, Y = 700, Corners = c, Orientation = 37.4 }));
        }

        private void MapEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // listen for entity changes
            if(e.NewItems != null)
            {
                foreach (IVisualElement element in e.NewItems) element.PropertyChanged += EntityChanged;
            }
            if(e.OldItems != null)
            {
                foreach (IVisualElement element in e.OldItems) element.PropertyChanged -= EntityChanged;
            }
            
        }

        private void SelectedEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // handle items where selection was removed
            foreach(IVisualElement element in MapEntities)
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

        private void EntityChanged(object sender, PropertyChangedEventArgs e)
        {
            IVisualElement entity = (IVisualElement)sender;

            if(e.PropertyName == "IsSelected")
            {
                if (entity.IsSelected && !_selectedEntities.Contains(entity)) _selectedEntities.Add(entity);
                else if (!entity.IsSelected && _selectedEntities.Contains(entity)) _selectedEntities.Remove(entity); 
            }
        }

        public IEditorAction CurrentAction { get; set; }

        public ObservableStack<IEditorAction> UndoStack { get; set; }
        public ObservableStack<IEditorAction> RedoStack { get; set; }

        public ObservableCollection<IVisualElement> Clipboard { get; set; }
        public Point CopyLocation { get; set; }
        public Point MousePosition { get; set; }

        public ObservableCollection<IVisualElement> MapEntities
        {
            get { return _mapEntities; }
            set
            {
                _mapEntities = value;
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

        public bool EntitiesSelected { get { return SelectedEntities.Count > 0; } }

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

        ///
        /// The current scale at which the content is being viewed.
        /// 
        public double ContentScale
        {
            get
            {
                return contentScale;
            }
            set
            {
                contentScale = value;
                OnPropertyChanged("ContentScale");
            }
        }

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetX
        {
            get
            {
                return contentOffsetX;
            }
            set
            {
                contentOffsetX = value;

                OnPropertyChanged("ContentOffsetX");
                OnPropertyChanged("SelectionLocation");
            }
        }

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetY
        {
            get
            {
                return contentOffsetY;
            }
            set
            {
                contentOffsetY = value;

                OnPropertyChanged("ContentOffsetY");
                OnPropertyChanged("SelectionLocation");
            }
        }

        ///
        /// The width of the content (in content coordinates).
        /// 
        public double ContentWidth
        {
            get
            {
                return contentWidth;
            }
            set
            {
                contentWidth = value;

                OnPropertyChanged("ContentWidth");
                OnPropertyChanged("SelectionLocation");
            }
        }

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        public double ContentHeight
        {
            get
            {
                return contentHeight;
            }
            set
            {
                contentHeight = value;

                OnPropertyChanged("ContentHeight");
                OnPropertyChanged("SelectionLocation");
            }
        }

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportWidth
        {
            get
            {
                return contentViewportWidth;
            }
            set
            {
                contentViewportWidth = value;

                OnPropertyChanged("ContentViewportWidth");
                OnPropertyChanged("SelectionLocation");
            }
        }

        ///
        /// The heigth of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportHeight
        {
            get
            {
                return contentViewportHeight;
            }
            set
            {
                contentViewportHeight = value;

                OnPropertyChanged("ContentViewportHeight");
                OnPropertyChanged("SelectionLocation");
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// </summary>
        private void OnPropertyChanged(string name)
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
