using ScaleCollectorDbServer.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ScaleCollectorDbServer.Contracts.v1.Responses
{
    public class ModelKitResponse
    {
        public long Id { get; set; }

        public KitType Type { get; set; }

        public KitStatus Status { get; set; }

        [MaxLength(80)]
        public string ManufacturerArticleNumber { get; set; } = null!;

        public string Title { get; set; } = null!;

        public List<long> Reference { get; set; } = null!;

        public List<long> ReferenceOf { get; set; } = null!;

        public long ScaleId { get; set; }

        public long BrandId { get; set; }

        public long NationId { get; set; }
    }
}
