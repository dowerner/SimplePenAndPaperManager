using SimplePenAndPaperManager.MapEditor.Entities.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Characters.Interface
{
    public interface ICharacterEntity : IMapEntity
    {
        bool IsPlayer { get; set; }
    }
}
