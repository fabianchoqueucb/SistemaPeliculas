using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Infrastructure.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> entity)
        {
            entity.ToTable("review");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Grade).HasColumnType("float");

            entity.HasOne(d => d.Movie).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Review_Movie");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Review_User");
        }
    }
}
