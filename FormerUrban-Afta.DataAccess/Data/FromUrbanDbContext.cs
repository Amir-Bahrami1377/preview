using FormerUrban_Afta.DataAccess.Configurations.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FormerUrban_Afta.DataAccess.Data
{
    public class FromUrbanDbContext : IdentityDbContext<
        CostumIdentityUser,
        CostumIdentityRole,
        string,
        IdentityUserClaim<string>,
        ApplicationUserRole,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>
    >
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;
        //private readonly IEventLogThresholdService _eventLogThresholdService;

        public FromUrbanDbContext(DbContextOptions<FromUrbanDbContext> options, IHttpContextAccessor httpContextAccessor/*, IEventLogThresholdService eventLogThresholdService*/)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            //_eventLogThresholdService = eventLogThresholdService;
        }

        // ✅ Constructor without IHttpContextAccessor for design-time migrations
        public FromUrbanDbContext(DbContextOptions<FromUrbanDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new AddAuditDataInterceptor(_httpContextAccessor/*, _eventLogThresholdService*/));

            //.AddInterceptors(new CancelTagedQuery());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configure custom UserRole entity

            builder.Entity<CostumIdentityUser>(entity =>
            {
                entity.Property(e => e.Hashed);
            });

            builder.Entity<CostumIdentityRole>(entity =>
            {
                entity.Property(e => e.Hashed);
            });

            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.Property(e => e.Hashed);
            });
        }

        public DbSet<ApplicationUserRole> UserRoles { get; set; }
        public DbSet<Tarifha> Tarifha { get; set; }
        public DbSet<Parvandeh> Parvandeh { get; set; }
        public DbSet<Melk> Melk { get; set; }
        public DbSet<Sakhteman> Sakhteman { get; set; }
        public DbSet<Apartman> Apartman { get; set; }
        public DbSet<Darkhast> Darkhast { get; set; }
        public DbSet<Dv_karbari> Dv_karbari { get; set; }
        public DbSet<Dv_malekin> Dv_malekin { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<EventLogFilter> EventLogFilter { get; set; }
        public DbSet<EventLogThreshold> EventLogThreshold { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<Dv_savabegh> Dv_savabegh { get; set; }
        public DbSet<Erja> Erja { get; set; }
        public DbSet<Expert> Expert { get; set; }
        public DbSet<ControlMap> ControlMap { get; set; }
        public DbSet<Estelam> Estelam { get; set; }
        public DbSet<Parvaneh> Parvaneh { get; set; }
        public DbSet<WeakPassword> WeakPassword { get; set; }
        public DbSet<LogSMS> LogSMS { get; set; }
        public DbSet<BlockedIPRange> BlockedIPRange { get; set; }
        public DbSet<AllowedIPRange> AllowedIPRange { get; set; }
        public DbSet<UserLogined> UserLogined { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<ActivityLogFilters> ActivityLogFilters { get; set; }
        public DbSet<RoleRestriction> RoleRestrictions { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditFilter> AuditFilters { get; set; }
    }
}
