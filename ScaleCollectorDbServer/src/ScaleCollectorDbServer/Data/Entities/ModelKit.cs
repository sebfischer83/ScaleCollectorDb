namespace ScaleCollectorDbServer.Data.Entities
{
    public class ModelKit : TenantEntity
    {
        /// <summary>
        /// The type of the kit.
        /// </summary>
        public KitType Type { get; set; }

        public KitStatus Status { get; set; }

        /// <summary>
        /// The item number that the manufacturer uses for this kit.
        /// </summary>
        public string? ManufacturerArticleNumber { get; set; } = null!;

        /// <summary>
        /// The title of the kit, e.g. Jagdpanzer 38(t).
        /// </summary>
        public string Title { get; set; } = null!;

        public virtual ICollection<ModelKitReference> Reference { get; set; } = null!;
        public virtual ICollection<ModelKitReference> ReferenceOf { get; set; } = null!;

        public long ScaleId { get; set; }
        public virtual Scale Scale { get; set; } = null!;

        public long BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;

        public long NationId { get; set; }
        public virtual Nation Nation { get; set; } = null!;

        public virtual ICollection<Image> Images { get; set; } = null!;
    }
}
