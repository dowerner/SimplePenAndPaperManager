using System;
using System.Collections.ObjectModel;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;

namespace SimplePenAndPaperManager.UserInterface.Model.EditorActions
{
    public class CreateRectangleAction : BaseAction
    {
        public override void Do()
        {
            throw new NotImplementedException();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public CreateRectangleAction(ObservableCollection<IVisualElement> selectedEntities) : base(selectedEntities)
        {
        }
    }
}
