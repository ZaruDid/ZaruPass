using Microsoft.EntityFrameworkCore;

namespace ZaruPass.Core.DataBase.Migrations;

public partial class MyContext : DbContext
{
    public MyContext()
    {;
    }

    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Password> Passwords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite($"Data Source=db.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Password>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pass).HasColumnName("password");
            entity.Property(e => e.Site).HasColumnName("site");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
