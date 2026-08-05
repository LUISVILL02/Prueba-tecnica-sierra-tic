using Microsoft.EntityFrameworkCore;
using To_do_list.Data.Entities;

namespace To_do_list.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<TaskItem>  TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("TaskItem");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Title).HasColumnType("text").IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt);
            entity.Property(e => e.IsCompleted);
        });
    }
}