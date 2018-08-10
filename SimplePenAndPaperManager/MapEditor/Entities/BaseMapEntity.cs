using SimplePenAndPaperManager.MapEditor.Entities.Interface;
using System;

namespace SimplePenAndPaperManager.MapEditor.Entities
{
    [Serializable]
    public abstract class BaseMapEntity : IMapEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Orientation { get; set; }
        public double X { get; set; }
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
