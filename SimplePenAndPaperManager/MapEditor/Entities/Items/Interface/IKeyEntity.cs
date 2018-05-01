using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Items.Interface
{
    public interface IKeyEntity : IItemEntity
    {
        string Description { get; set; }
    }
}
