using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Data.Entities;

namespace Portfolio.Data.TableConfig
{
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasOne(P => P.PortfolioUser)
                 .WithMany(Port => Port.Projects)
                 .HasForeignKey(P => P.PortfolioUserId);

            builder.HasMany(p => p.Tools)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);

        }
    }
}
