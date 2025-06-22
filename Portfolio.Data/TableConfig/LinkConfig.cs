using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Data.Entities;

namespace Portfolio.Data.TableConfig
{
    public class LinkConfig : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder.HasOne(L => L.PortfolioUser)
                .WithMany(L => L.Links)
                .HasForeignKey(Links => Links.PortfolioUserId);
        }
    }
}
