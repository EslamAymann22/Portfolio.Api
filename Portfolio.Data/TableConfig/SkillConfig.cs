using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Data.Entities;

namespace Portfolio.Data.TableConfig
{
    public class SkillConfig : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasOne(S => S.portfolioUser)
                .WithMany(P => P.Skills)
                .HasForeignKey(S => S.PortfolioUserId);

            builder.HasMany(S => S.Tags)
                .WithOne(T => T.Skill)
                .HasForeignKey(T => T.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
