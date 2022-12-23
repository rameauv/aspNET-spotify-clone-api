using Microsoft.EntityFrameworkCore;

namespace Repositories.Contexts;

public partial class SpotifyContext : DbContext
{
    public SpotifyContext()
    {
    }

    public SpotifyContext(DbContextOptions<SpotifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Spotify;Username=postgres;Password=a");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("Spotify", "uuid-ossp");

        modelBuilder.Entity<Album>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Album");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Artist");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("likes_pk");

            entity.HasIndex(e => e.Id, "likes_id_uindex").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.DeviceId).HasName("refreshtokens_pk");

            entity.HasIndex(e => e.DeviceId, "refreshtokens_deviceid_uindex").IsUnique();

            entity.HasIndex(e => e.Token, "refreshtokens_token_uindex").IsUnique();

            entity.Property(e => e.DeviceId).ValueGeneratedNever();
            entity.Property(e => e.Token).HasColumnName("token");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("refreshtokens_users_id_fk");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("song_pk");

            entity.ToTable("Song");

            entity.HasIndex(e => e.Id, "song_id_uindex").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.UserName, "users_username_uindex").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Data).HasColumnType("jsonb");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
