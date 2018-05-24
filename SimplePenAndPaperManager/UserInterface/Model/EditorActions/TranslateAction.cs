using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class TranslateAction : BaseAction
    {
        public List<Point> EntityOffsets { get; set; }

        public Point TransformStartPoint
        {
            get { return _transformStartPoint; }
            set
            {
                _transformStartPoint = value;

                EntityOffsets.Clear();
                foreach(IVisualElement entity in AffectedEntities)
                {
                    EntityOffsets.Add(new Point(entity.X - _transformStartPoint.X, entity.Y - _transformStartPoint.Y));
                }
            }
        }
        private Point _transformStartPoint;

        public Point TransformEndPoint
        {
            get { return _transformEndPoint; }
            set
            {
                _transformEndPoint = value;
            }
        }
        private Point _transformEndPoint;

        public override void Do()
        {
            _context.SelectedEntities.Clear();
            for (int i = 0; i < AffectedEntities.Count; i++)
            {
                AffectedEntities[i].X = _transformEndPoint.X + EntityOffsets[i].X;
                AffectedEntities[i].Y = _transformEndPoint.Y + EntityOffsets[i].Y;
                _context.SelectedEntities.Add(AffectedEntities[i]);
                AffectedEntities[i].IsSelected = true;
            }
            _context.SelectionLocation = _transformEndPoint;
        }

        public override void Undo()
        {
            _context.SelectedEntities.Clear();
            for (int i = 0; i < AffectedEntities.Count; i++)
            {
                AffectedEntities[i].X = _transformStartPoint.X + EntityOffsets[i].X;
                AffectedEntities[i].Y = _transformStartPoint.Y + EntityOffsets[i].Y;
                _context.SelectedEntities.Add(AffectedEntities[i]);
                AffectedEntities[i].IsSelected = true;
            }
            _context.SelectionLocation = _transformStartPoint;
        }

        public TranslateAction(ObservableCollection<IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
            EntityOffsets = new List<Point>();
        }
    }
}
