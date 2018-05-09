using SimplePenAndPaperManager.UserInterface.Model.EditorActions.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public abstract class BaseAction : IEditorAction
    {
        public List<IVisualElement> AffectedEntities { get; set; }

        public abstract void Do();

        public abstract void Undo();

        public BaseAction(ObservableCollection<IVisualElement> selectedEntities)
        {
            AffectedEntities = new List<IVisualElement>();
            if(selectedEntities != null)
            {
                foreach (IVisualElement enity in selectedEntities) AffectedEntities.Add(enity);
            }
        }
    }
}
