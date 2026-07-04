using CyberInsight.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberInsight.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ThreatAlert> ThreatAlerts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasMany(e => e.Subscriptions)
                    .WithOne(s => s.User)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.ThreatAlerts)
                    .WithOne(t => t.User)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Subscription configuration
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StripeCustomerId).HasMaxLength(255);
                entity.Property(e => e.StripeSubscriptionId).HasMaxLength(255);
                entity.Property(e => e.Plan).HasConversion<string>();
            });

            // ThreatAlert configuration
            modelBuilder.Entity<ThreatAlert>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Severity).HasConversion<string>();
                entity.Property(e => e.ThreatType).HasMaxLength(100);
                entity.HasIndex(e => e.DetectedAt);
            });

            // AuditLog configuration
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Action).HasMaxLength(255);
                entity.HasIndex(e => e.CreatedAt);
            });
        }
    }
}