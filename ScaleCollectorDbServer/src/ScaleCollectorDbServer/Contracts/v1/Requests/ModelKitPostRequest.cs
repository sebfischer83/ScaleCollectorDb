using ScaleCollectorDbServer.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ScaleCollectorDbServer.Contracts.v1.Requests
{
    public class ModelKitPostRequest
    {
        [Required()]
        public KitType Type { get; set; }

        [Required()]
        public KitStatus Status { get; set; }

        [MaxLength(80)]
        public string ManufacturerArticleNumber { get; set; } = null!;

        [MaxLength(200), Required()]
        public string Title { get; set; } = null!;

        [Required()]
        public long ScaleId { get; set; }

        [Required()]
        public long BrandId { get; set; }

        public long NationId { get; set; }
    }
}
