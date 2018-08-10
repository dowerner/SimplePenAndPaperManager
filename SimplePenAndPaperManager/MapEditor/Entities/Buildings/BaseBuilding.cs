using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using System.Collections.Generic;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System;
using System.Xml.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [Serializable]
    public abstract class BaseBuilding : BaseMapEntity, IBuildingEntity
    {
        [XmlArrayItem("ListOfFloors")]
        public List<IFloorEntity> Floors { get; set; }
        [XmlArrayItem("ListOfCorners")]
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
