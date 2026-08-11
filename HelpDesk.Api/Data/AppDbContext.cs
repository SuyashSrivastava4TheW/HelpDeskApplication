using HelpDesk.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Data;

/// <summary>
/// EF Core Code-First database context. This is the only class in the
/// solution that is allowed to know about EF Core's DbSet/SaveChanges API —
/// everything above it (repository, service, controller) talks in terms of
/// plain POCOs and Task, never DbContext directly.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Tickets");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(t => t.Priority)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(t => t.RaisedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Speeds up the required GetTicketsByStatusAsync / dashboard counts / filter feature.
            entity.HasIndex(t => t.Status);
        });
    }
}
