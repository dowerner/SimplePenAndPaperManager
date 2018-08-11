using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System.Runtime.Serialization;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [DataContract]
    public abstract class BaseBuilding : BaseMapEntity, IBuildingEntity
    {
        [DataMember]
        public List<IFloorEntity> Floors { get; set; }
        [DataMember]
        public List<Point2D> Corners { get; set; }

        public BaseBuilding()
        {
            Floors = new List<IFloorEntity>();
            Corners = new List<Point2D>();
        }

        protected override void CopyFillInBaseProperties(IMapEntity copy, bool copyLocation = false)
        {
            base.CopyFillInBaseProperties(copy, copyLocation);

            IBuildingEntity copiedBuilding = (IBuildingEntity)copy;
            foreach (IFloorEntity floor in Floors) copiedBuilding.Floors.Add((IFloorEntity)floor.Copy(copyLocation));
        }
    }
}
