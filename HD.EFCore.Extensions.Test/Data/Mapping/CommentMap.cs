using HD.EFCore.Extensions.Test.Entity;
using Microsoft.EntityFrameworkCore;

namespace HD.EFCore.Extensions.Test.Data.Mapping
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Comment> entity)
        {
            entity.Property(e => e.Id).HasColumnType("int(11)");

            entity.Property(e => e.BlogId).HasColumnType("int(11)");

            entity.Property(e => e.Body)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.CreateTime).HasColumnType("datetime");

            entity.Property(e => e.ToUserId).HasColumnType("int(11)");

            entity.Property(e => e.UserId).HasColumnType("int(11)");

            //导航属性
            entity.HasOne(e => e.Blog).WithMany(e => e.Comments).HasForeignKey(e => e.BlogId).HasPrincipalKey(e => e.Id);
        }
    }
}
