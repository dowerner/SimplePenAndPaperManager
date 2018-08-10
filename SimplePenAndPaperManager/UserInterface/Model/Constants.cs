namespace SimplePenAndPaperManager.UserInterface.Model
{
    public static class Constants
    {
        public const double PxPerMeter = 40;
        public const double MinBrushSize = 0.1; // 10 cm
        public const double MaxBrushSize = 100; // 100 m
        public const double StartMax = -1e9;
        public const double StartMin = 1e9;
        public const double BuildingBoundExtension = 2;
        public const double ManipulatorDiameter = 0.3;

        public const double DefaultOutsideWallThickness = 0.25;
        public const double ThresholdDistanceForWallAttachables = 1;
        public const double DefaultDoorWidth = 1;

        public const string DefaultHouseName = "House";
        public const string DefaultDoorName = "Door";
        public const string DefaultWallName = "Wall";
        public const string DefaultOuterWallName = "OuterWall";
        public const string DefaultMarkerName = "Marker";
        public const string DefaultFloorName = "Floor";

        public const string SaveFileExtension = ".xml";
        public const string SaveFileExtensionMapFilter = "Pen&Paper Map Files (.xml)|*.xml";
        public const string DefaultMapName = "PenAndPaperMap.xml";
    }
}
