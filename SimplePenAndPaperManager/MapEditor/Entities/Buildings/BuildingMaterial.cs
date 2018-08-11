using System.Runtime.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities.Buildings
{
    [DataContract]
    public enum BuildingMaterial
    {
        [EnumMember]
        Brick,
        [EnumMember]
        Wood,
        [EnumMember]
        Metal,
        [EnumMember]
        Stone
    }
}
