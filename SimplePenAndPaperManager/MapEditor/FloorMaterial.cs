using System.Runtime.Serialization;

namespace SimplePenAndPaperManager.MapEditor
{
    [DataContract]
    public enum FloorMaterial
    {
        [EnumMember]
        Grass,
        [EnumMember]
        Stone,
        [EnumMember]
        Asphalt,
        [EnumMember]
        Wood,
        [EnumMember]
        Water,
        [EnumMember]
        Sand,
        [EnumMember]
        None
    }
}
