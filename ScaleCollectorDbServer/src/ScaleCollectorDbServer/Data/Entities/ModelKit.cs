namespace ScaleCollectorDbServer.Data.Entities
{
    public class ModelKit : TenantEntity
    {
        public long ScaleId { get; set; }
        public virtual Scale Scale { get; set; } = null!;
    }
}
