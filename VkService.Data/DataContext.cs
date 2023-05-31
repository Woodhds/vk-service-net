using Microsoft.EntityFrameworkCore;
using VkService.Data.Entities;

namespace VkService.Data;

public class DataContext : DbContext
{
    public DbSet<VkUser> Users { get; set; }
    public DbSet<VkMessage> Messages { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VkMessage>(a =>
        {
            a.HasKey(f => new { f.OwnerId, f.Id });
            a.OwnsOne<VkMessageSearch>(e => e.Content, q =>
            {
                q.WithOwner(r => r.Message).HasForeignKey(t => new { t.OwnerId, t.Id });
                q.ToTable("VkMessageSearch", e =>
                    e.UseSqlReturningClause(false));
            });

            a.Navigation(q => q.Content)
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}