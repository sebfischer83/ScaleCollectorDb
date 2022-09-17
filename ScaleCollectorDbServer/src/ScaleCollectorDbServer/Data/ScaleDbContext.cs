using Microsoft.EntityFrameworkCore;
using ScaleCollectorDbServer.Data.Entities;
using ScaleCollectorDbServer.Services.UserResolver;

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

        public ScaleDbContext(DbContextOptions<ScaleDbContext> options) : base(options)
        {
            _currentUserExternalId = "";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // model kits
            modelBuilder.Entity<ModelKit>().HasKey(s => s.Id).IsClustered();
            modelBuilder.Entity<ModelKit>().Property(s => s.Tenant).HasMaxLength(32);

            modelBuilder.Entity<ModelKit>().HasIndex(s => s.Tenant).IsUnique(false);
            modelBuilder.Entity<ModelKit>().HasQueryFilter(a => a.IsDeleted == false);
            modelBuilder.Entity<ModelKit>().HasQueryFilter(a => a.Tenant == _currentUserExternalId);
            modelBuilder.Entity<ModelKit>().HasOne<Scale>(s => s.Scale).WithMany(s => s.Models).HasForeignKey(s => s.ScaleId);

            // scales
            modelBuilder.Entity<Scale>().HasKey(s => s.Id).IsClustered();
            modelBuilder.Entity<Scale>().Property(s => s.Tenant).HasMaxLength(32);
            modelBuilder.Entity<Scale>().Property(s => s.RatioText).HasMaxLength(10);
            modelBuilder.Entity<Scale>().HasIndex(s => new { s.Tenant, s.RatioText }).IsUnique(true);

            modelBuilder.Entity<Scale>().HasQueryFilter(a => a.IsDeleted == false);
            modelBuilder.Entity<Scale>().HasQueryFilter(a => a.Tenant == _currentUserExternalId);
        }
    }
}
