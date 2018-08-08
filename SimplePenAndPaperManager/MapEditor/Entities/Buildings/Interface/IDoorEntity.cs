using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;
using System.Collections.Generic;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface
{
    public interface IDoorEntity : IMapEntity
    {
        double Width { get; set; }
        double Thickness { get; set; }
        bool Locked { get; set; }
        List<IKeyEntity> Keys { get; set; }
        BuildingMaterial Material { get; set; }
    }
}
