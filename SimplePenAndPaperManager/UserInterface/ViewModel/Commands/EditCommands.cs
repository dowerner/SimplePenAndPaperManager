using SimplePenAndPaperManager.MathTools;
using SimplePenAndPaperManager.UserInterface.Model;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions;
using SimplePenAndPaperManager.UserInterface.Model.EditorActions.Interface;
using SimplePenAndPaperManager.UserInterface.View.Controls;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;
using System;
using System.Windows.Input;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.Commands
{
    public class EditCommands
    {
        public static UndoCommand UndoCommand
        {
            get
            {
                if (_undoCommand == null) _undoCommand = new UndoCommand();
                return _undoCommand;
            }
        }
        private static UndoCommand _undoCommand;

        public static RedoCommand RedoCommand
        {
            get
            {
                if (_redoCommand == null) _redoCommand = new RedoCommand();
                return _redoCommand;
            }
        }
        private static RedoCommand _redoCommand;

        public static CopyCommand CopyCommand
        {
            get
            {
                if (_copyCommand == null) _copyCommand = new CopyCommand();
                return _copyCommand;
            }
        }
        private static CopyCommand _copyCommand;

        public static PasteCommand PasteCommand
        {
            get
            {
                if (_pasteCommand == null) _pasteCommand = new PasteCommand();
                return _pasteCommand;
            }
        }
        private static PasteCommand _pasteCommand;

        public static DeleteCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null) _deleteCommand = new DeleteCommand();
                return _deleteCommand;
            }
        }
        private static DeleteCommand _deleteCommand;

        public static DeselectAllCommand DeselectAllCommand
        {
            get
            {
                if (_deselectAllCommand == null) _deselectAllCommand = new DeselectAllCommand();
                return _deselectAllCommand;
            }
        }
        private static DeselectAllCommand _deselectAllCommand;

        public static EditEntityCommand EditEntityCommand
        {
            get
            {
                if (_editEntityCommand == null) _editEntityCommand = new EditEntityCommand();
                return _editEntityCommand;
            }
        }
        private static EditEntityCommand _editEntityCommand;
    }

    public class UndoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DataModel.Instance.UndoStack.Count > 0;
        }

        public void Execute(object parameter)
        {
            IEditorAction action = DataModel.Instance.UndoStack.Pop();
            action.Undo();
            DataModel.Instance.RedoStack.Push(action);
        }

        public UndoCommand()
        {
            DataModel.Instance.UndoStack.StackChanged += UndoStack_StackChanged;
        }

        private void UndoStack_StackChanged(object sender, StackChangedEventArgs<IEditorAction> e)
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class RedoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DataModel.Instance.RedoStack.Count > 0;
        }

        public void Execute(object parameter)
        {
            IEditorAction action = DataModel.Instance.RedoStack.Pop();
            action.Do();
            DataModel.Instance.UndoStack.Push(action);
        }

        public RedoCommand()
        {
            DataModel.Instance.RedoStack.StackChanged += RedoStack_StackChanged;
        }

        private void RedoStack_StackChanged(object sender, StackChangedEventArgs<IEditorAction> e)
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class CopyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DataModel.Instance.SelectedEntities.Count > 0;
        }

        public void Execute(object parameter)
        {
            DataModel.Instance.Clipboard.Clear();
            DataModel.Instance.CopyLocation = DataModel.Instance.SelectionLocation;
            foreach (IVisualElement entity in DataModel.Instance.SelectedEntities) DataModel.Instance.Clipboard.Add(entity);
        }

        public CopyCommand()
        {
            DataModel.Instance.SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
        }

        private void SelectedEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class PasteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DataModel.Instance.Clipboard.Count > 0;
        }

        public void Execute(object parameter)
        {
            PasteAction pasteAction = new PasteAction(DataModel.Instance.Clipboard);
            pasteAction.PasteLocation = DataModel.Instance.MousePosition.PxToMeter();
            pasteAction.Do();
            DataModel.Instance.UndoStack.Push(pasteAction);
        }

        public PasteCommand()
        {
            DataModel.Instance.Clipboard.CollectionChanged += Clipboard_CollectionChanged;
        }

        private void Clipboard_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DataModel.Instance.SelectedEntities.Count > 0;
        }

        public void Execute(object parameter)
        {
            DeleteAction deleteAction = new DeleteAction(DataModel.Instance.SelectedEntities);
            deleteAction.Do();
            DataModel.Instance.UndoStack.Push(deleteAction);
        }

        public DeleteCommand()
        {
            DataModel.Instance.SelectedEntities.CollectionChanged += SelectedEntities_CollectionChanged;
        }

        private void SelectedEntities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class DeselectAllCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DataModel.Instance.SelectedEntities.Clear();
        }
    }

    public class EditEntityCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is IDataModel) BuildingEditor.OpenBuildingMap((IDataModel)parameter);
            else if (parameter is VisualTextMarker)
            {
                EditEntityAction action = new EditEntityAction(null) { AffectedElement = (IVisualElement)parameter };
                action.Do();
                DataModel.Instance.UndoStack.Push(action);
            }
        }
    }
}
