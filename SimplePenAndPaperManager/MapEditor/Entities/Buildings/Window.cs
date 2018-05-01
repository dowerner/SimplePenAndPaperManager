using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Items.Interface;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    public class Window : BaseMapEntity, IWindowEntity
    {
        public List<IKeyEntity> Keys { get; set; }
        public bool Locked { get; set; }
    }
}
