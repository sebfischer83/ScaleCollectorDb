namespace ScaleCollectorDbServer.Data.Entities
{
    public class ModelKitReference
    {
        public long Id { get; set; }

        public long ModelKitId { get; set; }
        public virtual ModelKit ModelKit { get; set; }

        public long ReferenceId { get; set; }
        public virtual ModelKit Reference { get; set; }
    }
}
