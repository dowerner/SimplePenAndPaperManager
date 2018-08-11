using SimplePenAndPaperManager.MapEditor.Entities.Buildings;
using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using SimplePenAndPaperManager.MapEditor.Entities.Markers;
using System.Runtime.Serialization;

namespace SimplePenAndPaperManager.MapEditor.Entities
{
    [DataContract]
    public abstract class BaseMapEntity : IMapEntity
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Orientation { get; set; }
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

        public abstract IMapEntity Copy(bool copyLocation = false);

        protected virtual void CopyFillInBaseProperties(IMapEntity copy, bool copyLocation = false)
        {
            copy.Name = Name;
            copy.Orientation = Orientation; // copy orientation

            if (copyLocation)
            {
                copy.X = X;
                copy.Y = Y;
            }
        }
    }
}
