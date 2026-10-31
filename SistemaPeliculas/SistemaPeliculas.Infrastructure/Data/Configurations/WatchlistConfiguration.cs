using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Infrastructure.Data.Configurations
{
    public class WatchlistConfiguration : IEntityTypeConfiguration<Watchlist>
    {
        public void Configure(EntityTypeBuilder<Watchlist> builder)
        {
            builder.ToTable("watchlist");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.DateAdded)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(e => new { e.UserId, e.MovieId })
                .IsUnique()
                .HasDatabaseName("UK_Watchlist_User_Movie");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Watchlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Movie)
                .WithMany(p => p.Watchlists)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

