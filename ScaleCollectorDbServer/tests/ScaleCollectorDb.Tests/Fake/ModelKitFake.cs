using Bogus;
using ScaleCollectorDbServer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleCollectorDb.Tests.Fake
{
    internal class ModelKitFake
    {
        public static Faker<ModelKit> GetFake()
        {
            var fake = new Faker<ModelKit>()
                .RuleFor(m => m.IsDeleted, f => false)
                .RuleFor(m => m.BrandId, f => f.Random.Long(1))
                .RuleFor(m => m.Type, f => f.Random.Enum<KitType>())
                .RuleFor(m => m.Status, f => f.Random.Enum<KitStatus>())
                .RuleFor(m => m.ManufacturerArticleNumber, f => f.Commerce.Ean13())
                .RuleFor(m => m.Title, f => f.Name.JobTitle())
                .RuleFor(m => m.ScaleId, f => f.Random.Long(1))
                .RuleFor(m => m.Id, f => f.Random.Long(1));
            
            return fake;
        }
    }
}
