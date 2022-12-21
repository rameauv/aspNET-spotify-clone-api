using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyIdentity.Contexts;

public partial class SpotifyContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public SpotifyContext()
    {
    }

    public SpotifyContext(DbContextOptions<SpotifyContext> options)
        : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Spotify;Username=postgres;Password=a");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("Spotify", "uuid-ossp");

        // modelBuilder.Entity<User>(entity =>
        // {
        //     entity.Property(e => e.Id).ValueGeneratedNever();
        // });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.HasKey(e => e.UserId);
        });
        
        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.HasKey(e => e.UserId);
        });
        
        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.HasKey(e => e.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
