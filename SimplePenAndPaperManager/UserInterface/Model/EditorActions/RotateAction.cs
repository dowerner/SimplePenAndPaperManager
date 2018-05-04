using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class RotateAction : BaseAction
    {
        public List<Point> EntityOffsets { get; set; }
        public List<double> EntityOrientations { get; set; }
        public double StartRotation { get; set; }
        public double EndRotation { get; set; }

        public Point PivotPoint
        {
            get { return _pivotPoint; }
            set
            {
                _pivotPoint = value;

                EntityOffsets.Clear();
                EntityOrientations.Clear();
                foreach (IVisualElement entity in AffectedEntities)
                {
                    EntityOffsets.Add(new Point(entity.X - _pivotPoint.X, entity.Y - _pivotPoint.Y));
                    EntityOrientations.Add(entity.Orientation);
                }
            }
        }
        private Point _pivotPoint;

        public override void Do()
        {
            DataModel.Instance.SelectedEntities.Clear();
            for (int i = 0; i < AffectedEntities.Count; i++)
            {
                double angle = EndRotation * Math.PI / 180;
                AffectedEntities[i].X = _pivotPoint.X + EntityOffsets[i].X * Math.Cos(angle) - EntityOffsets[i].Y * Math.Sin(angle);
                AffectedEntities[i].Y = _pivotPoint.Y + EntityOffsets[i].X * Math.Sin(angle) + EntityOffsets[i].Y * Math.Cos(angle);

                if(AffectedEntities.Count > 1) AffectedEntities[i].Orientation = (EntityOrientations[i] + EndRotation) % 360;   // if there are more than one entities being rotated use differential angle
                else AffectedEntities[i].Orientation = EndRotation;    // if there is only one entity being rotated use absolute angle

                DataModel.Instance.SelectedEntities.Add(AffectedEntities[i]);
                AffectedEntities[i].IsSelected = true;
            }
        }

        public override void Undo()
        {
            DataModel.Instance.SelectedEntities.Clear();
            for (int i = 0; i < AffectedEntities.Count; i++)
            {
                double angle = StartRotation * Math.PI / 180;
                AffectedEntities[i].X = _pivotPoint.X + EntityOffsets[i].X * Math.Cos(angle) - EntityOffsets[i].Y * Math.Sin(angle);
                AffectedEntities[i].Y = _pivotPoint.Y + EntityOffsets[i].X * Math.Sin(angle) + EntityOffsets[i].Y * Math.Cos(angle);
                AffectedEntities[i].Orientation = EntityOrientations[i];
                DataModel.Instance.SelectedEntities.Add(AffectedEntities[i]);
                AffectedEntities[i].IsSelected = true;
            }
        }

        public RotateAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
            EntityOffsets = new List<Point>();
            EntityOrientations = new List<double>();
        }
    }
}
