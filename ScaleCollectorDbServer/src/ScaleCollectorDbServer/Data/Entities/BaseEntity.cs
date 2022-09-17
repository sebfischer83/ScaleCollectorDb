namespace ScaleCollectorDbServer.Data.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
