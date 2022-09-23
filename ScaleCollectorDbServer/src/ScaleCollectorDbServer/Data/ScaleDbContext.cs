using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ScaleCollectorDbServer.Data.Entities;
using ScaleCollectorDbServer.Services.UserResolver;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ScaleCollectorDbServer.Data
{
    public class ExtendedScaleDbContext
    {
        public ScaleDbContext _context;
        public UserResolverService _userService;

        public ExtendedScaleDbContext(ScaleDbContext context, UserResolverService userService)
        {
            _context = context;
            _userService = userService;
            _context._currentUserExternalId = _userService.GetUser();
        }
    }

    public class ScaleDbContext : DbContext
    {
        internal string _currentUserExternalId;

        public DbSet<ModelKit> ModelKits { get; set; } = null!;
        public DbSet<Scale> Scales { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Nation> Nations { get; set; } = null!;
        private const int TenantLength = 32;


        public ScaleDbContext(DbContextOptions<ScaleDbContext> options) : base(options)
        {
            _currentUserExternalId = "";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // model kits
            modelBuilder.Entity<ModelKit>().HasKey(s => s.Id).IsClustered();
            modelBuilder.Entity<ModelKit>().Property(s => s.Tenant).HasMaxLength(TenantLength);
            modelBuilder.Entity<ModelKit>().HasIndex(s => s.Tenant).IsUnique(false);

            modelBuilder.Entity<ModelKit>().Property(s => s.ManufacturerArticleNumber).HasMaxLength(80).IsRequired(false);
            modelBuilder.Entity<ModelKit>().Property(s => s.Title).HasMaxLength(200).IsRequired(true);

            modelBuilder.Entity<ModelKit>().HasOne<Scale>(s => s.Scale).WithMany(s => s.Models).HasForeignKey(s => s.ScaleId).IsRequired();
            modelBuilder.Entity<ModelKit>().Navigation(e => e.Scale).AutoInclude();

            modelBuilder.Entity<ModelKit>().HasOne<Brand>(s => s.Brand).WithMany(s => s.Models).HasForeignKey(s => s.BrandId).IsRequired();
            modelBuilder.Entity<ModelKit>().Navigation(e => e.Brand).AutoInclude();

            modelBuilder.Entity<ModelKit>().HasOne<Nation>(s => s.Nation).WithMany(s => s.Models).HasForeignKey(s => s.NationId);
            modelBuilder.Entity<ModelKit>().Navigation(e => e.Nation).AutoInclude();

            modelBuilder.Entity<ModelKitReference>()
                .HasKey(e => new { e.ModelKitId, e.ReferenceId });

            modelBuilder.Entity<ModelKitReference>()
                      .HasOne(pt => pt.ModelKit)
                      .WithMany(p => p.ReferenceOf)
                      .HasForeignKey(pt => pt.ModelKitId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ModelKitReference>()
                .HasOne(pt => pt.Reference)
                .WithMany(t => t.Reference)
                .HasForeignKey(pt => pt.ReferenceId);

            modelBuilder.Entity<ModelKit>().Navigation(e => e.Reference).AutoInclude();
            modelBuilder.Entity<ModelKit>().Navigation(e => e.ReferenceOf).AutoInclude();

            // scales
            modelBuilder.Entity<Scale>().HasKey(s => s.Id).IsClustered();
            modelBuilder.Entity<Scale>().Property(s => s.Tenant).HasMaxLength(TenantLength);
            modelBuilder.Entity<Scale>().Property(s => s.RatioText).HasMaxLength(10);
            modelBuilder.Entity<Scale>().HasIndex(s => new { s.Tenant, s.RatioText }).IsUnique(true);

            // brands
            modelBuilder.Entity<Brand>().HasKey(s => s.Id).IsClustered();
            modelBuilder.Entity<Brand>().Property(s => s.Tenant).HasMaxLength(TenantLength);
            modelBuilder.Entity<Brand>().Property(s => s.Name).HasMaxLength(200);

            // nations
            modelBuilder.Entity<Nation>().HasKey(s => s.Id).IsClustered();
            modelBuilder.Entity<Nation>().Property(s => s.Tenant).HasMaxLength(TenantLength);
            modelBuilder.Entity<Nation>().Property(s => s.Name).HasMaxLength(200);

            modelBuilder.ApplyGlobalFilters<BaseEntity>(e => e.IsDeleted == false);
            modelBuilder.ApplyGlobalFilters<TenantEntity>(e => e.Tenant == _currentUserExternalId || e.Tenant == "global");
            
        }
        
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var changes = this.ChangeTracker.Entries().Where(e => e.Entity is TenantEntity && (e.State == EntityState.Added)).Select(e => e.Entity as TenantEntity);
            foreach (var item in changes)
            {
                if (item != null && string.IsNullOrWhiteSpace(item.Tenant))
                    item.Tenant = _currentUserExternalId;
            }

            int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                       
            return result;
        }
    }

    public static class DbContextExtensions
    {
        public static Task EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: true);
        public static Task DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: false);

        private static Task SetIdentityInsert<T>(DbContext context, bool enable)
        {
            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            return context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
        }

        public static void ApplyGlobalFilters<TBaseClass>(this ModelBuilder modelBuilder, Expression<Func<TBaseClass, bool>> expression)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableTo(typeof(TBaseClass)))
                {
                    var newParam = Expression.Parameter(entityType.ClrType);
                    var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(newBody, newParam));
                }
            }
        }
    }
}
