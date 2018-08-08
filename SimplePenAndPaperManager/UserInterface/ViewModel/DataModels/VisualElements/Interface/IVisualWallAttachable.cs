namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface
{
    public interface IVisualWallAttachable : IVisualElement
    {
        WallElement AttachedWall { get; set; }
    }
}
