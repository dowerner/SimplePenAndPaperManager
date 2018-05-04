using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions.Interface
{
    public interface IEditorAction
    {
        List<IVisualElement> AffectedEntities { get; set; }
        void Do();
        void Undo();
    }
}
