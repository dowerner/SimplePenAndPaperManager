using SimplePenAndPaperManager.MapEditor;
using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions.Interface;
using SimplePenAndPaperManager.UserInterface.View.States;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Ink;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels
{
    public class GlobalManagement : INotifyPropertyChanged
    {


        public bool InTerrainEditingMode
        {
            get { return _inTerrainEditingMode; }
            set
            {
                _inTerrainEditingMode = value;
                if (!_inTerrainEditingMode)
                {
                    _terrainBrush = TerrainBrush.None;
                    OnPropertyChanged("TerrainBrush");
                }
                OnPropertyChanged("InTerrainEditingMode");
                OnPropertyChanged("ShowTerrainEllipse");
                OnPropertyChanged("ShowTerrainRectangle");
            }
        }
        private bool _inTerrainEditingMode;

        public TerrainBrush TerrainBrush
        {
            get { return _terrainBrush; }
            set
            {
                _terrainBrush = value;
                InTerrainEditingMode = _terrainBrush != TerrainBrush.None;
                OnPropertyChanged("TerrainBrush");
                OnPropertyChanged("ShowTerrainEllipse");
                OnPropertyChanged("ShowTerrainRectangle");
            }
        }
        private TerrainBrush _terrainBrush = TerrainBrush.Circle;

        public VisualRectangularBuilding NewRectangleBuilding { get; set; }
        public Point BuildingStartLocation { get; set; }
        public bool ShowTerrainEllipse { get { return _terrainBrush == TerrainBrush.Circle && InTerrainEditingMode; } }
        public bool ShowTerrainRectangle { get { return _terrainBrush == TerrainBrush.Rectangle && InTerrainEditingMode; } }

        public Point CanvasPosition
        {
            get { return _canvasPosition; }
            set
            {
                _canvasPosition = value;
                OnPropertyChanged("CanvasPosition");
            }
        }
        private Point _canvasPosition;

        public bool IsCreatingRectangularBuilding
        {
            get { return _isCreatingRectangularBuilding; }
            set
            {
                _isCreatingRectangularBuilding = value;
                OnPropertyChanged("IsCreatingRectangularBuilding");
            }
        }
        private bool _isCreatingRectangularBuilding;

        public bool IsCreatingPolygonalBuilding
        {
            get { return _isCreatingPolygonalBuilding; }
            set
            {
                _isCreatingPolygonalBuilding = value;
                OnPropertyChanged("IsCreatingPolygonalBuilding");
            }
        }
        private bool _isCreatingPolygonalBuilding;

        public double TerrainBrushSize
        {
            get { return _brushSize; }
            set
            {
                _brushSize = value;
                OnPropertyChanged("TerrainBrushSize");
            }
        }
        private double _brushSize = 10;

        public FloorMaterial Terrain
        {
            get { return _terrain; }
            set
            {
                _terrain = value;
                OnPropertyChanged("Terrain");
            }
        }
        private FloorMaterial _terrain = FloorMaterial.Grass;

        public StrokeCollection TerrainStrokes
        {
            get { return _terrainStrokes; }
            set
            {
                _terrainStrokes = value;
                OnPropertyChanged("TerrainStrokes");
            }
        }
        private StrokeCollection _terrainStrokes;

        public ObservableCollection<IVisualElement> Clipboard { get; set; }
        public ObservableStack<IEditorAction> UndoStack { get; set; }
        public ObservableStack<IEditorAction> RedoStack { get; set; }
        public IEditorAction CurrentAction { get; set; }

        public static GlobalManagement Instance
        {
            get
            {
                if (_instance == null) _instance = new GlobalManagement();
                return _instance;
            }
        }
        private static GlobalManagement _instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void TerrainStrokes_StrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
        {
            if (e.Added != null && e.Added.Count > 0)
            {
                UndoStack.Push(new AddTerrainStrokeAction(null, null) { Added = e.Added });
            }
        }

        private GlobalManagement()
        {
            TerrainBrushSize = 10;
            Terrain = FloorMaterial.Grass;

            Clipboard = new ObservableCollection<IVisualElement>();
            UndoStack = new ObservableStack<IEditorAction>();
            RedoStack = new ObservableStack<IEditorAction>();
            TerrainStrokes = new StrokeCollection();
            TerrainStrokes.StrokesChanged += TerrainStrokes_StrokesChanged;
        }
    }
}
