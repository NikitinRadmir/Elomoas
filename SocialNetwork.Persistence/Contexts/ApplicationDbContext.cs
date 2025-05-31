using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Elomoas.Domain.Entities;
using Elomoas.Persistence.Configurations;
//using Elomoas.Domain.Entities;

namespace Elomoas.Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        //public DbSet<Post> Posts { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new GroupsConfiguration());
            modelBuilder.ApplyConfiguration(new CoursesConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        }
    }
}
