using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions.Interface;
using SimplePenAndPaperManager.UserInterface.View.Controls;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;
using System.Collections.Specialized;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class UndoCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return GlobalManagement.Instance.UndoStack.Count > 0;
        }

        public override void Execute(object parameter)
        {
            IEditorAction action = GlobalManagement.Instance.UndoStack.Pop();
            action.Undo();
            GlobalManagement.Instance.RedoStack.Push(action);
        }

        public UndoCommand(IDataModel context) : base(context)
        {
            GlobalManagement.Instance.UndoStack.StackChanged += UndoStack_StackChanged;
        }

        private void UndoStack_StackChanged(object sender, StackChangedEventArgs<IEditorAction> e)
        {
            OnCanExecutedChanged(this, null);
        }
    }

    public class RedoCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return GlobalManagement.Instance.RedoStack.Count > 0;
        }

        public override void Execute(object parameter)
        {
            IEditorAction action = GlobalManagement.Instance.RedoStack.Pop();
            action.Do();
            GlobalManagement.Instance.UndoStack.Push(action);
        }

        public RedoCommand(IDataModel context) : base(context)
        {
            GlobalManagement.Instance.RedoStack.StackChanged += RedoStack_StackChanged;
        }

        private void RedoStack_StackChanged(object sender, StackChangedEventArgs<IEditorAction> e)
        {
            OnCanExecutedChanged(this, null);
        }
    }

    public class CopyCommand : BaseCommand
    {
        public CopyCommand(IDataModel context) : base(context)
        {
            context.SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
        }

        private void SelectedEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnCanExecutedChanged(this, null);
        }

        public override bool CanExecute(object parameter)
        {
            return _context.SelectedEntities.Count > 0;
        }

        public override void Execute(object parameter)
        {
            GlobalManagement.Instance.Clipboard.Clear();
            _context.CopyLocation = _context.SelectionLocation;
            foreach (IVisualElement entity in _context.SelectedEntities) GlobalManagement.Instance.Clipboard.Add(entity);
        }
    }

    public class PasteCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return GlobalManagement.Instance.Clipboard.Count > 0;
        }

        public override void Execute(object parameter)
        {
            PasteAction pasteAction = new PasteAction(GlobalManagement.Instance.Clipboard, _context);
            pasteAction.PasteLocation = _context.MousePosition.PxToMeter();
            pasteAction.Do();
            GlobalManagement.Instance.UndoStack.Push(pasteAction);
        }

        public PasteCommand(IDataModel context) : base(context)
        {
            GlobalManagement.Instance.Clipboard.CollectionChanged += Clipboard_CollectionChanged;
        }

        private void Clipboard_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCanExecutedChanged(this, null);
        }
    }

    public class DeleteCommand : BaseCommand
    {
        public DeleteCommand(IDataModel context) : base(context)
        {
            _context.SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
        }

        private void SelectedEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCanExecutedChanged(this, null);
        }

        public override bool CanExecute(object parameter)
        {
            return _context.SelectedEntities.Count > 0;
        }

        public override void Execute(object parameter)
        {
            DeleteAction deleteAction = new DeleteAction(_context.SelectedEntities, _context);
            deleteAction.Do();
            GlobalManagement.Instance.UndoStack.Push(deleteAction);
        }
    }

    public class DeselectAllCommand : BaseCommand
    {
        public DeselectAllCommand(IDataModel context) : base(context)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            _context.SelectedEntities.Clear();
        }
    }

    public class EditEntityCommand : BaseCommand
    {
        public EditEntityCommand(IDataModel context) : base(context)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            IVisualElement selectedElement = _context.LastSelected;

            if (selectedElement is IDataModel) BuildingEditor.OpenBuildingMap((IDataModel)selectedElement);
            else if (selectedElement is VisualTextMarker)
            {
                EditEntityAction action = new EditEntityAction(null, _context) { AffectedElement = selectedElement };
                action.Do();
                GlobalManagement.Instance.UndoStack.Push(action);
            }
        }
    }
}
