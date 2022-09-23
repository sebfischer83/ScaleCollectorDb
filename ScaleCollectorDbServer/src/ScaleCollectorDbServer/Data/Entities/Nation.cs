namespace ScaleCollectorDbServer.Data.Entities
{
    public class Nation : TenantEntity
    {
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<ModelKit> Models { get; set; }
    }
}
