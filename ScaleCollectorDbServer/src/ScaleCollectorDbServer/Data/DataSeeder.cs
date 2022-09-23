using ScaleCollectorDbServer.Data.Entities;

namespace ScaleCollectorDbServer.Data
{
    public static class DataSeeder
    {
        public static void Seed(ScaleDbContext scaleDbContext)
        {
            SeedScales(scaleDbContext);
            SeedBrands(scaleDbContext);
        }

        private static void SeedBrands(ScaleDbContext scaleDbContext)
        {
            using var transaction = scaleDbContext.Database.BeginTransaction();
            scaleDbContext.EnableIdentityInsert<Brand>();

            Brand? brand;

            brand = scaleDbContext.Brands.Find(1L);
            if (brand == null)
                scaleDbContext.Brands.Add(new Brand() { Tenant = "global", Name = "Tamiya", Id = 1 });
            scaleDbContext.SaveChanges();

            scaleDbContext.DisableIdentityInsert<Brand>();
            transaction.Commit();
        }

        private static void SeedScales(ScaleDbContext scaleDbContext)
        {
            using var transaction = scaleDbContext.Database.BeginTransaction();
            scaleDbContext.EnableIdentityInsert<Scale>();

            Scale? scale;

            scale = scaleDbContext.Scales.Find(1L);
            if (scale == null)
                scaleDbContext.Scales.Add(new Scale(1, 35) { Tenant = "global", Id = 1 });

            scaleDbContext.SaveChanges();


            scaleDbContext.DisableIdentityInsert<Scale>();
            transaction.Commit();
        }
    }
}
