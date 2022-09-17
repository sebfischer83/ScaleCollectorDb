namespace ScaleCollectorDbServer.Data.Entities
{
    public abstract class TenantEntity : BaseEntity
    {
        public string Tenant { get; set; }

        protected TenantEntity()
        {
            Tenant = "";
        }
    }
}
