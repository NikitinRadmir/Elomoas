using Microsoft.EntityFrameworkCore;
//using Elomoas.Domain.Entities;

namespace Elomoas.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        //public DbSet<Post> Posts { get; set; }
       


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new PostsConfiguration());
            
        }
    }
}
