using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.Generic;
using System.Windows;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Vegetation.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class PasteAction : BaseAction
    {
        public List<IVisualElement> PastedEntities { get; set; }
        public List<Point> EntityOffsets { get; set; }
        public Point PasteLocation { get; set; }

        public override void Do()
        {
            _context.SelectedEntities.Clear();
            foreach(IVisualElement entity in AffectedEntities)
            {
                // check for objects that are not allowed to be paseted inside of buildings
                if (_context is IVisualBuilding && 
                    (entity.SourceEntity is IBuildingEntity
                  || entity.SourceEntity is IVegetationEntity)) continue;

                double offsetX = entity.X - _context.CopyLocation.X;
                double offsetY = entity.Y - _context.CopyLocation.Y;

                IVisualElement copy = entity.Copy();
                copy.X = PasteLocation.X + offsetX;
                copy.Y = PasteLocation.Y + offsetY;

                _context.MapEntities.Add(copy);
                PastedEntities.Add(copy);

                _context.SelectedEntities.Add(copy);
                copy.IsSelected = true;
            }
        }

        public override void Undo()
        {
            foreach (IVisualElement copy in PastedEntities)
            {
                _context.SelectedEntities.Remove(copy);
                _context.MapEntities.Remove(copy);
            }

            PastedEntities.Clear();
        }

        public PasteAction(ObservableCollection<IVisualElement> selectedEntities, IDataModel context) : base(selectedEntities, context)
        {
            PastedEntities = new List<IVisualElement>();
        }
    }
}
