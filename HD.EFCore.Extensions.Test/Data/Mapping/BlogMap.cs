using HD.EFCore.Extensions.Test.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HD.EFCore.Extensions.Test.Data.Mapping
{
    public class BlogMap : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> entity)
        {
            entity.Property(e => e.Id).HasColumnType("int(11)");

            entity.Property(e => e.Body)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(e => e.CreateTime).HasColumnType("datetime");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.CommentCount).HasColumnType("int(11)");

            entity.Property(e => e.UserId).HasColumnType("int(11)");
        }
    }
}
