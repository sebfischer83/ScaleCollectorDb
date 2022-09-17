namespace ScaleCollectorDbServer.Data.Entities
{
    public class Brand : TenantEntity
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<ModelKit> Models { get; set; } = null!;
    }
}
