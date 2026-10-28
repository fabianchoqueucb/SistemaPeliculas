using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Infrastructure.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> entity)
        {
            entity.ToTable("movie");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Title).IsUnique();

            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.ReleaseDate).HasColumnType("date");
            entity.Property(e => e.Length).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Genre).HasMaxLength(30).IsRequired();
        }
    }
}
