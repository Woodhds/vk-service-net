using Microsoft.EntityFrameworkCore;
using VkService.Data.Entities;
using DbFunctionsExtensions = VkService.Data.Extensions.DbFunctionsExtensions;

namespace VkService.Data;

public class DataContext : DbContext
{
    public DbSet<VkUser> Users { get; set; }
    public DbSet<VkMessage> Messages { get; set; }
    public DbSet<VkMessageSearch> MessageSearch { get; set; }
    public DbSet<BannedGroup> BannedGroups { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VkMessage>(a =>
        {
            a.HasKey(r => r.MessageId);
            a.HasIndex(f => new { f.OwnerId, f.Id }).IsUnique();
            a.HasOne<VkMessageSearch>(e => e.Content)
                .WithOne(f => f.Message)
                .HasForeignKey<VkMessageSearch>(r => r.RowId);
        });
        
        modelBuilder.Entity<VkMessageSearch>(z =>
        {
            z.HasKey(r => r.RowId);
            z.ToTable("messages_search", e =>
                e.UseSqlReturningClause(false));
        });

        modelBuilder.HasDbFunction(typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Match)));
        
        base.OnModelCreating(modelBuilder);
    }
}
