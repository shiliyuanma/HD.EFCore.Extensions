using HD.EFCore.Extensions.Cache;
using HD.EFCore.Extensions.Test.Data.Mapping;
using HD.EFCore.Extensions.Test.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HD.EFCore.Extensions.Test.Data
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogMap());
            modelBuilder.ApplyConfiguration(new CommentMap());
        }

        public override int SaveChanges()
        {
            this.PreSaveChangesForEntityCache();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            this.PreSaveChangesForEntityCache();
            return base.SaveChangesAsync();
        }
    }
}
