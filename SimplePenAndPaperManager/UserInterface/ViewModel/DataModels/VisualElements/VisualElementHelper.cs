﻿using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Buildings.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using SimplePenAndPaperManager.MapEditor.Entities.Markers.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Buildings;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Interface;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements.Markers;

namespace SimplePenAndPaperManager.UserInterface.ViewModel.DataModels.VisualElements
{
    public static class VisualElementHelper
    {
        public static IVisualElement CreateFromMapEntity(IMapEntity mapEntity)
        {
            if (mapEntity is RectangularBuilding) return new VisualRectangularBuilding((IRectangularBuildingEntity)mapEntity);
            if (mapEntity is PolygonBuilding) return new VisualPolygonalBuilding((IBuildingEntity)mapEntity);
            if (mapEntity is TextMarker) return new VisualTextMarker((ITextMarkerEntity)mapEntity);
            if (mapEntity is IWallEntity) return new WallElement((IWallEntity)mapEntity);
            if (mapEntity is IDoorEntity) return new VisualDoor((IDoorEntity)mapEntity);
            if (mapEntity is IFloorEntity) return new VisualFloor((IFloorEntity)mapEntity);

            return null;
        }
    }
}