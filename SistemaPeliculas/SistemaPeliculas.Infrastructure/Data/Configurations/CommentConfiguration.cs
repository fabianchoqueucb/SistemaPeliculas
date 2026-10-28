using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Infrastructure.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.ToTable("comment");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Review).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ReviewId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Comment_Review");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Comment_User");
        }
    }
}
