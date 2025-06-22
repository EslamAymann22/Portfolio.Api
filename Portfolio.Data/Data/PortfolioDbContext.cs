using Microsoft.EntityFrameworkCore;
using Portfolio.Data.Entities;
using System.Reflection;

namespace Portfolio.Data.Data
{
    public class PortfolioDbContext : DbContext
    {

        public DbSet<PortfolioUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Link> SocialLinks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Tool> Tools { get; set; }


        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}